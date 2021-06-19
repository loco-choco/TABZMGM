using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod.WeaponMakerHelper
{
    public class MeleeMakerHelper
    {
        public static Weapon MakeMeleeWeapon(Transform playerTransform, MeleeWeaponSettings settings)
        {
            var MeleeWeapon = new GameObject(settings.Name); //Needs to be the same name as the DisplayName in the item
            MeleeWeapon.AddComponent<Rigidbody>().angularDrag = 0.05f;
            MeleeWeapon.transform.parent = playerTransform;

            //PhotonView Settings
            var pV = MeleeWeapon.AddComponent<PhotonView>();
            pV.instantiationId = -1;
            pV.currentMasterID = -1;
            pV.synchronization = ViewSynchronization.Off;
            pV.prefixBackup = -1;
            pV.onSerializeRigidBodyOption = OnSerializeRigidBody.All;
            pV.onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
            pV.viewID = 0;

            pV.ownerId = playerTransform.GetComponent<PhotonView>().ownerId;

            //NoiseSpawner
            var noiseSpawn = MeleeWeapon.AddComponent<NoiseSpawner>();
            WeaponEditing.NoiseLoudnessRef(noiseSpawn) = settings.NoiseLoudness;
            WeaponEditing.NoiseIntervalRef(noiseSpawn) = settings.NoiseInterval;
            WeaponEditing.NoiseDistanceRef(noiseSpawn) = settings.NoiseHearableDistance;

            var melee = MeleeWeapon.AddComponent<MeleeWeapon>();
            melee.weaponMultiplier = settings.MeleeWeaponMultiplier;
            melee.multiplier = settings.MeleeMultiplier;

            // WeaponRightHand
            GameObject weaponRightHand = new GameObject("WeaponRightHand", typeof(WeaponRightHandTag));
            weaponRightHand.transform.parent = MeleeWeapon.transform;
            weaponRightHand.transform.localPosition = settings.WeaponRightHand_Position;

            if (settings.IsTwoHandedWeapon)
            {
                GameObject weaponLeftHand = new GameObject("WeaponLeftHand", typeof(WeaponLeftHandTag));
                weaponRightHand.transform.parent = MeleeWeapon.transform;
                weaponRightHand.transform.localPosition = settings.WeaponLeftHand_Position;
            }


            MeleeWeapon.AddComponent<CenterOfMass>();
            var weapon = MeleeWeapon.AddComponent<Weapon>();
            weapon.punchCurve = settings.PunchCurve;
            weapon.punchForce = settings.PunchForce;
            weapon.punchTime = settings.PunchTime;
            weapon.punchRate = settings.PunchRate;
            weapon.melee = true;
            HarmonyLib.AccessTools.FieldRefAccess<Weapon, bool>(weapon, "mIsMelee") = true;
            weapon.aimTarget = playerTransform.Find("CameraHolder/CameraRot/CameraForce/Aims/Melee"); //Change to .Find("[...]....\....\Melee")


            //Weapon collider and mesh
            GameObject collider = new GameObject("Collider", typeof(BoxCollider));
            collider.GetComponent<BoxCollider>().size = settings.BoxColliderSize;
            collider.transform.parent = MeleeWeapon.transform;
            GameObject mesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

            mesh.GetComponent<MeshFilter>().mesh = settings.WeaponMesh;
            mesh.GetComponent<MeshRenderer>().material = settings.WeaponMaterial;

            mesh.transform.parent = MeleeWeapon.transform;

            mesh.transform.localRotation = settings.ColliderAndMeshAngle;
            collider.transform.localRotation = settings.ColliderAndMeshAngle;
            
            MeleeWeapon.transform.localPosition = new Vector3(0f, 1.52f, -0.88f); //Pos. of weapons in game

            return weapon;
        }       
    }

    public struct MeleeWeaponSettings
    {
        public string Name;
        public Mesh WeaponMesh;
        public Material WeaponMaterial;

        public float MeleeWeaponMultiplier;
        public float MeleeMultiplier;


        public Vector3 WeaponRightHand_Position;
        public bool IsTwoHandedWeapon;
        public Vector3 WeaponLeftHand_Position;

        public AnimationCurve PunchCurve;
        public float PunchForce;
        public float PunchTime;
        public float PunchRate;

        public Vector3 BoxColliderSize;
        public Quaternion ColliderAndMeshAngle;

        public float NoiseInterval;
        public float NoiseLoudness;
        public float NoiseHearableDistance;
    }
}
