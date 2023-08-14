using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.SubStates
{
    /// <summary>
    /// The players waits for the ball to arrive at target. If the
    /// ball comes within control distance, the player controls the ball
    /// </summary>
    public class WaitForBallAtReceiveTarget : BState
    {
        public override void Enter()
        {
            base.Enter();

            //simply set tracking to be on
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            //update to the position of the ball
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.NormalizedPosition);

        }

        public override void Exit()
        {
            base.Exit();

            //set tracking to be off
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
