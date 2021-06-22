using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace TABZMoreGunsMod.WeaponMakerHelper
{
    public class WeaponEditing
    {
        static public ref int MagazineSizeRef(Weapon weapon)
        {
            return ref AccessTools.FieldRefAccess<Weapon, int>(weapon, "m_MagazineSize");
        }
        static public ref float ReloadTimeRef(Weapon weapon)
        {
            return ref AccessTools.FieldRefAccess<Weapon, float>(weapon, "mReloadTime");
        }
        static public ref float NoiseIntervalRef(NoiseSpawner noiseSpawner)
        {
            return ref AccessTools.FieldRefAccess<NoiseSpawner, float>(noiseSpawner, "m_interval");
        }
        static public ref float NoiseLoudnessRef(NoiseSpawner noiseSpawner)
        {
            return ref AccessTools.FieldRefAccess<NoiseSpawner, float>(noiseSpawner, "m_noiseLoudness");
        }
        static public ref float NoiseDistanceRef(NoiseSpawner noiseSpawner)
        {
            return ref AccessTools.FieldRefAccess<NoiseSpawner, float>(noiseSpawner, "m_distance");
        }

        static public ref float BulletLifeTimeRef(RaycastProjectile raycastProjectile)
        {
            return ref AccessTools.FieldRefAccess<RaycastProjectile, float>(raycastProjectile, "m_BulletLifeTime");
        }

        public static T[] GetAllNestedComponents<T>(Transform transform)
        {
            List<T> components = new List<T>();
            foreach (Transform child in transform)
            {
                if (child.GetComponent<T>() != null)
                    components.Add(child.GetComponent<T>());

                components.AddRange(GetAllNestedComponents<T>(child));
            }
            return components.ToArray();
        }
    }
}
