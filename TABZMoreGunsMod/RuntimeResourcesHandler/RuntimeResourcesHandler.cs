using System;
using System.Collections.Generic;
using TABZMoreGunsMod;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using TABZMoreGunsMod.InventoryItemEditingHelper;

namespace TABZMoreGunsMod.RuntimeResources
{
    public class RuntimeResourcesHandler
    {
        private static Dictionary<string, GameObject> RuntimeGameObjectsResources = new Dictionary<string, GameObject>();

        private static Dictionary<string, object> RuntimeOtherResources = new Dictionary<string, object>();

        /// <summary>
        /// Stores the object in a dictionary with the name as the key, it sets the viewID to 0 and the object as inactive. Make a copy of it if you want to use it later.
        /// </summary>
        /// <param name="photonView"></param>
        /// <param name="name"></param>
        /// <returns>True if successful, false if there was already a object with that key</returns>
        public static bool AddResource(PhotonView photonView, string name)
        {
            if (RuntimeGameObjectsResources.ContainsKey(name))
            {
                Debug.Log(string.Format("A photonView with the name {0} already exists, ignoring the request", name));
                return false;
            }
            photonView.viewID = 0;
            photonView.gameObject.AddComponent<RuntimeResourcesTag>();
            photonView.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(photonView.gameObject);
            RuntimeGameObjectsResources.Add(name, photonView.gameObject);
            return true;
        }

        public static bool AddNonNetworkedResource(GameObject gameObject, string name)
        {
            if (RuntimeGameObjectsResources.ContainsKey(name))
            {
                Debug.Log(string.Format("A GameObject with the name {0} already exists, ignoring the request", name));
                return false;
            }
            gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(gameObject);
            RuntimeGameObjectsResources.Add(name, gameObject);
            return true;
        }

        public static Mesh GetMeshResource(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            if (RuntimeOtherResources.ContainsKey(fileName))
            {
                Debug.Log(string.Format("A Mesh with from the file {0} already exists, won't create new", fileName));
                return (Mesh)RuntimeOtherResources[fileName];
            }
            try
            {
                Mesh mesh = new CAMOWA.ObjImporter().ImportFile(Directory.GetFiles(MoreGunsMod.DllExecutablePath, fileName, SearchOption.AllDirectories)[0]);
                RuntimeOtherResources.Add(fileName, mesh);
                return mesh;
            }
            catch
            {
                Debug.Log(string.Format("Couldn't load the mesh {0}, make sure that it is inside the game folder", fileName));
                return null;
            }
        }
        public static Texture2D GetTexture2DResource(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            if (RuntimeOtherResources.ContainsKey(fileName))
            {
                Debug.Log(string.Format("A Texture2D from the file {0} already exists, won't create new", fileName));
                return (Texture2D)RuntimeOtherResources[fileName];
            }
            try
            {
                Texture2D texture2d = FileImporting.ImportImage(Directory.GetFiles(MoreGunsMod.DllExecutablePath, fileName, SearchOption.AllDirectories)[0]);
                RuntimeOtherResources.Add(fileName, texture2d);
                return texture2d;
            }
            catch
            {
                Debug.Log(string.Format("Couldn't load the Texture2D {0}, make sure that it is inside the game folder", fileName));
                return null;
            }
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
                if (RuntimeGameObjectsResources.ContainsKey(prefabName)) //For some reason this is the solution, no idea why we can't just return the magicCube param without creating a copy of it.
                {                               // We can destroy it right after doe (actually we can't, strange stuff happens when we do that)
                    var go = UnityEngine.Object.Instantiate(RuntimeGameObjectsResources[prefabName].gameObject);
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
