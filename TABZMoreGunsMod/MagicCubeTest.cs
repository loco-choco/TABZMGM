using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod
{
    public class MagicCubeTest : MonoBehaviour
    {
        static public GameObject MagicCube;
        static public Dictionary<string, GameObject> PrefabCacheRef;
        void Start()
        {
            if (MagicCube == null)
            {
                MagicCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                MagicCube.name = "MagicCube";
                MagicCube.AddComponent<PhotonView>().viewID = 0;
                MagicCube.AddComponent<PhotonTransformView>();
                MagicCube.SetActive(false);
            }
            if (!PrefabCacheRef.ContainsKey("MagicCube"))
            {
                PrefabCacheRef.Add("MagicCube", MagicCube);
            }

        }
        void Update()
        {
            
            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("Spawnando cubo magico");
                Rigidbody rig = NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig;
                PhotonNetwork.Instantiate("MagicCube",
                    rig.position, Quaternion.identity, 0);
            }
        }

        void OnDestroy()
        {
            if (PrefabCacheRef.ContainsKey("MagicCube"))
            {
                Debug.Log("Copia do MagicCube ainda no PrefabCache, removendo ele de la ...");
                PrefabCacheRef.Remove("MagicCube");
            }
        }
    }
}
