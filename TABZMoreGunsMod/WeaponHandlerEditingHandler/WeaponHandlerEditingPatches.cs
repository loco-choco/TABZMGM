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

            var WH_postfix = new HarmonyMethod(typeof(WeaponHandlerEditingPatches).GetMethod(nameof(WeaponHandlerEditingPatches.WeaponHandlerStart_Postfix)));

            harmonyInstance.Patch(WeaponHandlerStart, postfix: WH_postfix);
        }
        static public bool loadedCustomSounds = false;
        public static void WeaponHandlerStart_Postfix(WeaponHandler __instance)
        {
            if (!loadedCustomSounds)
            {
                WeaponHandlerEditingHelper.LoadCustomGunSoundsToManager();
                loadedCustomSounds = true;
            }

            Debug.Log("Loading Custom Weapons");
            var weaponsToAdd = WeaponHandlerEditingHelper.weaponsToAdd;
            var weaponsWrappers = new WeaponHandler.WeaponWrapper[weaponsToAdd.Count];
            for (int i = 0; i < weaponsWrappers.Length; i++)
            {
                try
                {
                    Weapon weapon = weaponsToAdd[i].WeaponSpawningMethod(__instance.transform);
                    weapon.gameObject.SetActive(false);
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
