using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
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
            Harmony harmonyInstance = new Harmony(id);

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
        private void ApplyPatch(Harmony harmonyInstance, IMonitor monitor)
        {
            var patchDescriptor = this.GetPatchDescriptor();

            // get target method
            MethodBase targetMethod = string.IsNullOrEmpty(patchDescriptor.TargetMethodName)
                ? AccessTools.Constructor(patchDescriptor.TargetType, patchDescriptor.TargetMethodArguments ?? Type.EmptyTypes)
                : AccessTools.Method(patchDescriptor.TargetType, patchDescriptor.TargetMethodName, patchDescriptor.TargetMethodArguments);
            if (targetMethod == null)
            {
                monitor.Log($"Can't apply the {this.GetType().Name} patch: required method '{patchDescriptor.TargetType?.FullName}.{patchDescriptor.TargetMethodName ?? "ctor"}' not found. The mod may not work correctly.", LogLevel.Error);
                return;
            }

            // get patch methods
            MethodInfo prefix = AccessTools.Method(this.GetType(), "Prefix");
            MethodInfo postfix = AccessTools.Method(this.GetType(), "Postfix");

            // apply patches
            try
            {
                harmonyInstance.Patch(
                    original: targetMethod,
                    prefix: prefix != null ? new HarmonyMethod(prefix) : null,
                    postfix: postfix != null ? new HarmonyMethod(postfix) : null
                );
            }
            catch (Exception ex)
            {
                monitor.Log($"Can't apply the {this.GetType().Name} patch. The mod may not work correctly.\n\n{ex}", LogLevel.Error);
            }
        }
    }
}
