using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TABZMoreGunsMod.RuntimeResources;
using TABZMoreGunsMod.InventoryItemEditingHelper;
using TABZMoreGunsMod.WeaponHandlerEditingHandler;
using TABZMoreGunsMod.WeaponMakerHelper;
using System.IO;

namespace TABZMoreGunsMod
{
    public class MagicCubeTest : MonoBehaviour
    {
        static private MeleeWeaponSettings WeaponMeleeSettings;
        static private FireWeaponSettings WeaponFireSettings;
        private static Mesh needleMesh;

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
            string path = "";
            try { path = Directory.GetFiles(Application.dataPath, "icon.png", SearchOption.AllDirectories)[0]; }
            catch
            {
                Debug.Log("Couldn't load the image icon.png, make sure that it is inside the game folder");
            }
            Texture2D image = FileImporting.ImportImage(path);

            ItemWeaponSettings itemSettings = new ItemWeaponSettings()
            {
                DisplayName = "Needle",
                FavourText = "<b>Wishlist\n<color=orange>Straight To The Point</color></b>",
                ItemMaterial = new Material(Shader.Find("Standard"))
                {
                    color = Color.grey
                },
                ItemMesh = needleMesh,
                ItemIcon = image,
                AmmoType = InventoryService.AmmoType.Small
            };
            var MagicCubeItem = ItemWeaponMakerHelper.MakeItemWeapon(itemSettings);
            MagicCubeItem.transform.localScale = Vector3.one / 2f;

            //WeaponMeleeSettings = new MeleeWeaponSettings()
            //{
            //    Name = "Needle",
            //    WeaponMesh = needleMesh,
            //    WeaponMaterial = new Material(Shader.Find("Standard"))
            //    {
            //        color = Color.grey
            //    },
            //    PunchCurve = AnimationCurve.Linear(0f, 1f, 2f, 0f),
            //    PunchForce = 2000f,
            //    PunchTime = 0.1f,
            //    PunchRate = 1f,
            //    BoxColliderSize = new Vector3(0.3f, 1f, 6f),
            //    ColliderAndMeshAngle = Quaternion.Euler(60f, 0f, 0f),

            //    MeleeMultiplier = 1f,
            //    MeleeWeaponMultiplier = 1f,
            //    WeaponRightHand_Position = Vector3.zero,
            //    IsTwoHandedWeapon = false,
            //    NoiseInterval = 0f,
            //    NoiseHearableDistance = 50f,
            //    NoiseLoudness = 0.3f,
            //};

            WeaponFireSettings =  new FireWeaponSettings()
            {
                Name = "Needle",
                WeaponMesh = needleMesh,
                WeaponMaterial = new Material(Shader.Find("Standard"))
                {
                    color = Color.grey
                },
                Automatic = false,
                ADS_Position = -Vector3.forward / 2f,
                WeaponProjectileType = ProjectileType.Bullet_Big,
                MagazineSize = 2,
                FirePoint_Position = Vector3.forward * 3f,
                FOV = 60,
                FireRate = 1f,
                ReloadTime = 0.01f,

                BoxColliderSize = Vector3.one / 10f,
                ColliderAndMeshAngle = Quaternion.Euler(60f, 0f, 0f),

                WeaponRightHand_Position = Vector3.zero,
                IsTwoHandedWeapon = false,

                NoiseInterval = 0f,
                NoiseHearableDistance = 50f,
                NoiseLoudness = 0.3f,
            };

            RuntimeResourcesHandler.AddResource(MagicCubeItem.GetComponent<PhotonView>(), "MagicCubeItem");

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
        static public Weapon MakeMagicCube(Transform playerTransform)
        {
            try
            {
                //Weapon weapon = MeleeMakerHelper.MakeMeleeWeapon(playerTransform, WeaponMeleeSettings);
                Weapon weapon = FireWeaponMakerHelper.MakeFireWeapon(playerTransform, WeaponFireSettings);
                weapon.transform.localScale = Vector3.one / 2;

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
