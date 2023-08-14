using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Init.MainState
{
    public class InitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // disable the user controlled icon
            Owner.IconUserControlled.SetActive(false);

            //init
            Owner.Init();

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
