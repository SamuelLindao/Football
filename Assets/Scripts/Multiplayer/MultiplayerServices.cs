using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Football.Multiplayer
{
    public class MultiplayerServices : IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks, ILobbyCallbacks, IWebRpcCallback, IErrorInfoCallback
    {
        public void Enable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Disable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }


        public void OnConnected()
        {
            Debug.Log("Connected to photon server");
        }

        public void OnLeftRoom()
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnJoinedLobby()
        {
            MultiplayerManager.onJoinedLobby();
        }

        public void OnLeftLobby()
        {
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            MultiplayerManager.onRoomListUpdated(roomList);
        }

        public void OnJoinedRoom()
        {
            MultiplayerManager.onJoinedRoom();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server");
            PhotonNetwork.JoinLobby();
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnWebRpcResponse(OperationResponse response)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnErrorInfo(ErrorInfo errorInfo)
        {
        }
    }
}
