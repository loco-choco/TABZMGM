using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TABZMoreGunsMod.WeaponMakerHelper;
using TABZMoreGunsMod.RuntimeResources;

namespace TABZMoreGunsMod.CustomWeaponsSettings
{
        [Serializable]
        public struct CustomItemWeaponSettings
        {
        public string DisplayName;
        public string FavourText;

        public string IconFileName;
        public InventoryService.AmmoType AmmoType;

        public string ItemMeshFileName;
        public Vector3 MeshScale;

        public string ShaderName;
        public Vector4 ColorRBG;
        public Vector2 TextureOffset;
        public Vector2 TextureScale;

        public int BulletsInMagazine;

        public static CustomItemWeaponSettings FromJson(string path)
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<CustomItemWeaponSettings>(json);
            }

            public ItemWeaponSettings ToItemWeaponSettings()
            {
                return new ItemWeaponSettings()
                {
                    AmmoType = AmmoType,
                    BulletsInMagazine = BulletsInMagazine,
                    DisplayName = DisplayName,
                    FavourText = FavourText,
                    ItemMaterial = new Material(Shader.Find(ShaderName))
                    {
                        color = new Color(ColorRBG.x, ColorRBG.y, ColorRBG.z, ColorRBG.w),
                        mainTextureOffset = TextureOffset,
                        mainTextureScale = TextureScale
                    },
                    ItemIcon = RuntimeResourcesHandler.GetTexture2DResource(IconFileName),
                    ItemMesh = RuntimeResourcesHandler.GetMeshResource(ItemMeshFileName)
                };
            }

            public void ToJsonFile(string path)
            {
                string file = JsonUtility.ToJson(this);
                StreamWriter s = File.CreateText(path + "/Example_WeaponIcon.json");
                s.Write(file);
                s.Flush();
                s.Close();
            }
    }
}
