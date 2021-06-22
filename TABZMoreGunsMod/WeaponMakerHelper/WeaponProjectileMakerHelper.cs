using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod.WeaponMakerHelper
{
    public class WeaponProjectileMakerHelper
    {
        public static GameObject MakeProjectile(WeaponProjectileSettings settings)
        {
            GameObject newProjectile;
            //GameObject effect = null;
            //if (!string.IsNullOrEmpty(settings.HitEffect))
            //{
            //    effect = RuntimeResources.RuntimeResourcesHandler.InstantiateGameObject(settings.HitEffect);
            //    GameObject.DontDestroyOnLoad(effect);
            //    effect.SetActive(false);
            //}
            if (string.IsNullOrEmpty(settings.ProjectilyTypeToBeBasedOn))
            {
                newProjectile = new GameObject();
                newProjectile.AddComponent<Rigidbody>();

                var rayCastP = newProjectile.AddComponent<RaycastProjectile>();
                rayCastP.mask = LayerMask.NameToLayer("Everything") ^ LayerMask.NameToLayer("TerrainBox");
                rayCastP.rayLength = settings.RayLenght;
                WeaponEditing.BulletLifeTimeRef(rayCastP) = settings.BulletLifeTime;

                var pHit = newProjectile.AddComponent<ProjectileHit>();
                pHit.damage = settings.Damage;
                pHit.force = settings.Force;
                pHit.fall = settings.Fall;
                //if (effect != null)
                //    pHit.effect = effect;

                var Force = newProjectile.AddComponent<AddForce>();
                Force.force = settings.InitialFowardsForce;
                Force.upForce = settings.InitialUpwardsForce;

                newProjectile.AddComponent<LookAtVelocity>();

                if (settings.Spread != 0)
                {
                    newProjectile.AddComponent<SetRandomRotation>().spread = settings.Spread;
                }
            }
            else
            {
                newProjectile = RuntimeResources.RuntimeResourcesHandler.InstantiateGameObject(settings.ProjectilyTypeToBeBasedOn);
                var rayCastP = newProjectile.GetComponent<RaycastProjectile>();
                if (rayCastP != null)
                {
                    rayCastP.rayLength = settings.RayLenght;
                    WeaponEditing.BulletLifeTimeRef(rayCastP) = settings.BulletLifeTime;
                }

                var rayCastChildrenComponents = newProjectile.GetComponentsInChildren<RaycastProjectile>();
                for (int i = 0; i < rayCastChildrenComponents.Length; i++)
                {
                    rayCastChildrenComponents[i].rayLength = settings.RayLenght;
                    WeaponEditing.BulletLifeTimeRef(rayCastChildrenComponents[i]) = settings.BulletLifeTime;
                }


                var pHit = newProjectile.GetComponent<ProjectileHit>();
                if (pHit != null)
                {
                    pHit.damage = settings.Damage;
                    pHit.force = settings.Force;
                    pHit.fall = settings.Fall;
                    //if (effect != null)
                    //    pHit.effect = effect;
                }


                var pHitChildrenComponents = newProjectile.GetComponentsInChildren<ProjectileHit>();
                for (int i = 0; i < pHitChildrenComponents.Length; i++)
                {
                    pHitChildrenComponents[i].damage = settings.Damage;
                    pHitChildrenComponents[i].force = settings.Force;
                    pHitChildrenComponents[i].fall = settings.Fall;
                    //if (effect != null)
                    //    pHitChildrenComponents[i].effect = effect;
                }


                var Force = newProjectile.GetComponent<AddForce>();
                if (Force != null)
                {
                    Force.force = settings.InitialFowardsForce;
                    Force.upForce = settings.InitialUpwardsForce;
                }

                var ForceChildrenComponents = newProjectile.GetComponentsInChildren<AddForce>();
                for (int i = 0; i < ForceChildrenComponents.Length; i++)
                {
                    ForceChildrenComponents[i].force = settings.InitialFowardsForce;
                    ForceChildrenComponents[i].upForce = settings.InitialUpwardsForce;
                }

                if (newProjectile.GetComponent<SetRandomRotation>() != null)
                    newProjectile.GetComponent<SetRandomRotation>().spread = settings.Spread;

                var SpreadChildrenComponents = newProjectile.GetComponentsInChildren<SetRandomRotation>();
                for (int i = 0; i < SpreadChildrenComponents.Length; i++)
                    SpreadChildrenComponents[i].spread = settings.Spread;
            }

            newProjectile.SetActive(false);
            return newProjectile;
        }
    }

    public struct WeaponProjectileSettings
    {
        public string ProjectilyTypeToBeBasedOn;

        public float Damage;
        public float Force;
        public float Fall;
        //public string HitEffect;

        public float InitialFowardsForce;
        public float InitialUpwardsForce;

        public float BulletLifeTime;
        public float RayLenght;

        public float Spread;
    }

    public static class ProjectileHitEffects
    {
        public static readonly string BasicHitEffect = "Part_1";
        public static readonly string SniperHitEffect = "Part_2";
        public static readonly string SniperBigHitEffect = "Part_3";

        public static readonly string KnockBackHitEffect = "Part_Knock";
        public static readonly string MusketHitEffect = "Part_Musket";
        public static readonly string EvilHitEffect = "Part_Evil";
        public static readonly string SquareHitEffect = "Part_Square";
    }
}
