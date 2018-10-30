using System;
using System.Linq;
using System.Reflection;
using Harmony;

namespace FunnySnek.AntiCheat.Server
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
        public static void PatchAll(string id)
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(id);

            var types = (
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.IsClass && type.BaseType == typeof(Patch)
                select type
            );
            foreach (Type type in types)
                ((Patch)Activator.CreateInstance(type)).ApplyPatch(harmonyInstance);
        }


        /*********
        ** Private methods
        *********/
        private void ApplyPatch(HarmonyInstance harmonyInstance)
        {
            var patchDescriptor = this.GetPatchDescriptor();

            MethodBase targetMethod = string.IsNullOrEmpty(patchDescriptor.TargetMethodName)
                ? (MethodBase)patchDescriptor.TargetType.GetConstructor(patchDescriptor.TargetMethodArguments ?? new Type[0])
                : (patchDescriptor.TargetMethodArguments != null
                    ? patchDescriptor.TargetType.GetMethod(patchDescriptor.TargetMethodName, patchDescriptor.TargetMethodArguments)
                    : patchDescriptor.TargetType.GetMethod(patchDescriptor.TargetMethodName, (BindingFlags)62)
                );

            harmonyInstance.Patch(targetMethod, new HarmonyMethod(this.GetType().GetMethod("Prefix")), new HarmonyMethod(this.GetType().GetMethod("Postfix")));
        }


        protected class PatchDescriptor
        {
            /*********
            ** Accessors
            *********/
            public Type TargetType;
            public string TargetMethodName;
            public Type[] TargetMethodArguments;


            /*********
            ** Public methods
            *********/
            /// <param name="targetType">Don't use typeof() or it won't work on other platforms</param>
            /// <param name="targetMethodName">Null if constructor is desired</param>
            /// <param name="targetMethodArguments">Null if no method abiguity</param>
            public PatchDescriptor(Type targetType, string targetMethodName, Type[] targetMethodArguments = null)
            {
                this.TargetType = targetType;
                this.TargetMethodName = targetMethodName;
                this.TargetMethodArguments = targetMethodArguments;
            }
        }
    }
}
