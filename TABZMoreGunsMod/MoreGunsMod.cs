using System.IO;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine;
using CAMOWA;
using HarmonyLib;
using TABZMoreGunsMod.RuntimeResources;
using TABZMoreGunsMod.WeaponHandlerEditingHandler;
using TABZMoreGunsMod.CustomWeaponsSettings;
using TABZMoreGunsMod.WeaponMakerHelper;

namespace TABZMoreGunsMod
{
    public class MoreGunsMod
    {
        private static bool HasThePatchesHappened = false;
        private static string gamePath;
        private static FireWeaponGenerator[] fireWeapons;
        private static MeleeWeaponGenerator[] meleeWeapons;
        public static string GameExecutabePath
        {
            get
            {
                if (string.IsNullOrEmpty(gamePath))
                    gamePath = Application.dataPath.Remove(Application.dataPath.LastIndexOf('/'));
                return gamePath;
            }

            private set { }
        }
        [IMOWAModInnit("More Guns Mod", -1, 1)]
        static public void ModInnit(string startingPoint)
        {
            if (!HasThePatchesHappened)
            {
                var harmonyInstance = new Harmony("com.ivan.MoreGunsMod");
                try
                {
                    ResourceHandlerPatches.Patches(harmonyInstance);
                    WeaponHandlerEditingPatches.Patches(harmonyInstance);
                    TABZChatPatches.Patches(harmonyInstance);
                    HasThePatchesHappened = true;

                    AccessTools.StaticFieldRefAccess<string>(typeof(MainMenuHandler), "mVersionString") = "TABZ v1.21-MagicCubeTest";
                    PhotonNetwork.Disconnect();
                    PhotonNetwork.ConnectUsingSettings(MainMenuHandler.VERSION_NUMBER);

                    GenerateAllRuntimeWeaponsAndStuff();
                }
                catch (Exception ex)
                {
                    Debug.Log(string.Format("Exception in {0} : {1}", ex.Source, ex.Message));
                }
            }
            if (Application.loadedLevel == 1)
                WeaponHandlerEditingPatches.loadedCustomSounds = false;
        }

        private static void GenerateAllRuntimeWeaponsAndStuff()
        {
            //Items
            var ItemJsonPaths = Directory.GetFiles(GameExecutabePath, "*_ItemWeapon.json", SearchOption.AllDirectories);
            for (int i = 0; i < ItemJsonPaths.Length; i++)
            {
                try
                {
                    var itemSettings = CustomItemWeaponSettings.FromJson(ItemJsonPaths[i]);
                    var item = ItemWeaponMakerHelper.MakeItemWeapon(itemSettings.ToItemWeaponSettings());
                    item.transform.localScale = itemSettings.MeshScale;
                    RuntimeResourcesHandler.AddResource(item.GetComponent<PhotonView>(), itemSettings.DisplayName);
                }
                catch
                {
                    Debug.Log(string.Format("The Item from the file {0} couldn't be read", ItemJsonPaths[i]));
                }
            }
            //Projectiles
            var WeaponProjectileJsonPaths = Directory.GetFiles(GameExecutabePath, "*_WeaponProjectile.json", SearchOption.AllDirectories);
            for (int i = 0; i < WeaponProjectileJsonPaths.Length; i++)
            {
                try
                {
                    var projectileSettings = CustomWeaponProjectileSettings.FromJson(WeaponProjectileJsonPaths[i]);
                    var projectile = WeaponProjectileMakerHelper.MakeProjectile(projectileSettings.ToWeaponProjectileSettings());
                    projectile.name = projectileSettings.Name;
                    RuntimeResourcesHandler.AddNonNetworkedResource(projectile, projectile.name);
                }
                catch
                {
                    Debug.Log(string.Format("The Projectile from the file {0} couldn't be read", WeaponProjectileJsonPaths[i]));
                }
            }
            //Fire Weapons
            var FireWeaponJsonPaths = Directory.GetFiles(GameExecutabePath, "*_FireWeapon.json", SearchOption.AllDirectories);
            fireWeapons = new FireWeaponGenerator[FireWeaponJsonPaths.Length];
            for (int i = 0; i < WeaponProjectileJsonPaths.Length; i++)
            {
                try
                {
                    fireWeapons[i] = new FireWeaponGenerator(CustomFireWeaponSettings.FromJson(FireWeaponJsonPaths[i]));
                }
                catch
                {
                    Debug.Log(string.Format("The Fire Weapon from the file {0} couldn't be read", FireWeaponJsonPaths[i]));
                }
            }
            fireWeapons = fireWeapons.OrderBy(x => x.Name).ToArray();

            //Melee Weapons
            var MeleeWeaponJsonPaths = Directory.GetFiles(GameExecutabePath, "*_MeleeWeapon.json", SearchOption.AllDirectories);
            meleeWeapons = new MeleeWeaponGenerator[MeleeWeaponJsonPaths.Length];
            for (int i = 0; i < MeleeWeaponJsonPaths.Length; i++)
            {
                try
                {
                    meleeWeapons[i] = new MeleeWeaponGenerator(CustomMeleeSettings.FromJson(MeleeWeaponJsonPaths[i]));
                }
                catch
                {
                    Debug.Log(string.Format("The Melee Weapon from the file {0} couldn't be read", MeleeWeaponJsonPaths[i]));
                }
            }
            meleeWeapons = meleeWeapons.OrderBy(x => x.Name).ToArray();
        }
    }
}
