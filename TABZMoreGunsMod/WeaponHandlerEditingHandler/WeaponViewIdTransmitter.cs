using UnityEngine;
using System.Collections.Generic;

namespace TABZMoreGunsMod.WeaponHandlerEditingHandler
{
    public class WeaponViewIdTransmitter : MonoBehaviour
    {
        private PhotonView playerPhotonView;
        private Dictionary<string, int> ReceivedIds;
        private List<KeyValuePair<string, PhotonView>> SetOnStart;
        public void Awake()
        {
            playerPhotonView = gameObject.GetPhotonView();
            ReceivedIds = new Dictionary<string, int>();
            SetOnStart = new List<KeyValuePair<string, PhotonView>>();
        }
        public void Start()
        {
            for (int i = 0; i < SetOnStart.Count; i++)
            {
                SetOnStart[i].Value.viewID = GetWeaponViewId(SetOnStart[i].Key);
            }
            SetOnStart.Clear();
            SetOnStart = null;
        }
        public void AllocateManualPhotonView(string weaponName)
        {
            if (playerPhotonView == null)
            {
                Debug.LogError("Player View wasn't found, can't allocate ViewId");
                return;
            }
            if (!playerPhotonView.isMine)
            {
                Debug.LogError("Can't allocate photon view in remote version");
            }
            int allocatedID = PhotonNetwork.AllocateViewID();
            Debug.Log(string.Format("Sending[{0}/{1}] viewId for {2}({3})", PhotonNetwork.player.ID, playerPhotonView.ownerId, weaponName, allocatedID));
            playerPhotonView.RPC("ReceiveCorrectWeaponViewId", PhotonTargets.AllBuffered, weaponName, allocatedID);
        }
        [PunRPC]
        public void ReceiveCorrectWeaponViewId(string weaponName, int viewID)
        {
            Debug.Log(string.Format("Received[{0}/{1}] viewId for {2}({3})", PhotonNetwork.player.ID, playerPhotonView.ownerId, weaponName, viewID));
            ReceivedIds.Add(weaponName, viewID);
        }
        public int GetWeaponViewId(string weaponName)
        {
            ReceivedIds.TryGetValue(weaponName, out int id);
            return id;
        }
        public void SetWeaponIdOnStart(PhotonView photonView, string weaponName)
        {
            if (SetOnStart != null)
                SetOnStart.Add(new KeyValuePair<string, PhotonView>(weaponName, photonView));
        }
    }
}
