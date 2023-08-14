using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerChaseBallTester : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;

        private void Awake()
        {
            Ball.Instance.Owner = _secondaryPlayer;

            _secondaryPlayer.PlaceBallInfronOfMe();
            _secondaryPlayer.OnTackled += Instance_OnTackled;
        }

        private void Instance_OnTackled()
        {
            _secondaryPlayer.InFieldPlayerFSM.ChangeState<TackledMainState>();
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer.Init(15f, 5f, 15f, 5f, 30f, 5f);
            //_secondaryPlayer.Init(15f, 5f, 15f, 5f, 30f, 5f);

            _primaryPlayer.Init();
            _secondaryPlayer.Init();

            //set the state to supprt attacker
            _primaryPlayer.InFieldPlayerFSM.ChangeState<ChaseBallMainState>();
        }
    }
}
