using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Multiplayer
{
    public static class MultiplayerManager
    {
        public static MultiplayerServices services;
        private static AppSettings appSettings;
        private const string appId = "34d98825-c2b1-4b4f-a41e-d8c10e6e6d81";

        internal static int maxPlayersCount = 2;
        internal static Action<List<RoomInfo>> onRoomListUpdated;
        internal static Action onJoinedRoom;
        internal static Action onJoinedLobby;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            services = new MultiplayerServices();
            appSettings = new AppSettings();
            services.Enable();
            SetCloudId(appId);
            SetCloudVersion();
            Connect();
        }

        private static void SetCloudId(string appId)
        {
            appSettings.AppIdRealtime = appId;
        }

        private static void SetCloudVersion()
        {
            appSettings.AppVersion = $"{Application.version} u{Application.unityVersion}";
        }

        private static void Connect()
        {
            PhotonNetwork.ConnectUsingSettings(appSettings);
        }

        public static void ConnectToMatch(string matchName)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersCount;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.EmptyRoomTtl = 0;
            PhotonNetwork.JoinOrCreateRoom(matchName, roomOptions, null);
        }
    }
}
