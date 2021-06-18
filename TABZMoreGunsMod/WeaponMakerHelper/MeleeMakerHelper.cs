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


            MeleeWeapon.AddComponent<MeleeWeapon>();

            // WeaponRightHand
            GameObject weaponRightHand = new GameObject("WeaponRightHand", typeof(WeaponRightHandTag));
            weaponRightHand.transform.parent = MeleeWeapon.transform;
            weaponRightHand.transform.localPosition = Vector3.zero;


            MeleeWeapon.AddComponent<CenterOfMass>();
            var weapon = MeleeWeapon.AddComponent<Weapon>();
            weapon.punchCurve = settings.PunchCurve;
            weapon.punchForce = settings.PunchForce;
            weapon.punchTime = settings.PunchTime;
            weapon.melee = true;
            HarmonyLib.AccessTools.FieldRefAccess<Weapon, bool>(weapon, "mIsMelee") = true;
            weapon.aimTarget = playerTransform.Find("Knige").GetComponent<Weapon>().aimTarget; //Change to .Find("[...]....\....\Melee")


            //Weapon collider and mesh
            GameObject collider = new GameObject("Collider", typeof(BoxCollider));
            collider.GetComponent<BoxCollider>().size = new Vector3(0.3f, 1f, 6f);
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

        public struct MeleeWeaponSettings
        {
            public string Name;
            public Mesh WeaponMesh;
            public Material WeaponMaterial;

            public AnimationCurve PunchCurve;
            public float PunchForce;
            public float PunchTime;

            public Vector3 BoxColliderSize;
            public Quaternion ColliderAndMeshAngle;
        }
    }
}
