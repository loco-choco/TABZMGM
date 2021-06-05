using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod
{
    public class MagicCubeTest : MonoBehaviour
    {
        public static GameObject CreateMagicCube()
        {
            var MagicCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MagicCube.name = "MagicCube";
            MagicCube.AddComponent<PhotonView>().viewID = 0;
            MagicCube.AddComponent<PhotonTransformView>();
            MagicCube.SetActive(false);
            return MagicCube;
        }
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("Spawnando cubo magico");
                Rigidbody rig = NetworkManager.LocalPlayerPhotonView.gameObject.GetComponent<PhysicsAmimationController>().mainRig;
                PhotonNetwork.Instantiate("MagicCube", rig.position, Quaternion.identity, 0);
            }
        }
    }
}
