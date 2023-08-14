using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.SubStates
{
    public class GetSupportSport : BState
    {
        public override void Enter()
        {
            base.Enter();

            // get the support spot ffrom the main state
            //if no support spot then go to home
            SupportSpot supportSpot = ((SupportAttackerMainState)Machine).SupportSpot;

            if (supportSpot == null)
                Machine.ChangeState<SteerToHome>();
            else
                Machine.ChangeState<SteerToSupportSpot>();
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
