using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.SubStates
{
    public class WaitAtHome : BState
    {
        public override void Enter()
        {
            base.Enter();

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            //update the track position
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.NormalizedPosition);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //steer if not at target
            if (!Owner.IsAtTarget(Owner.HomeRegion.position))
                Machine.ChangeState<SteerToHome>();
        }

        public Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
