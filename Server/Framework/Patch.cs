using System;
using System.Linq;
using System.Reflection;
using Harmony;
using StardewModdingAPI;

namespace FunnySnek.AntiCheat.Server.Framework
{
    //Remember Prefix/Postfix should be public and static! Do not use lambdas
    internal abstract class Patch
    {
        /*********
        ** Properties
        *********/
        protected abstract PatchDescriptor GetPatchDescriptor();


        /*********
        ** Public methods
        *********/
        public static void PatchAll(string id, IMonitor monitor)
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(id);

            var types = (
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.IsClass && type.BaseType == typeof(Patch)
                select type
            );
            foreach (Type type in types)
                ((Patch)Activator.CreateInstance(type)).ApplyPatch(harmonyInstance, monitor);
        }


        /*********
        ** Private methods
        *********/
        private void ApplyPatch(HarmonyInstance harmonyInstance, IMonitor monitor)
        {
            var patchDescriptor = this.GetPatchDescriptor();

            // get target method
            MethodBase targetMethod = string.IsNullOrEmpty(patchDescriptor.TargetMethodName)
                ? (MethodBase)patchDescriptor.TargetType.GetConstructor(patchDescriptor.TargetMethodArguments ?? new Type[0])
                : (patchDescriptor.TargetMethodArguments != null
                    ? patchDescriptor.TargetType.GetMethod(patchDescriptor.TargetMethodName, patchDescriptor.TargetMethodArguments)
                    : patchDescriptor.TargetType.GetMethod(patchDescriptor.TargetMethodName, (BindingFlags)62)
                );
            if (targetMethod == null)
            {
                monitor.Log($"Can't apply the {this.GetType().Name} patch: required method '{patchDescriptor.TargetType?.FullName}.{patchDescriptor.TargetMethodName ?? "ctor"}' not found. The mod may not work correctly.", LogLevel.Error);
                return;
            }

            // apply patches
            try
            {
                harmonyInstance.Patch(
                    original: targetMethod,
                    prefix: new HarmonyMethod(this.GetType().GetMethod("Prefix")),
                    postfix: new HarmonyMethod(this.GetType().GetMethod("Postfix"))
                );
            }
            catch (Exception ex)
            {
                monitor.Log($"Can't apply the {this.GetType().Name} patch. The mod may not work correctly.\n\n{ex}", LogLevel.Error);
            }
        }
    }
}
