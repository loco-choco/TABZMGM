//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using TABZMoreGunsMod.RuntimeResources;
//using TABZMoreGunsMod.InventoryItemEditingHelper;
//using TABZMoreGunsMod.WeaponHandlerEditingHandler;
//using TABZMoreGunsMod.WeaponMakerHelper;
//using System.IO;

//Fire And Melee Weapon Examples in code

//namespace TABZMoreGunsMod
//{
//    public class MagicCubeTest : MonoBehaviour
//    {
//        static private MeleeWeaponSettings WeaponMeleeSettings;
//        static private FireWeaponSettings WeaponFireSettings;
//        private static Mesh needleMesh;

//        static public void CreateWeapon()
//        {
//            try
//            {
//                if (needleMesh == null)
//                    needleMesh = new CAMOWA.ObjImporter().ImportFile(Directory.GetFiles(Application.dataPath, "needle.obj", SearchOption.AllDirectories)[0]);
//            }
//            catch
//            {
//                Debug.Log("Couldn't load the mesh needle.obj, make sure that it is inside the game folder");
//            }
//            //string path = "";
//            //try { path = Directory.GetFiles(Application.dataPath, "icon.png", SearchOption.AllDirectories)[0]; }
//            //catch
//            //{
//            //    Debug.Log("Couldn't load the image icon.png, make sure that it is inside the game folder");
//            //}
//            //Texture2D image = FileImporting.ImportImage(path);

//            //ItemWeaponSettings itemSettings = new ItemWeaponSettings()
//            //{
//            //    DisplayName = "Needle",
//            //    FavourText = "<b>Wishlist\n<color=orange>Straight To The Point</color></b>",
//            //    ItemMaterial = new Material(Shader.Find("Standard"))
//            //    {
//            //        color = Color.grey
//            //    },
//            //    ItemMesh = needleMesh,
//            //    ItemIcon = image,
//            //    AmmoType = InventoryService.AmmoType.Small,
//            //    BulletsInMagazine = 2000
//            //};
//            //var MagicCubeItem = ItemWeaponMakerHelper.MakeItemWeapon(itemSettings);
//            //MagicCubeItem.transform.localScale = Vector3.one / 2f;

//            //WeaponMeleeSettings = new MeleeWeaponSettings()
//            //{
//            //    Name = "Needle",
//            //    WeaponMesh = needleMesh,
//            //    WeaponMaterial = new Material(Shader.Find("Standard"))
//            //    {
//            //        color = Color.grey
//            //    },
//            //    FOV = 70,
//            //    PunchCurve = AnimationCurve.Linear(0f, 1f, 2f, 0f),
//            //    PunchForce = 2000f,
//            //    PunchTime = 0.1f,
//            //    PunchRate = 1f,
//            //    BoxColliders = new TransformSettings[] {
//            //        new TransformSettings()
//            //        {
//            //            Position = Vector3.zero,
//            //            Rotation = new Vector3(60f, 0f, 0f),
//            //            Scale = new Vector3(0.3f, 1f, 6f)
//            //        }
//            //    },

//            //    MeshTransform = new TransformSettings()
//            //    {
//            //        Position = Vector3.zero,
//            //        Rotation = new Vector3(60f, 0f, 0f),
//            //        Scale = Vector3.one
//            //    },

//            //    MeleeMultiplier = 1f,
//            //    MeleeWeaponMultiplier = 1f,
//            //    WeaponRightHand_Position = Vector3.zero,
//            //    IsTwoHandedWeapon = false,
//            //    NoiseInterval = 0f,
//            //    NoiseHearableDistance = 50f,
//            //    NoiseLoudness = 0.3f,
//            //};
//            var ProjectileWeaponSettings = new WeaponProjectileSettings()
//            {
//                BulletLifeTime = 60f,
//                Damage = 10f,
//                Force = 100f,
//                ProjectilyTypeToBeBasedOn = ProjectileTypes.Bullet_Sniper,
//                //HitEffect = ProjectileHitEffects.MusketHitEffect, //broken?
//                RayLenght = 2f,
//                InitialFowardsForce = 5f,
//                //InitialUpwardsForce = 15f
//            };
//            var Projectile = WeaponProjectileMakerHelper.MakeProjectile(ProjectileWeaponSettings);
//            Projectile.name = "AAAAAAAAA";
//            RuntimeResourcesHandler.AddNonNetworkedResource(Projectile, Projectile.name);

//            WeaponFireSettings = new FireWeaponSettings()
//            {
//                Name = "Needle",
//                WeaponMesh = needleMesh,
//                WeaponMaterial = new Material(Shader.Find("Standard"))
//                {
//                    color = Color.grey,
                    
//                },
//                Automatic = true,
//                Recoil = -Vector3.forward*10000f,
//                ADS_Position = -Vector3.forward / 2f,
//                WeaponProjectile = Projectile.name,
//                MagazineSize = 1000,
//                FirePoint_Position = Vector3.forward / 4f,
//                FOV = 60,
//                FireRate = 0.5f,
//                ReloadTime = 10f,

//                BoxColliders = new TransformSettings[]{
//                    new TransformSettings()
//                    {
//                        Position = Vector3.zero,
//                        Rotation = Vector3.zero,
//                        Scale = Vector3.one
//                    }
//                },

//                MeshTransform = new TransformSettings()
//                {
//                    Position = Vector3.zero,
//                    Rotation = Vector3.zero,
//                    Scale = Vector3.one / 10f
//                },

//                WeaponRightHand_Position = Vector3.zero,
//                IsTwoHandedWeapon = false,

//                NoiseInterval = 0f,
//                NoiseHearableDistance = 200f,
//                NoiseLoudness = 100f,
//            };
//            SoundEventsManager.WeaponSoundWrapper WeaponSounds = new SoundEventsManager.WeaponSoundWrapper()
//            {
//                WeaponEvent = NoiseWeaponEvents.RifleMusket,
//                HitEvent = NoiseWeaponHitEvents.SMallHit,
//                ReloadEvent = NoiseWeaponReloadEvents.GenericReload
//            };

//            //RuntimeResourcesHandler.AddResource(MagicCubeItem.GetComponent<PhotonView>(), "MagicCubeItem");

//            WeaponHandlerEditingHelper.AddWeaponToList("Needle", WeaponSounds, MakeMagicCube);
//            Debug.Log("Objects were created!");
//        }

//        void Update()
//        {

//            if (Input.GetKeyUp(KeyCode.N))
//            {
//                Debug.Log("Spwning Magic Cube");
//                PhotonNetwork.Instantiate("Needle", NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig.position, Quaternion.identity, 0, new object[] { 200 });
//            }
//        }
//        static public Weapon MakeMagicCube(Transform playerTransform)
//        {
//            try
//            {
//                //Weapon weapon = MeleeMakerHelper.MakeMeleeWeapon(playerTransform, WeaponMeleeSettings);
//                Weapon weapon = FireWeaponMakerHelper.MakeFireWeapon(playerTransform, WeaponFireSettings);
//                weapon.transform.localScale = Vector3.one / 2;

//                return weapon;
//            }
//            catch (Exception ex)
//            {
//                Debug.Log(ex.Message);
//                Debug.Log(ex.Source);
//                Debug.Log(ex.StackTrace);
//                Debug.Log(ex.InnerException.Message);

//                return null;
//            }
//        }
//    }
//}
