using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Football.Multiplayer.Matchmake
{
    public class MatchmakeController : MonoBehaviour
    {
        public GameObject beforeConnectionScreen;
        public GameObject joiningRoomScreen;
        public Transform roomScrollView;
        public GameObject roomInfo;
        public TMP_InputField roomNameInput;

        List<GameObject> roomInfoInstances = new List<GameObject>();

        private void OnEnable()
        {
            MultiplayerManager.onRoomListUpdated += OnRoomListUpdated;
            MultiplayerManager.onJoinedLobby += OnConnectedToPhotonServer;
        }

        private void Awake()
        {
            beforeConnectionScreen.SetActive(true);
        }

        public void OnConnectedToPhotonServer()
        {
            beforeConnectionScreen.SetActive(false);
        }

        public void OnRoomListUpdated(List<RoomInfo> roomInfo)
        {
            foreach (var roomInfoInstance in roomInfoInstances)
            {
                Destroy(roomInfoInstance);
            }

            roomInfoInstances.Clear();

            foreach (var room in roomInfo)
            {
                var roomInfoPanel = Instantiate(this.roomInfo, roomScrollView).GetComponent<MatchmakeRoomPanel>();
                roomInfoPanel.roomName.text = room.Name;
                roomInfoPanel.roomPlayers.text = $"{room.PlayerCount}/{room.MaxPlayers}";

                if (room.PlayerCount == room.MaxPlayers)
                {
                    roomInfoPanel.roomJoin.interactable = false;
                    roomInfoPanel.roomJoinText.text = "Full";
                }
                else
                {
                    roomInfoPanel.roomJoin.onClick.AddListener(() =>
                    {
                        JoinMatch(room.Name);
                    });
                }

                roomInfoPanel.gameObject.SetActive(true);

                roomInfoInstances.Add(roomInfoPanel.gameObject);
            }
        }

        public void CreateMatch()
        {
            if (!String.IsNullOrEmpty(roomNameInput.text))
            {
                JoinMatch(roomNameInput.text);
            }
        }

        public void JoinMatch(string name)
        {
            joiningRoomScreen.SetActive(true);
            MultiplayerManager.ConnectToMatch(name);
        }
    }
}
