using System;
using System.IO;
using TABZMoreGunsMod.WeaponMakerHelper;
using TABZMoreGunsMod.RuntimeResources;
using UnityEngine;

namespace TABZMoreGunsMod
{
    [Serializable]
    public struct CustomWeaponProjectileSettings
    {
        public string Name;
        public string ProjectilyTypeToBeBasedOn;

        public float Damage;
        public float Force;
        public float Fall;

        public float InitialFowardsForce;
        public float InitialUpwardsForce;

        public float BulletLifeTime;
        public float RayLenght;

        public float Spread;

        public static CustomWeaponProjectileSettings FromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<CustomWeaponProjectileSettings>(json);
        }

        public WeaponProjectileSettings ToWeaponProjectileSettings()
        {
            return new WeaponProjectileSettings()
            {
                ProjectilyTypeToBeBasedOn = ProjectilyTypeToBeBasedOn,
                Damage = Damage,
                Force = Force,
                Fall = Fall,
                InitialFowardsForce = InitialFowardsForce,
                InitialUpwardsForce = InitialUpwardsForce,
                BulletLifeTime = BulletLifeTime,
                RayLenght = RayLenght,
                Spread = Spread
            };
        }

        public void ToJsonFile(string path)
        {
            string file = JsonUtility.ToJson(this);
            StreamWriter s = File.CreateText(path + "/Example_WeaponProjectile.json");
            s.Write(file);
            s.Flush();
            s.Close();
        }
    }
}
