using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

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
    }
}
