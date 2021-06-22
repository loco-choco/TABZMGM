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

namespace TABZMoreGunsMod
{
    // TODO
    // 1 - Inserir embaixo do Resources.Load() o nosso código para carregar GO's (NetworkingPeer, PhotonNetwork, ) - Done
    // 1.5 - Achar e corrigir outras coisas que podem fazer o sistema dar errado - Done
    // 1 .75 - Retirar código reduntando ao inserirmnos coisas com as patches - In progress
    // 2 - Descobrir como criar uma arma
    // 3 - Descobrir quais são as coisas mais basicas da criação de arma
    // 4 - Separar as descobertas em variaveis mutaveis
    // 5 - Criar uma struct serializavel e um arquivo que se pode mudar

    public class MoreGunsMod
    {
        private static bool HasThePatchHappened = false;
        private static string gamePath;
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
            if (!HasThePatchHappened)
            {
                var harmonyInstance = new Harmony("com.ivan.MoreGunsMod");
                try
                {
                    ResourceHandlerPatches.Patches(harmonyInstance);
                    WeaponHandlerEditingPatches.Patches(harmonyInstance);
                    HasThePatchHappened = true;
                    AccessTools.StaticFieldRefAccess<string>(typeof(MainMenuHandler), "mVersionString") = "TABZ v1.21-MagicCubeTest";
                    PhotonNetwork.Disconnect();
                    PhotonNetwork.ConnectUsingSettings(MainMenuHandler.VERSION_NUMBER);

                    MagicCubeTest.CreateWeapon();

                    var FireWeaponJsonPaths = Directory.GetFiles(GameExecutabePath, "_FireWeapon.json",SearchOption.AllDirectories);
                    var MeleeWeaponJsonPaths = Directory.GetFiles(GameExecutabePath, "_MeleeWeapon.json", SearchOption.AllDirectories);
                    var WeaponProjectileJsonPaths = Directory.GetFiles(GameExecutabePath, "_WeaponProjectile.json", SearchOption.AllDirectories);

                    new CustomWeaponProjectileSettings().ToJsonFile(GameExecutabePath);
                    new CustomFireWeaponSettings().ToJsonFile(GameExecutabePath);
                    new CustomMeleeSettings().ToJsonFile(GameExecutabePath);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }

            if (Application.loadedLevel == 1)
            {
                WeaponHandlerEditingPatches.loadedCustomSounds = false;

                if (NetworkManager.MasterPhotonView.gameObject.GetComponent<MagicCubeTest>() == null)
                    NetworkManager.MasterPhotonView.gameObject.AddComponent<MagicCubeTest>();
            }
        }
    }
}
