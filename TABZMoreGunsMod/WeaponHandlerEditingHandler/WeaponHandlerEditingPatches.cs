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
            var WeaponHandlerAwake = AccessTools.Method(typeof(WeaponHandler), "Awake");
            
            var WH_Awake_postfix = new HarmonyMethod(typeof(WeaponHandlerEditingPatches).GetMethod(nameof(WeaponHandlerEditingPatches.WeaponHandlerAwake_Postfix)));
            
            harmonyInstance.Patch(WeaponHandlerAwake, postfix: WH_Awake_postfix);
        }
        static public bool loadedCustomSounds = false;
        public static void WeaponHandlerAwake_Postfix(WeaponHandler __instance)
        {
            WeaponViewIdTransmitter viewIdTransmitter = __instance.gameObject.AddComponent<WeaponViewIdTransmitter>();

            if (!loadedCustomSounds)
            {
                WeaponHandlerEditingHelper.LoadCustomGunSoundsToManager();
                loadedCustomSounds = true;
            }
            var weaponsToAdd = WeaponHandlerEditingHelper.weaponsToAdd;

            if (__instance.photonView.isMine)
            {
                Debug.Log("Allocating viewIds");
                for (int i = 0; i < weaponsToAdd.Count; i++)
                    viewIdTransmitter.AllocateManualPhotonView(weaponsToAdd[i].Item.DisplayName);
            }
            Debug.Log("Loading Custom Weapons");

            var weaponsWrappers = new WeaponHandler.WeaponWrapper[weaponsToAdd.Count];
            Debug.Log("Loading Single Custom Weapons");
            for (int i = 0; i < weaponsWrappers.Length; i++)
            {
                try
                {
                    Weapon weapon = weaponsToAdd[i].WeaponSpawningMethod(__instance.transform, __instance.photonView.ownerId);
                    viewIdTransmitter.SetWeaponIdOnStart(weapon.gameObject.GetPhotonView(), weaponsToAdd[i].Item.DisplayName);
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