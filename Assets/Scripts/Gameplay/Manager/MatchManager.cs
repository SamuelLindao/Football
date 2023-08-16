using Cinemachine;
using Football.Gameplay.AI;
using Football.Gameplay.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Football.Gameplay.Manager
{
    public class MatchManager : MonoBehaviour
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
        private void Awake()
        {
            instance = this;

            time = initialTime;
            UpdateTimerText();
            SpawnBall();
            SpawnOfflinePlayers();
            InputManager.onSwitchPlayer += ControlPlayer;
        }

        private void Start()
        {
            ControlPlayer(currentControllingPlayer);
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
            Instantiate(ballPrefab, ballCenter.position, ballCenter.rotation);
            foreach (var player in players)
            {
                player.SetupCollision();
            }
        }

        public void SpawnOfflinePlayers()
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
    }
}
