using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod.WeaponMakerHelper
{
    public class FireWeaponMakerHelper
    {

        public static Weapon MakeFireWeapon(Transform playerTransform, FireWeaponSettings settings)
        {
            var FireWeapon = new GameObject(settings.Name); //Needs to be the same name as the DisplayName in the item
            FireWeapon.AddComponent<Rigidbody>().angularDrag = 0.05f;
            FireWeapon.transform.parent = playerTransform;

            //PhotonView Settings
            var pV = FireWeapon.AddComponent<PhotonView>();
            pV.instantiationId = -1;
            pV.currentMasterID = -1;
            pV.synchronization = ViewSynchronization.Off;
            pV.prefixBackup = -1;
            pV.onSerializeRigidBodyOption = OnSerializeRigidBody.All;
            pV.onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
            pV.viewID = 0;

            pV.ownerId = playerTransform.GetComponent<PhotonView>().ownerId;

            //NoiseSpawner
            var noiseSpawn = FireWeapon.AddComponent<NoiseSpawner>();
            WeaponEditing.NoiseLoudnessRef(noiseSpawn) = settings.NoiseLoudness;
            WeaponEditing.NoiseIntervalRef(noiseSpawn) = settings.NoiseInterval;
            WeaponEditing.NoiseDistanceRef(noiseSpawn) = settings.NoiseHearableDistance;

            //ADS
            GameObject adsPos = new GameObject("ADS_Position", typeof(ADS));
            adsPos.transform.parent = FireWeapon.transform;
            adsPos.transform.localPosition = settings.ADS_Position;

            //FirePoint
            GameObject firePoint = new GameObject("FirePoint", typeof(FirePoint));
            firePoint.transform.parent = FireWeapon.transform;
            firePoint.transform.localPosition = settings.FirePoint_Position;

            // WeaponRightHand
            GameObject weaponRightHand = new GameObject("WeaponRightHand", typeof(WeaponRightHandTag));
            weaponRightHand.transform.parent = FireWeapon.transform;
            weaponRightHand.transform.localPosition = settings.WeaponLeftHand_Position;

            if (settings.IsTwoHandedWeapon)
            {
                GameObject weaponLeftHand = new GameObject("WeaponLeftHand", typeof(WeaponLeftHandTag));
                weaponRightHand.transform.parent = FireWeapon.transform;
                weaponRightHand.transform.localPosition = settings.WeaponLeftHand_Position;
            }
            
            var weapon = FireWeapon.AddComponent<Weapon>();
            weapon.fov = settings.FOV;

            weapon.fireRate = settings.FireRate;
            weapon.automatic = settings.Automatic;
            WeaponEditing.ReloadTimeRef(weapon) = settings.ReloadTime;
            WeaponEditing.MagazineSizeRef(weapon) = settings.MagazineSize;

            weapon.recoil = settings.Recoil;
            weapon.angularRecoil = settings.AngularRecoil;
            weapon.forceShake = settings.ForceShake;
            weapon.forceShakeTime = settings.ForceShakeTime;

            string projectilePrefabName = GetPrefabNameFromType(settings.WeaponProjectileType);
            if (projectilePrefabName == "")
                projectilePrefabName = settings.CustomProjectileResourceName;
            weapon.projectile = RuntimeResources.RuntimeResourcesHandler.InstantiateGameObject(projectilePrefabName);

            if(settings.IsTwoHandedWeapon)
                weapon.aimTarget = playerTransform.Find("CameraHolder/CameraRot/CameraForce/Aims/Rifle");
            else
                weapon.aimTarget = playerTransform.Find("CameraHolder/CameraRot/CameraForce/Aims/Pistol");


            //Weapon collider and mesh
            GameObject collider = new GameObject("Collider", typeof(BoxCollider));
            collider.GetComponent<BoxCollider>().size = settings.BoxColliderSize;
            collider.transform.parent = FireWeapon.transform;
            GameObject mesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

            mesh.GetComponent<MeshFilter>().mesh = settings.WeaponMesh;
            mesh.GetComponent<MeshRenderer>().material = settings.WeaponMaterial;

            mesh.transform.parent = FireWeapon.transform;

            mesh.transform.localRotation = settings.ColliderAndMeshAngle;
            collider.transform.localRotation = settings.ColliderAndMeshAngle;

            FireWeapon.transform.localPosition = new Vector3(0f, 1.52f, -0.88f); //Pos. of weapons in game
            return weapon;
        }

        public static string GetPrefabNameFromType(ProjectileType type)
        {
            string name = "";
            switch (type)
            {
                case ProjectileType.Bullet_Big:
                    name = "Bullet_Big";
                    break;
                case ProjectileType.Bullet_Bigger:
                    name = "Bullet_Bigger";
                    break;
                case ProjectileType.Bullet_DoubleBarrel:
                    name = "Bullet_DoubleBarrel";
                    break;
                case ProjectileType.Bullet_Evil:
                    name = "Bullet_Evil";
                    break;
                case ProjectileType.Bullet_Fall:
                    name = "Bullet_Fall";
                    break;
                case ProjectileType.Bullet_Grabber:
                    name = "Bullet_Grabber";
                    break;
                case ProjectileType.Bullet_KnockBack:
                    name = "Bullet_KnockBack";
                    break;
                case ProjectileType.Bullet_Musket:
                    name = "Bullet_Musket";
                    break;
                case ProjectileType.Bullet_Musket2:
                    name = "Bullet_Musket2";
                    break;
                case ProjectileType.Bullet_Small:
                    name = "Bullet_Small";
                    break;
                case ProjectileType.Bullet_Sniper:
                    name = "Bullet_Sniper";
                    break;
                case ProjectileType.Bullet_SniperBig:
                    name = "Bullet_SniperBig";
                    break;
                case ProjectileType.Bullet_SquareBrawl:
                    name = "Bullet_SquareBrawl";
                    break;
                default:
                    break;
            }
            return name;
        }
    }

    public struct FireWeaponSettings
    {
        public string Name;
        public Mesh WeaponMesh;
        public Material WeaponMaterial;

        public float FOV;
        public float FireRate;
        public bool Automatic;
        public int MagazineSize;
        public float ReloadTime;
        public Vector3 Recoil;
        public Vector3 AngularRecoil;

        public Vector3 ForceShake;
        public float ForceShakeTime;

        public ProjectileType WeaponProjectileType;
        public string CustomProjectileResourceName;

        public Vector3 ADS_Position;
        public Vector3 WeaponRightHand_Position;
        public Vector3 FirePoint_Position;

        public bool IsTwoHandedWeapon;
        public Vector3 WeaponLeftHand_Position;

        public Vector3 BoxColliderSize;
        public Quaternion ColliderAndMeshAngle;

        public float NoiseInterval;
        public float NoiseLoudness;
        public float NoiseHearableDistance;
    }
    public enum ProjectileType : int
    {
        Bullet_Big,
        Bullet_Bigger,
        Bullet_DoubleBarrel,
        Bullet_Evil,
        Bullet_Fall,
        Bullet_Grabber,
        Bullet_KnockBack,
        Bullet_Musket,
        Bullet_Musket2,
        Bullet_Small,
        Bullet_Sniper,
        Bullet_SniperBig,
        Bullet_SquareBrawl,

        CustomBullet
    }
}
