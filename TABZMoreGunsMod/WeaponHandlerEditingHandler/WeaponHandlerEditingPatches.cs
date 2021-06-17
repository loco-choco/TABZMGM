using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;


namespace TABZMoreGunsMod.WeaponHandlerEditingHandler
{
    public class WeaponHandlerEditingPatches
    {
        public static void Patches(Harmony harmonyInstance)
        {
            var WeaponHandlerStart = AccessTools.Method(typeof(WeaponHandler), "Start");

            var postfix = new HarmonyMethod(typeof(WeaponHandlerEditingPatches).GetMethod(nameof(WeaponHandlerEditingPatches.Postfix)));

            harmonyInstance.Patch(WeaponHandlerStart, postfix: postfix);
        }

        public static void Postfix(WeaponHandler __instance)
        {
            Debug.Log("Loading Custom Weapons");
            var weaponsToAdd = WeaponHandlerEditingHelper.weaponsToAdd;
            var weaponsWrappers = new WeaponHandler.WeaponWrapper[weaponsToAdd.Count];
            for (int i = 0; i < weaponsWrappers.Length; i++)
            {
                try
                {
                    Weapon weapon = weaponsToAdd[i].WeaponSpawningMethod(__instance.transform);
                    weapon.gameObject.SetActive(false);
                    //weapon.GetComponent<PhotonView>().viewID = PhotonNetwork.AllocateViewID(); < fonte do problema, não é assim que é sincronizado entre players >:(
                    weaponsWrappers[i] = new WeaponHandler.WeaponWrapper
                    {
                        m_item = weaponsToAdd[i].Item,
                        m_weapon = weapon
                    };
                }
                catch
                {
                    Debug.Log(string.Format("Couldn't add the weapon from the item {0}", weaponsToAdd[i].Item.DisplayName));
                }
            }
            __instance.m_weaponList = __instance.m_weaponList.AddRangeToArray(weaponsWrappers);
            Debug.Log("Customs weapons loaded");
        }
    }
}
