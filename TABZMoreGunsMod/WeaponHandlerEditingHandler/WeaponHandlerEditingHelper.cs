using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace TABZMoreGunsMod.WeaponHandlerEditingHandler
{
    //TODO
    // - Add this list to the SoundEventsManager
    public class WeaponHandlerEditingHelper
    {
        static public List<WeaponWeapperResourcesNames> weaponsToAdd = new List<WeaponWeapperResourcesNames>();
        static public void AddWeaponToList(string WeaponName, SoundEventsManager.WeaponSoundWrapper WeaponSounds, WeaponSpawner WeaponSpawningMethod)
        {
            var itemExampleHolder = new GameObject(WeaponName).AddComponent<InventoryItemWeapon>();
            itemExampleHolder.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(itemExampleHolder);
            InventoryItemEditingHelper.InventoryItemEditing.DisplayNameRef(itemExampleHolder) = WeaponName;
            weaponsToAdd.Add(new WeaponWeapperResourcesNames
            {
                Item = itemExampleHolder,
                WeaponSpawningMethod = WeaponSpawningMethod,
                WeaponSounds = WeaponSounds,
            });
        }

        public static void LoadCustomGunSoundsToManager()
        {
            Debug.Log("Loading Custom Weapons Sounds");
            SoundEventsManager soundEventsManager = UnityEngine.Object.FindObjectOfType<SoundEventsManager>();

            var weaponsToAdd = WeaponHandlerEditingHelper.weaponsToAdd;
            var weaponsSoundsWrappers = new SoundEventsManager.WeaponSound[weaponsToAdd.Count];
            for (int i = 0; i < weaponsSoundsWrappers.Length; i++)
            {
                try
                {
                    weaponsSoundsWrappers[i] = new SoundEventsManager.WeaponSound
                    {
                        WeaponName = weaponsToAdd[i].Item.gameObject,
                        WeaponEvent = weaponsToAdd[i].WeaponSounds.WeaponEvent,
                        HitEvent = weaponsToAdd[i].WeaponSounds.HitEvent,
                        ReloadEvent = weaponsToAdd[i].WeaponSounds.ReloadEvent
                    };
                }
                catch
                {
                    Debug.Log(string.Format("Couldn't add the weapon's sounds from the item {0}", weaponsToAdd[i].Item.DisplayName));
                }
            }
            var managerSoundsList = AccessTools.FieldRefAccess<SoundEventsManager, SoundEventsManager.WeaponSound[]>(soundEventsManager, "m_Weapons");
            AccessTools.FieldRefAccess<SoundEventsManager, SoundEventsManager.WeaponSound[]>(soundEventsManager, "m_Weapons") = managerSoundsList.AddRangeToArray(weaponsSoundsWrappers);

            Debug.Log("Customs weapons sounds loaded");
        }

        public struct WeaponWeapperResourcesNames
        {
            public InventoryItemWeapon Item;
            public WeaponSpawner WeaponSpawningMethod;
            public SoundEventsManager.WeaponSoundWrapper WeaponSounds;
        }

        public delegate Weapon WeaponSpawner(Transform playerTransform, int ownerID);
    }


    public static class NoiseWeaponEvents
    {
        public static readonly string PistolMakarov = "event:/GUNS/PistolMakarov";
        public static readonly string PistolMagnum = "event:/GUNS/PistolMagnum";
        public static readonly string RifleRepeater = "event:/GUNS/RifleRepeater";
        public static readonly string RifleAK74 = "event:/GUNS/RifleAK74";
        public static readonly string SMGCZ61 = "event:/GUNS/SMGCZ61";
        public static readonly string SMGMP5 = "event:/GUNS/SMGMP5";
        public static readonly string ShotgunSawedOff = "event:/GUNS/ShotgunSawedOff";
        public static readonly string ShotgunMp133 = "event:/GUNS/ShotgunMp133";
        public static readonly string HuntingSniper = "event:/GUNS/HuntingSniper";
        public static readonly string BigSniper = "event:/GUNS/BigSniper";
        public static readonly string RifleMusket = "event:/GUNS/RifleMusket";
        public static readonly string SquareBrawlShotgun = "event:/GUNS/SquareBrawlShotgun";
        public static readonly string EvilAk = "event:/GUNS/EvilAk";
        public static readonly string HomerunGun = "event:/GUNS/homerunGun";
        public static readonly string Annoyer = "event:/GUNS/Annoyer";

        public static string KnifeSwing = "event:/MELEE/KnifeSwing";
    }
    public static class NoiseWeaponHitEvents
    {
        public static readonly string SmallHit = "event:/GUNS/hitsnstuff/SmallHit";
        public static readonly string MediumHit = "event:/GUNS/hitsnstuff/MediumHit";
        public static readonly string SMallHit = "event:/GUNS/hitsnstuff/SMallHit";
        public static readonly string BigHit = "event:/GUNS/hitsnstuff/BigHit";

        public static readonly string AxeHit = "event:/MELEE/AxeHit";
        public static readonly string KnifeHit = "event:/MELEE/KnifeHit";
    }
    public static class NoiseWeaponReloadEvents
    {
        public static readonly string GenericReload = "event:/GUNS/GenericReload";
    }
}