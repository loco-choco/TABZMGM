using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TABZMoreGunsMod.RuntimeResources;
using TABZMoreGunsMod.InventoryItemEditingHelper;
using TABZMoreGunsMod.WeaponHandlerEditingHandler;
using System.IO;

namespace TABZMoreGunsMod
{
    public class MagicCubeTest : MonoBehaviour
    {
        static public void CreateWeapon()
        {
            try
            {
                if (needleMesh == null)
                    needleMesh = new CAMOWA.ObjImporter().ImportFile(Directory.GetFiles(Application.dataPath, "needle.obj", SearchOption.AllDirectories)[0]);
            }
            catch
            {
                Debug.Log("Couldn't load the mesh needle.obj, make sure that it is inside the game folder");
            }
            var MagicCubeItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MagicCubeItem.GetComponent<MeshFilter>().mesh = needleMesh;
            MagicCubeItem.GetComponent<MeshRenderer>().material.color = Color.grey;
            MagicCubeItem.transform.localScale = Vector3.one / 2f;
            MagicCubeItem.name = "MagicCubeItem";
            MagicCubeItem.layer = LayerMask.NameToLayer("Item");
            var pVItem = MagicCubeItem.AddComponent<PhotonView>();
            //MagicCube.AddComponent<PhotonTransformView>();
            var item = MagicCubeItem.AddComponent<InventoryItemWeapon>();

            string path = "";
            try
            {
                path = Directory.GetFiles(Application.dataPath, "icon.png", SearchOption.AllDirectories)[0];
            }
            catch
            {
                Debug.Log("Couldn't load the image icon.png, make sure that it is inside the game folder");
            }
            Texture2D image = FileImporting.ImportImage(path);

            InventoryItemEditing.DisplayNameRef(item) = "Needle";
            InventoryItemEditing.FlavourTextRef(item) = "<b>Wishlist\n<color=orange>Straight To The Point</color></b>";
            InventoryItemEditing.ItemIconRef(item) = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
            InventoryItemEditing.ItemTypeRef(item) = InventoryService.ItemType.WEAPON;
            InventoryItemEditing.AmmoTypeRef(item) = InventoryService.AmmoType.Big;
            RuntimeResourcesHandler.AddResource(pVItem, "MagicCubeItem");
            
            WeaponHandlerEditingHelper.AddWeaponToList("Needle", MakeMagicCube);
            Debug.Log("Objects were created!");
        }

        void Update()
        {

            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("Spwning Magic Cube");
                PhotonNetwork.Instantiate("MagicCubeItem", NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig.position, Quaternion.identity, 0, new object[] { 200 });
            }
        }

        private static Mesh needleMesh;
        static public Weapon MakeMagicCube(Transform playerTransform)
        {
            try
            {
                //Weapon
                var MagicCube = new GameObject("Needle"); //Needs to be the same name as the DisplayName in the item
                MagicCube.AddComponent<Rigidbody>().angularDrag = 0.05f;
                MagicCube.transform.parent = playerTransform;
                MagicCube.transform.localPosition = new Vector3(0f, 1.52f, -0.88f);

                var pV = MagicCube.AddComponent<PhotonView>();
                pV.instantiationId = -1;
                pV.currentMasterID = -1;
                pV.synchronization = ViewSynchronization.Off;
                pV.prefixBackup = -1;
                pV.onSerializeRigidBodyOption = OnSerializeRigidBody.All;
                pV.onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
                pV.viewID = 0;
				pV.ownerId = playerTransform.GetComponent<PhotonView>().ownerId;
                MagicCube.AddComponent<MeleeWeapon>();
                //Firepoint
                //GameObject firePoint = new GameObject("FirePoint", typeof(FirePoint));
                //firePoint.transform.parent = MagicCube.transform;
                //firePoint.transform.localPosition = Vector3.zero;
                //ADS
                //GameObject ads = new GameObject("ADS_Position", typeof(ADS));
                //ads.transform.parent = MagicCube.transform;
                //ads.transform.localPosition = Vector3.zero;

                // WeaponRightHand
                GameObject weaponRightHand = new GameObject("WeaponRightHand", typeof(WeaponRightHandTag));
                weaponRightHand.transform.parent = MagicCube.transform;
                weaponRightHand.transform.localPosition = Vector3.zero;


                MagicCube.AddComponent<CenterOfMass>();
                var weapon = MagicCube.AddComponent<Weapon>();
                weapon.punchCurve = AnimationCurve.Linear(0f, 1f, 2f, 0f);
                weapon.punchForce = 2000f;
                weapon.punchTime = 0.2f;
                weapon.melee = true;
                HarmonyLib.AccessTools.FieldRefAccess<Weapon, bool>(weapon, "mIsMelee") = true;
                weapon.aimTarget = playerTransform.Find("Knige").GetComponent<Weapon>().aimTarget;


                //Weapon collider and mesh
                GameObject collider = new GameObject("Collider", typeof(BoxCollider));
                collider.GetComponent<BoxCollider>().size = new Vector3(0.3f, 1f, 6f);
                collider.transform.parent = MagicCube.transform;
                GameObject mesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

                mesh.GetComponent<MeshFilter>().mesh = needleMesh;
                var render = mesh.GetComponent<MeshRenderer>();
                render.material = new Material(Shader.Find("Standard"))
                {
                    color = Color.grey
                };

                mesh.transform.parent = MagicCube.transform;



                mesh.transform.localPosition = Vector3.zero;
                collider.transform.localPosition = Vector3.zero;
                mesh.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                collider.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

                MagicCube.transform.localScale = Vector3.one / 2;
                return weapon;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.Source);
                Debug.Log(ex.StackTrace);
                Debug.Log(ex.InnerException.Message);

                return null;
            }
        }
    }
}
