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
            pV.viewID = PhotonNetwork.AllocateViewID(); //Not the correct solution, but setting it to 0 isn't right either

            if (playerTransform.GetComponent<PhotonView>().ownerId != NetworkManager.LocalPlayerPhotonView.ownerId)
                pV.TransferOwnership(playerTransform.GetComponent<PhotonView>().ownerId);

            //NoiseSpawner
            var noiseSpawn = FireWeapon.AddComponent<NoiseSpawner>();
            WeaponEditing.NoiseLoudnessRef(noiseSpawn) = settings.NoiseLoudness;
            WeaponEditing.NoiseIntervalRef(noiseSpawn) = settings.NoiseInterval;
            WeaponEditing.NoiseDistanceRef(noiseSpawn) = settings.NoiseHearableDistance;

            //ADS
            GameObject adsPos = new GameObject("ADS_Position", typeof(ADS));
            adsPos.transform.parent = FireWeapon.transform;
            adsPos.transform.localPosition = settings.ADS_Position;
            adsPos.transform.localEulerAngles = settings.ADS_Rotation;

            //FirePoint
            GameObject firePoint = new GameObject("FirePoint", typeof(FirePoint));
            firePoint.transform.parent = FireWeapon.transform;
            firePoint.transform.localPosition = settings.FirePoint_Position;
            firePoint.transform.localEulerAngles = settings.FirePoint_Rotation;

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
            
            weapon.projectile = RuntimeResources.RuntimeResourcesHandler.InstantiateGameObject(settings.WeaponProjectile);

            if(settings.IsTwoHandedWeapon)
                weapon.aimTarget = playerTransform.Find("CameraHolder/CameraRot/CameraForce/Aims/Rifle");
            else
                weapon.aimTarget = playerTransform.Find("CameraHolder/CameraRot/CameraForce/Aims/Pistol");


            //Weapon colliders
            GameObject colliders = new GameObject("Colliders");
            colliders.transform.parent = FireWeapon.transform;
            for (int i = 0; i < settings.BoxColliders.Length; i++)
            {
                GameObject collider = new GameObject("Collider"+ i, typeof(BoxCollider));
                var bxCol = collider.GetComponent<BoxCollider>();
                collider.transform.parent = colliders.transform;

                bxCol.transform.localPosition = settings.BoxColliders[i].Position;
                bxCol.transform.localEulerAngles = settings.BoxColliders[i].Rotation;
                bxCol.transform.localScale = settings.BoxColliders[i].Scale;
            }
            //Mesh
            GameObject mesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

            mesh.GetComponent<MeshFilter>().mesh = settings.WeaponMesh;
            mesh.GetComponent<MeshRenderer>().material = settings.WeaponMaterial;

            mesh.transform.parent = FireWeapon.transform;
            mesh.transform.localPosition = settings.MeshTransform.Position;
            mesh.transform.localEulerAngles = settings.MeshTransform.Rotation;
            mesh.transform.localScale = settings.MeshTransform.Scale;

            FireWeapon.transform.localPosition = new Vector3(0f, 1.52f, -0.88f); //Pos. of weapons in game
            return weapon;
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

        public string WeaponProjectile;

        public Vector3 ADS_Position;
        public Vector3 ADS_Rotation;
        public Vector3 FirePoint_Position;
        public Vector3 FirePoint_Rotation;

        public Vector3 WeaponRightHand_Position;

        public bool IsTwoHandedWeapon;
        public Vector3 WeaponLeftHand_Position;
        
        public TransformSettings[] BoxColliders;
        public TransformSettings MeshTransform;

        public float NoiseInterval;
        public float NoiseLoudness;
        public float NoiseHearableDistance;
    }
    public struct TransformSettings
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public TransformSettings(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }

    public class ProjectileTypes
    {
        public static readonly string Bullet_Small = "Bullet_Small";
        public static readonly string Bullet_Big = "Bullet_Big";
        public static readonly string Bullet_Bigger = "Bullet_Bigger";

        public static readonly string Bullet_Sniper = "Bullet_Sniper";
        public static readonly string Bullet_SniperBig = "Bullet_SniperBig";
        
        public static readonly string Bullet_DoubleBarrel = "Bullet_DoubleBarrel";
        public static readonly string Bullet_SquareBrawl = "Bullet_SquareBrawl";

        public static readonly string Bullet_Musket = "Bullet_Musket";
        public static readonly string Bullet_Musket2 = "Bullet_Musket2";

        public static readonly string Bullet_Evil = "Bullet_Evil";
        public static readonly string Bullet_Fall = "Bullet_Fall";
        public static readonly string Bullet_Grabber = "Bullet_Grabber";
        public static readonly string Bullet_KnockBack = "Bullet_KnockBack";
    }
}
