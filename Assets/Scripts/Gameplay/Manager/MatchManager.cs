using Cinemachine;
using Football.Gameplay.AI;
using Football.Gameplay.Input;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Football.Gameplay.Manager
{
    public class MatchManager : MonoBehaviourPun, IPunObservable
    {
        public static MatchManager instance;

        public int leftTeamScore;
        public int rightTeamScore;

        public TMP_Text leftSideScoreText;
        public TMP_Text rightSideScoreText;
        public TMP_Text timeText;
        public int initialTime = 0;
        public float timeSpeed = 0.5f;

        public CinemachineVirtualCamera virtualCamera;

        float time;

        int minutes;
        int seconds;

        public ParticleSystem confetti;

        public GameObject aiPrefab;
        public GameObject ballPrefab;

        public Formation leftSide;
        public Formation rightSide;

        public Transform ballCenter;

        public bool isOnline;

        public List<Player> players = new List<Player>();
        public List<Player> controllablePlayers = new List<Player>();
        public int currentControllingPlayer = 0;
        public PhotonView photonView;
        public InputData playerOne;
        public InputData playerTwo;

        private void Awake()
        {
            instance = this;
            photonView = gameObject.AddComponent<PhotonView>();
            photonView.isRuntimeInstantiated = false;
            photonView.FindObservables(true);

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                time = initialTime;
                UpdateTimerText();
                SpawnBall();
                photonView.RPC("SpawnPlayers", RpcTarget.AllBuffered);
                ControlPlayer(currentControllingPlayer);
                InputManager.onSwitchPlayer += ControlPlayer;
            }

        }

        public void ControlPlayer(int index)
        {
            currentControllingPlayer = index;
            foreach (Player player in players)
            {
                player.playerIsControllingThis = false;
            }
            var controlPlayer = controllablePlayers[Mathf.Clamp(currentControllingPlayer, 0, controllablePlayers.Count - 1)];
            controlPlayer.playerIsControllingThis = true;
            Player.mainPlayer = controlPlayer;
            virtualCamera.Follow = controlPlayer.transform;
            virtualCamera.LookAt = controlPlayer.transform;
        }

        public void UpdateScore()
        {
            leftSideScoreText.text = leftTeamScore.ToString();
            rightSideScoreText.text = rightTeamScore.ToString();
        }

        public void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerOne = InputManager.input;
            }
            else
            {
                playerTwo = InputManager.input;
            }

            time = Mathf.Clamp(time + (timeSpeed), 0, float.MaxValue);
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            minutes = Mathf.FloorToInt(time / 60);
            seconds = Mathf.FloorToInt(time % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = timeString;
        }

        public void InstantiatePlayer(PlayerType type, bool controllable, PlayerFormation formation, Vector3 position, Quaternion rotation, bool isOpponent = true)
        {
            GameObject player = null;

            switch (type)
            {
                case PlayerType.Local:
                    player = Instantiate(aiPrefab, position, rotation);
                    break;
                case PlayerType.Online:
                    Debug.LogError("Not implemented yet");
                    break;
            }

            var playerComponent = player.GetComponent<Player>();
            playerComponent.scoreSide = isOpponent ? GoalSide.Left : GoalSide.Right;
            var fsmAIComponent = player.GetComponent<FSMSoccer>();
            fsmAIComponent.side = isOpponent ? GoalSide.Right : GoalSide.Left;
            fsmAIComponent.formation = formation;

            players.Add(playerComponent);

            if (controllable)
            {
                controllablePlayers.Add(playerComponent);
            }
        }

        public void SpawnBall()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(ballPrefab.name, ballCenter.position, ballCenter.rotation);
                foreach (var player in players)
                {
                    player.SetupCollision();
                }
            }
        }

        [PunRPC]
        public void SpawnPlayers()
        {
            InstantiatePlayer(PlayerType.Local, false, PlayerFormation.Goalkeeper, leftSide.Goalkeeper.position, leftSide.Goalkeeper.rotation, false);
            InstantiatePlayer(PlayerType.Local, false, PlayerFormation.Goalkeeper, rightSide.Goalkeeper.position, rightSide.Goalkeeper.rotation);

            int randomLocalPlayerPosition = Random.Range(0, 4);

            for (int i = 0; i < 4; i++)
            {
                var position = leftSide.GetPositionByIndex(i);
                if (randomLocalPlayerPosition == i)
                {
                    InstantiatePlayer(PlayerType.Local, true, PlayerFormation.Line, position.position, position.rotation, false);
                }
                else
                {
                    InstantiatePlayer(PlayerType.Local, true, PlayerFormation.Line, position.position, position.rotation, false);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                var position = rightSide.GetPositionByIndex(i);
                InstantiatePlayer(PlayerType.Local, false, PlayerFormation.Line, position.position, position.rotation);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    stream.SendNext(playerOne.move);
                    stream.SendNext(playerOne.through);
                    stream.SendNext(playerOne.shoot);
                    stream.SendNext(playerOne.pass);
                    stream.SendNext(playerOne.sprintAndSkill);
                    stream.SendNext(playerOne.currentPlayer);
                }
                else
                {
                    stream.SendNext(playerTwo.move);
                    stream.SendNext(playerTwo.through);
                    stream.SendNext(playerTwo.shoot);
                    stream.SendNext(playerTwo.pass);
                    stream.SendNext(playerTwo.sprintAndSkill);
                    stream.SendNext(playerTwo.currentPlayer);
                }
            }
            else
            {
                var playerMove = (Vector2)stream.ReceiveNext();
                var playerThrough = (bool)stream.ReceiveNext();
                var playerShoot = (bool)stream.ReceiveNext();
                var playerPass = (bool)stream.ReceiveNext();
                var playerSprintAndSkill = (bool)stream.ReceiveNext();
                var playerCurrentPlayer = (int)stream.ReceiveNext();

                if (PhotonNetwork.IsMasterClient)
                {
                    playerTwo.move = playerMove;
                    playerTwo.through = playerThrough;
                    playerTwo.shoot = playerShoot;
                    playerTwo.pass = playerPass;
                    playerTwo.sprintAndSkill = playerSprintAndSkill;
                    playerTwo.currentPlayer = playerCurrentPlayer;
                }
                else
                {
                    playerOne.move = playerMove;
                    playerOne.through = playerThrough;
                    playerOne.shoot = playerShoot;
                    playerOne.pass = playerPass;
                    playerOne.sprintAndSkill = playerSprintAndSkill;
                    playerOne.currentPlayer = playerCurrentPlayer;
                }
            }
        }
    }
}
