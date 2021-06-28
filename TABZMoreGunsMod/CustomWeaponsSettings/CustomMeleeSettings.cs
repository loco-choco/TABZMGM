using System;
using System.IO;
using TABZMoreGunsMod.WeaponMakerHelper;
using TABZMoreGunsMod.RuntimeResources;
using TABZMoreGunsMod.WeaponHandlerEditingHandler;
using UnityEngine;

namespace TABZMoreGunsMod
{
    [Serializable]
    public struct CustomMeleeSettings
    {
        public string Name;
        public string WeaponMeshFileName;

        public string ShaderName;
        public Vector4 ColorRBG;
        public string TextureFileName;
        public Vector2 TextureOffset;
        public Vector2 TextureScale;

        public float MeleeWeaponMultiplier;
        public float MeleeMultiplier;


        public Vector3 WeaponRightHand_Position;
        public bool IsTwoHandedWeapon;
        public Vector3 WeaponLeftHand_Position;
        
        public Vector4[] PunchCurve;
        public float PunchForce;
        public float PunchTime;
        public float PunchRate;

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

        public static CustomMeleeSettings FromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<CustomMeleeSettings>(json);
        }

        public Keyframe[] PunchCurveInKeyFrames()
        {
            Keyframe[] keyframes = new Keyframe[PunchCurve.Length];
            for (int i = 0; i < PunchCurve.Length; i++)
                keyframes[i] = new Keyframe(PunchCurve[i].x, PunchCurve[i].y, PunchCurve[i].z, PunchCurve[i].w);
            return keyframes;
        }
        public TransformSettings[] BoxCollidersToTransformSettingsArray()
        {
            TransformSettings[] transformSettings = new TransformSettings[BoxCollidersPosition.Length];
            for (int i = 0; i < transformSettings.Length; i++)
                transformSettings[i] = new TransformSettings(BoxCollidersPosition[i], BoxCollidersRotation[i], BoxCollidersScale[i]);
            return transformSettings;
        }

        public SoundEventsManager.WeaponSoundWrapper ToWeaponSoundWrapper()
        {
            return new SoundEventsManager.WeaponSoundWrapper()
            {
                WeaponEvent = WeaponEvent,
                HitEvent = HitEvent,
                ReloadEvent = NoiseWeaponReloadEvents.GenericReload
            };
        }

        public MeleeWeaponSettings ToMeleeWeaponSettings()
        {
            return new MeleeWeaponSettings()
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
                PunchCurve = new AnimationCurve(PunchCurveInKeyFrames()),
                PunchForce = PunchForce,
                PunchTime = PunchTime,
                PunchRate = PunchRate,
                BoxColliders = BoxCollidersToTransformSettingsArray(),

                MeshTransform = new TransformSettings(MeshPosition, MeshRotation, MeshScale),

                MeleeMultiplier = MeleeMultiplier,
                MeleeWeaponMultiplier = MeleeWeaponMultiplier,
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
            StreamWriter s = File.CreateText(path + "/Example_MeleeWeapon.json");
            s.Write(file);
            s.Flush();
            s.Close();
        }
    }
}
