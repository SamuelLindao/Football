using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using System.Linq;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerSupportAttackerTester : MonoBehaviour
    {
        public Player _primaryPlayer001;
        public Player _primaryPlayer002;
        public Player _secondaryPlayer;
        public Transform _pitcPoints;

        private void Awake()
        {
            //set the positions
            _primaryPlayer001.PlayerSupportSpots = _pitcPoints
                .GetComponentsInChildren<SupportSpot>()
                .ToList();

            //set the positions
            _primaryPlayer002.PlayerSupportSpots = _pitcPoints
                .GetComponentsInChildren<SupportSpot>()
                .ToList();
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        private void Update()
        {
            _secondaryPlayer.PlaceBallInfronOfMe();
            _primaryPlayer001.InFieldPlayerFSM.GetState<SupportAttackerMainState>().PositionInConsideration = _secondaryPlayer.Position;
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer001.Init(15f, 5f, 15f, 5f, 15f, 5f);
            //_primaryPlayer002.Init(15f, 5f, 15f, 5f, 15f, 5f);

            _primaryPlayer001.Init();
            _primaryPlayer002.Init();

            //set the state to supprt attacker
            _primaryPlayer001.InFieldPlayerFSM.ChangeState<SupportAttackerMainState>();
            _primaryPlayer002.InFieldPlayerFSM.ChangeState<SupportAttackerMainState>();
        }
    }
}
