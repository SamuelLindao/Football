using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.SubStates
{
    /// <summary>
    /// Player steers to pass target. He switches to wait
    /// for ball if che reaches the target
    /// </summary>
    public class SteerToReceiveTarget : BState
    {
        /// <summary>
        /// The steering target
        /// </summary>
        public Vector3 SteeringTarget { get; set; }

        public override void Enter()
        {
            base.Enter();

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(SteeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();
           
            //check if now at target and switch to wait for ball
            if (Owner.IsAtTarget(SteeringTarget))
                Machine.ChangeState<WaitForBallAtReceiveTarget>();
        }

        public override void Exit()
        {
            base.Exit();

            //stop any steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
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
