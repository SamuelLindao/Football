using System;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Defend.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.SubStates
{
    public class WaitForKickOff : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to opponent ontake-kick-off event
            Owner.Opponent.OnTakeKickOff += Instance_OnOpponentTakeKickOff;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to opponent ontake-kick-off event
            Owner.Opponent.OnTakeKickOff -= Instance_OnOpponentTakeKickOff;
        }

        private void Instance_OnOpponentTakeKickOff()
        {
            SuperMachine.ChangeState<DefendMainState>();
        }

        public Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
