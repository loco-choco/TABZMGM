using System;
using System.IO;
using TABZMoreGunsMod.WeaponMakerHelper;
using TABZMoreGunsMod.RuntimeResources;
using UnityEngine;

namespace TABZMoreGunsMod
{
    [Serializable]
    public struct CustomFireWeaponSettings
    {
        public string Name;
        public string WeaponMeshFileName;

        public string ShaderName;
        public Vector4 ColorRBG;
        public string TextureFileName;
        public Vector2 TextureOffset;
        public Vector2 TextureScale;

        public float FOV;
        public float FireRate;
        public bool Automatic;
        public int MagazineSize;
        public float ReloadTime;
        public Vector3 Recoil;
        public Vector3 AngularRecoil;

        public Vector3 ForceShake;
        public float ForceShakeTime;

        public string WeaponProjectile;

        public Vector3 ADS_Position;
        public Vector3 ADS_Rotation;
        public Vector3 FirePoint_Position;
        public Vector3 FirePoint_Rotation;

        public Vector3 WeaponRightHand_Position;

        public bool IsTwoHandedWeapon;
        public Vector3 WeaponLeftHand_Position;

        public Vector3[] BoxCollidersPosition;
        public Vector3[] BoxCollidersRotation;
        public Vector3[] BoxCollidersScale;

        public Vector3 MeshPosition;
        public Vector3 MeshRotation;
        public Vector3 MeshScale;

        public float NoiseInterval;
        public float NoiseLoudness;
        public float NoiseHearableDistance;

        public string WeaponEvent;
        public string HitEvent;
        public string ReloadEvent;

        public static CustomFireWeaponSettings FromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<CustomFireWeaponSettings>(json);
        }
        public TransformSettings[] BoxCollidersToTransformSettingsArray()
        {
            TransformSettings[] transformSettings = new TransformSettings[BoxCollidersPosition.Length];
            for (int i = 0; i < transformSettings.Length; i++)
                transformSettings[i] = new TransformSettings(BoxCollidersPosition[i], BoxCollidersRotation[i], BoxCollidersScale[i]);
            return transformSettings;
        }

        public FireWeaponSettings ToFireWeaponSettings()
        {
            return new FireWeaponSettings()
            {
                Name = Name,
                WeaponMesh = RuntimeResourcesHandler.GetMeshResource(WeaponMeshFileName),
                WeaponMaterial = new Material(Shader.Find(ShaderName))
                {
                    color = new Color(ColorRBG.x, ColorRBG.y, ColorRBG.z, ColorRBG.w),
                    mainTexture = RuntimeResourcesHandler.GetTexture2DResource(TextureFileName),
                    mainTextureOffset = TextureOffset,
                    mainTextureScale = TextureScale
                },
                FOV = FOV,
                FireRate = FireRate,
                Automatic = Automatic,
                MagazineSize = MagazineSize,
                ReloadTime = ReloadTime,
                Recoil = Recoil,
                AngularRecoil = AngularRecoil,

                ForceShake = ForceShake,
                ForceShakeTime = ForceShakeTime,

                WeaponProjectile = WeaponProjectile,

                ADS_Position = ADS_Position,
                ADS_Rotation = ADS_Rotation,
                FirePoint_Position = FirePoint_Position,
                FirePoint_Rotation = FirePoint_Rotation,

                BoxColliders = BoxCollidersToTransformSettingsArray(),

                MeshTransform = new TransformSettings(MeshPosition, MeshRotation, MeshScale),

                WeaponLeftHand_Position = WeaponLeftHand_Position,
                WeaponRightHand_Position = WeaponRightHand_Position,
                IsTwoHandedWeapon = IsTwoHandedWeapon,
                NoiseInterval = NoiseInterval,
                NoiseHearableDistance = NoiseHearableDistance,
                NoiseLoudness = NoiseLoudness,
            };
        }

        public void ToJsonFile(string path)
        {
            string file = JsonUtility.ToJson(this);
            StreamWriter s = File.CreateText(path + "/Example_FireWeapon.json");
            s.Write(file);
            s.Flush();
            s.Close();
        }
    }
}
