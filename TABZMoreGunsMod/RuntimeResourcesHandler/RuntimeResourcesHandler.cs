using System;
using System.Collections.Generic;
using TABZMoreGunsMod;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System.Text;

namespace TABZMoreGunsMod.RuntimeResources
{
    public class RuntimeResourcesHandler
    {
        private static Dictionary<string,PhotonView> RuntimeResources = new Dictionary<string, PhotonView>();
        
        /// <summary>
        /// Stores the object in a dictionary with the name as the key, it sets the viewID to 0 and the object as inactive. Make a copy of it if you want to use it later.
        /// </summary>
        /// <param name="photonView"></param>
        /// <param name="name"></param>
        /// <returns>True if successful, false if there was already a object with that key</returns>
        public static bool AddResource(PhotonView photonView, string name)
        {
            if (RuntimeResources.ContainsKey(name))
            {
                Debug.Log(string.Format("A photonView with the name {0} already exists, ignoring the request",name));
                return false;
            }
            photonView.viewID = 0;
            photonView.gameObject.AddComponent<RuntimeResourcesTag>();
            photonView.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(photonView.gameObject);
            RuntimeResources.Add(name, photonView);
            return true;
        }
        
        
        /// <summary>
        /// Same as doing Resources.Load() and casting to GameObject, but adds the pool of runtime added objects.
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public static GameObject InstantiateGameObject(string prefabName)
        {
            try
            {
                if (RuntimeResources.ContainsKey(prefabName)) //For some reason this is the solution, no idea why we can't just return the magicCube param without creating a copy of it.
                {                               // We can destroy it right after doe (actually we can't, strange stuff happens when we do that)
                    var go = UnityEngine.Object.Instantiate(RuntimeResources[prefabName].gameObject);
                    //GameObject.Destroy(RuntimeResources[prefabName].gameObject);
                    GameObject.DontDestroyOnLoad(go);
                    //RuntimeResources[prefabName] = null;
                    return go;
                }

                return (GameObject)Resources.Load(prefabName, typeof(GameObject));
            }
            catch
            {
                Debug.Log("Couldn't instantiate " + prefabName);
                return null;
            }
        }

        public static GameObject ShallowCopyFrom(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var go = UnityEngine.Object.Instantiate(prefab, position, rotation);
            if (prefab.GetComponent<RuntimeResourcesTag>() != null)
            {
                go.SetActive(true);
                //go.GetComponent<PhotonView>().viewID = PhotonNetwork.AllocateViewID();
            }
            return go;
        }

        public static GameObject ShallowCopyOfGameObject(IPunPrefabPool objectPool, string prefabName, Vector3 position, Quaternion rotation)
        {
            var go = objectPool.Instantiate(prefabName, position, rotation);
            if (go.GetComponent<RuntimeResourcesTag>() != null)
            {
                go.SetActive(true);
                go.GetComponent<PhotonView>().viewID = PhotonNetwork.AllocateViewID();
            }
            return go;
        }

        public class RuntimeResourcesTag : MonoBehaviour
        {
        }
    }
}

