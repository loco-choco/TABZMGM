using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using System.Reflection.Emit;

namespace TABZMoreGunsMod.RuntimeResources
{
    public class ResourceHandlerPatches
    {
        public static void Patches(Harmony harmonyInstance)
        {
            //Resorces.Load patches
            var NetworkingPeerInstantiate = AccessTools.Method(AccessTools.TypeByName("NetworkingPeer"), "DoInstantiate");
            var PhotonNetworkInstantiateSceneObject = typeof(PhotonNetwork).GetMethod(nameof(PhotonNetwork.InstantiateSceneObject));
            var PhotonNetworkInstantiate = typeof(PhotonNetwork).GetMethod(nameof(PhotonNetwork.Instantiate), new Type[] { typeof(string), typeof(Vector3), typeof(Quaternion), typeof(int), typeof(object[]) });

            var transpiler = new HarmonyMethod(typeof(InstantiatePatches).GetMethod(nameof(InstantiatePatches.InstantiateTranspiler)));

            harmonyInstance.Patch(NetworkingPeerInstantiate, transpiler: transpiler);
            harmonyInstance.Patch(PhotonNetworkInstantiateSceneObject, transpiler: transpiler);
            harmonyInstance.Patch(PhotonNetworkInstantiate, transpiler: transpiler);

            try
            {
                var transpilerShallowCopy = new HarmonyMethod(typeof(InstantiateCopyPatches).GetMethod(nameof(InstantiateCopyPatches.InstantiateCopyTranspiler)));
                harmonyInstance.Patch(NetworkingPeerInstantiate, transpiler: transpilerShallowCopy); //Corrigir
            }
            catch (Exception ex)
            {
                Debug.Log("Erro: " + ex.Message + "  " + ex.Source + "   " + ex.StackTrace + "  " + ex.InnerException.Message);
            }
        }
    }
    public class InstantiatePatches
    {
        public static IEnumerable<CodeInstruction> InstantiateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            int index = -1;
            var codes = new List<CodeInstruction>(instructions);
            bool isNetworkingPeerInstantiate = false;
            for (var i = 0; i < codes.Count; i++)
            {
                if ((codes[i].opcode == OpCodes.Ldarg_0 || codes[i].opcode == OpCodes.Ldloc_0) && codes[i + 1].opcode == OpCodes.Ldtoken && codes[i + 2].opcode == OpCodes.Call
                    && codes[i + 3].opcode == OpCodes.Call && codes[i + 4].opcode == OpCodes.Castclass && (codes[i + 5].opcode == OpCodes.Stloc_0 || codes[i + 5].opcode == OpCodes.Starg_S))//ldc.r4
                {
                    index = i + 6;
                    if (codes[i].opcode == OpCodes.Ldloc_0)
                        isNetworkingPeerInstantiate = true;
                    break;
                }
            }
            if (index > -1)
            {
                if (!isNetworkingPeerInstantiate)
                {
                    codes.Insert(index, new CodeInstruction(OpCodes.Ldarg_0));
                    codes.Insert(index + 1, HarmonyFix.Call(typeof(RuntimeResourcesHandler), "InstantiateGameObject", new Type[] { typeof(string) }));
                    codes.Insert(index + 2, new CodeInstruction(OpCodes.Stloc_0));
                }
                else
                {
                    codes.Insert(index, new CodeInstruction(OpCodes.Ldloc_0));
                    codes.Insert(index + 1, HarmonyFix.Call(typeof(RuntimeResourcesHandler), "InstantiateGameObject", new Type[] { typeof(string) }));
                    codes.Insert(index + 2, codes[index - 1].Clone());//Copied from the last OpCode in the search
                }
                Debug.Log("Criar os CodeInstructions de InstantiatePatches foi um sucesso");
            }
            Debug.Log("O Transpiler foi rodado");

            return codes.AsEnumerable();
        }
    }

    public class InstantiateCopyPatches
    {
        public static IEnumerable<CodeInstruction> InstantiateCopyTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            int index1 = -1;
            int index2 = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (index1 <= -1
                    && codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldfld && codes[i + 2].opcode == OpCodes.Ldloc_0
                    && codes[i + 3].opcode == OpCodes.Ldloc_3 && codes[i + 4].opcode == OpCodes.Ldloc_S && codes[i + 5].opcode == OpCodes.Callvirt
                    && codes[i + 6].opcode == OpCodes.Stloc_S)
                {
                    index1 = i + 7;
                }
                else if (index2 <= -1
                    && codes[i].opcode == OpCodes.Ldarg_3 && codes[i + 1].opcode == OpCodes.Ldloc_3 && codes[i + 2].opcode == OpCodes.Ldloc_S
                    && codes[i + 3].opcode == OpCodes.Call && codes[i + 4].opcode == OpCodes.Stloc_S)
                {
                    index2 = i + 5;
                }
            }

            if (index1 > -1)
            {

                CodeInstruction instructionForLoadObjectPool = codes[index1 - 6].Clone();

                codes.Insert(index1, new CodeInstruction(OpCodes.Ldarg_0));
                codes.Insert(index1 + 1, instructionForLoadObjectPool);//ObjectPool
                codes.Insert(index1 + 2, new CodeInstruction(OpCodes.Ldloc_0));
                codes.Insert(index1 + 3, new CodeInstruction(OpCodes.Ldloc_3));
                codes.Insert(index1 + 4, codes[index1 - 3].Clone()); //ldloc.s   rotation
                codes.Insert(index1 + 5, HarmonyFix.Call(typeof(RuntimeResourcesHandler), "ShallowCopyOfGameObject", new Type[] { typeof(IPunPrefabPool), typeof(string), typeof(Vector3), typeof(Quaternion) }));
                codes.Insert(index1 + 6, codes[index1 - 1].Clone()); //stloc.s   gameObject

                codes.RemoveRange(index1 - 7, 7);
            }
            if (index2 > -1)
            {
                codes.Insert(index2, new CodeInstruction(OpCodes.Ldarg_3));
                codes.Insert(index2 + 1, new CodeInstruction(OpCodes.Ldloc_3));
                codes.Insert(index2 + 2, codes[index2 - 3].Clone()); //ldloc.s   rotation
                codes.Insert(index2 + 3, HarmonyFix.Call(typeof(RuntimeResourcesHandler), "ShallowCopyFrom", new Type[] { typeof(GameObject), typeof(Vector3), typeof(Quaternion) }));
                codes.Insert(index2 + 4, codes[index2 - 1].Clone()); //stloc.s   gameObject2
                codes.RemoveRange(index2 - 5, 5);
            }

            Debug.Log("Criar os CodeInstructions de InstantiateCopyPatches foi um sucesso");
            Debug.Log("O Transpiler foi rodado");

            return codes;
        }
    }

    public class HarmonyFix
    {
        // --- CALLING

        /// <summary>Creates a CodeInstruction calling a method (CALL)</summary>
        /// <param name="type">The class/type where the method is declared</param>
        /// <param name="name">The name of the method (case sensitive)</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A code instruction that calls the method matching the arguments</returns>
        ///
        public static CodeInstruction Call(Type type, string name, Type[] parameters = null, Type[] generics = null)
        {
            var method = AccessTools.Method(type, name, parameters, generics);
            if (method is null) throw new ArgumentException($"No method found for type={type}, name={name}, parameters={parameters.Description()}, generics={generics.Description()}");
            return new CodeInstruction(OpCodes.Call, method);
        }

        /// <summary>Creates a CodeInstruction calling a method (CALL)</summary>
        /// <param name="typeColonMethodname">The target method in the form <c>TypeFullName:MethodName</c>, where the type name matches a form recognized by <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.gettype">Type.GetType</a> like <c>Some.Namespace.Type</c>.</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A code instruction that calls the method matching the arguments</returns>
        ///
        public static CodeInstruction Call(string typeColonMethodname, Type[] parameters = null, Type[] generics = null)
        {
            var method = AccessTools.Method(typeColonMethodname, parameters, generics);
            if (method is null) throw new ArgumentException($"No method found for {typeColonMethodname}, parameters={parameters.Description()}, generics={generics.Description()}");
            return new CodeInstruction(OpCodes.Call, method);
        }


        // --- FIELDS

        /// <summary>Creates a CodeInstruction loading a field (LD[S]FLD[A])</summary>
        /// <param name="type">The class/type where the field is defined</param>
        /// <param name="name">The name of the field (case sensitive)</param>
        /// <param name="useAddress">Use address of field</param>
        /// <returns></returns>
        public static CodeInstruction LoadField(Type type, string name, bool useAddress = false)
        {

            var field = AccessTools.Field(type, name);
            if (field is null) throw new ArgumentException($"No field found for {type} and {name}");
            return new CodeInstruction(useAddress ? (field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda) : (field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld), field);
        }



        /// <summary>Creates a CodeInstruction storing to a field (ST[S]FLD)</summary>
        /// <param name="type">The class/type where the field is defined</param>
        /// <param name="name">The name of the field (case sensitive)</param>
        /// <returns></returns>
        public static CodeInstruction StoreField(Type type, string name)
        {
            var field = AccessTools.Field(type, name);
            if (field is null) throw new ArgumentException($"No field found for {type} and {name}");
            return new CodeInstruction(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
        }

    }
}
