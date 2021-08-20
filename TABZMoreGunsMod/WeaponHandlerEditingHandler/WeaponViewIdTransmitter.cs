using UnityEngine;
using System.Collections.Generic;

namespace TABZMoreGunsMod.WeaponHandlerEditingHandler
{
    public class WeaponViewIdTransmitter:MonoBehaviour
    {
        private PhotonView playerPhotonView;
        private Dictionary<string, int> ReceivedIds;
        public void Awake()
        {
            playerPhotonView = gameObject.GetPhotonView();
            ReceivedIds = new Dictionary<string, int>();
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
    }
}
