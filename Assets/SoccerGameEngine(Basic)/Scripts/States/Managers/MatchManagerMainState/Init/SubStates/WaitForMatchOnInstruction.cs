using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.Init.SubStates
{
    public class WaitForMatchOnInstruction : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to some events
            Owner.OnMesssagedToSwitchToMatchOn += Instance_OnMessagedToSwitchToMatchOn;

            //raise the event that I'm waiting for the match on instruction
            ActionUtility.Invoke_Action(Owner.OnEnterWaitForMatchOnInstruction);
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to some events
            Owner.OnMesssagedToSwitchToMatchOn -= Instance_OnMessagedToSwitchToMatchOn;

            //raise the event that I have exited the match on instruction
            ActionUtility.Invoke_Action(Owner.OnExitWaitForMatchOnInstruction);
        }

        private void Instance_OnMessagedToSwitchToMatchOn()
        {
            SuperMachine.ChangeState<MatchOnMainState>();
        }

        /// <summary>
        /// Returns the owner of this instance
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
