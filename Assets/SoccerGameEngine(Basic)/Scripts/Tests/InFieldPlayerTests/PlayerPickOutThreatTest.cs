using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerPickOutThreatTest : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;

        private void Awake()
        {
            //Ball.Instance.Owner = _secondaryPlayer;

            //_secondaryPlayer.PlaceBallInfronOfMe();
            //_secondaryPlayer.OnTackled += Instance_OnTackled;
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        private void Init()
        {
            //init primary player
            //_primaryPlayer.Init(30f, 5f, 15f, 5f, 30f, 5f);
            //_secondaryPlayer.Init(30f, 5f, 15f, 5f, 30f, 5f);

            _primaryPlayer.Init();
            _secondaryPlayer.Init();

            //set the state to supprt attacker
            _primaryPlayer.InFieldPlayerFSM.GetState<PickOutThreatMainState>().Threat = _secondaryPlayer;
            _primaryPlayer.InFieldPlayerFSM.ChangeState<PickOutThreatMainState>();
        }
    }
}
