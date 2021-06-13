using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TABZMoreGunsMod.RuntimeResources;
using TABZMoreGunsMod.InventoryItemEditingHelper;
using System.IO;

namespace TABZMoreGunsMod
{
    public class MagicCubeTest : MonoBehaviour
    {
        private static bool hasCreatedCube = false;
        private GameObject gun;
        private Rigidbody rig;
        public void Start()
        {
            if (!hasCreatedCube)
            {
                var MagicCubeItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                MagicCubeItem.name = "MagicCubeItem";
                MagicCubeItem.layer = LayerMask.NameToLayer("Item");
                var pVItem = MagicCubeItem.AddComponent<PhotonView>();
                //MagicCube.AddComponent<PhotonTransformView>();
                var item = MagicCubeItem.AddComponent<InventoryItemWeapon>();

                //string path = Directory.GetFiles(Directory.GetCurrentDirectory(), "icon.png", SearchOption.AllDirectories)[0];

                //Texture2D image = FileImporting.ImportImage(path);

                InventoryItemEditing.DisplayNameRef(item) = "Ame";
                InventoryItemEditing.FlavourTextRef(item) = "Hummmm Ahnnn \n Ame gun?!?!?";
                //InventoryItemEditing.ItemIconRef(item) = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
                InventoryItemEditing.ItemTypeRef(item) = InventoryService.ItemType.WEAPON;
                RuntimeResourcesHandler.AddResource(pVItem, "MagicCubeItem");
                WeaponHandler.WeaponWrapper wrapper = new WeaponHandler.WeaponWrapper();
                wrapper.m_item = item;
                wrapper.m_weapon = MakeMagicCube();

               // gun.GetComponent<Renderer>().material.mainTexture = image;
                WeaponHandlerEditingHelper.AddWeaponWraperToList(wrapper);
                Debug.Log("Objects were created!");

                hasCreatedCube = true;
            }
            rig = NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig;
        }

        void Update()
        {
            
            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("Spwning Magic Cube");
                PhotonNetwork.Instantiate("MagicCubeItem", rig.position, Quaternion.identity, 0, new object[] { 200 });
            }

            if (gun.transform.parent != rig.transform)
                gun.transform.parent = rig.transform;
        }
        

        private Weapon MakeMagicCube()
        {
            try
            {
                //Weapon
                var MagicCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                MagicCube.AddComponent<Rigidbody>();
                MagicCube.name = "Ame"; //Needs to be the same name as the DisplayName in the item
                var pV = MagicCube.AddComponent<PhotonView>();
                pV.viewID = PhotonNetwork.AllocateViewID();
                //MagicCube.AddComponent<PhotonTransformView>();
                MagicCube.AddComponent<MeleeWeapon>(); //Dunno if this is helping
                //Firepoint
                GameObject firePoint = new GameObject("FirePoint", typeof(FirePoint));
                firePoint.transform.parent = MagicCube.transform;
                firePoint.transform.localPosition = Vector3.zero;
                //ADS
                GameObject ads = new GameObject("ADS_Position",typeof(ADS));
                ads.transform.parent = MagicCube.transform;
                ads.transform.localPosition = Vector3.zero;
                // WeaponRightHand
                GameObject weaponRightHand = new GameObject("WeaponRightHand", typeof(WeaponRightHandTag));
                weaponRightHand.transform.parent = MagicCube.transform;
                weaponRightHand.transform.localPosition = Vector3.zero;
                ////Sound Event didn't work :/
                //var soundManager = GameObject.FindObjectOfType<SoundEventsManager>(); 
                //var r = HarmonyLib.AccessTools.FieldRefAccess<SoundEventsManager, SoundEventsManager.WeaponSound[]>(soundManager, "m_Weapons");
                //var wr = r[0]; 
                //wr.WeaponName = MagicCube;
                //var l = r.ToList();
                //l.Add(wr);
                //r = l.ToArray();

                MagicCube.transform.parent = GameObject.FindObjectOfType<NetworkConnector>().transform;
                MagicCube.transform.localPosition = Vector3.zero;
                MagicCube.AddComponent<CenterOfMass>();
                var weapon = MagicCube.AddComponent<Weapon>();
                weapon.melee = true;
                HarmonyLib.AccessTools.FieldRefAccess<Weapon, bool>(weapon, "mIsMelee") = true;
                weapon.aimTarget = firePoint.transform;
                gun = weapon.gameObject;
                //RuntimeResourcesHandler.AddResource(pV, "MagicCube"); don't need to add it into the resources thing, or maybe we need
                return weapon;
            }
            catch(Exception ex)
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
