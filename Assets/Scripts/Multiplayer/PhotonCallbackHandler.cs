using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Multiplayer
{
    public class PhotonCallbackHandler
    {
        private static PhotonCallbackHandler instance;
        private static bool joinedRoom;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Create()
        {
            instance = new PhotonCallbackHandler();
            instance.Init();
        }

        private void Init()
        {
            MultiplayerManager.onJoinedRoom += OnJoinedRoom;
        }

        public void OnJoinedRoom()
        {
            Debug.Log("Joined on room");
            PhotonNetwork.LoadLevel("Gameplay");
            joinedRoom = true;
        }
    }
}
