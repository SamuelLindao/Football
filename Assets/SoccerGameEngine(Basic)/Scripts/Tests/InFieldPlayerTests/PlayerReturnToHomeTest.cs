using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerReturnToHomeTest : MonoBehaviour
    {
        public Player PlayerControlling;

        private void Start()
        {
            Invoke("Init", 1f);
        }

        void Init()
        {
            PlayerControlling.InFieldPlayerFSM.SetCurrentState<GoToHomeMainState>();
            PlayerControlling.InFieldPlayerFSM.GetState<GoToHomeMainState>().Enter();
            //PlayerControlling.Init(15f, 5f, 15f, 5f, 10f, 5f);
            PlayerControlling.Init();
        }
    }
}
