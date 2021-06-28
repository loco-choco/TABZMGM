using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TABZMoreGunsMod
{
    public class TABZChatPatches
    {
        public static void Patches(Harmony harmonyInstance)
        {
            var WeaponHandlerStart = AccessTools.Method(typeof(TABZChat), "Enter");

            var WH_prefix = new HarmonyMethod(typeof(TABZChatPatches).GetMethod(nameof(TABZChatPatches.TABZChatEnter_Prefix)));

            harmonyInstance.Patch(WeaponHandlerStart, prefix: WH_prefix);
        }
        public static void TABZChatEnter_Prefix(TABZChat __instance)
        {
            InputField input = AccessTools.FieldRefAccess<TABZChat, InputField>(__instance, "mInputField");
            if (input != null)
            {
                string text = input.text;
                if (text.StartsWith("m-"))
                    PhotonNetwork.Instantiate(text.TrimStart('m', '-'), NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig.position, Quaternion.identity, 0, new object[]
                    {
                        -1
                    });
            }
        }
    }
}
