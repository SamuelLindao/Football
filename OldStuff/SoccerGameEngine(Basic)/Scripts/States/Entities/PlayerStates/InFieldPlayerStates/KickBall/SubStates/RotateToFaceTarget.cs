using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class RotateToFaceTarget : BState
    {
        float waitTime;
        Vector3 _kickTarget;

        public override void Enter()
        {
            base.Enter();

            // set the wait time
            waitTime = 1.5f;

            // get the kick target
            _kickTarget = (Vector3)Owner.KickTarget;

            // check if target is infront of me
            bool isPositionInFrontOfMe = Owner.IsInfrontOfPlayer(_kickTarget);

            // proceed to check kick type if infront of me
            // try to face target
            if(isPositionInFrontOfMe)
                Machine.ChangeState<CheckKickType>();
            else
            {
                // set new speed
                Owner.RPGMovement.Speed *= 0.95f;

                //listen to game events
                Owner.OnTackled += Instance_OnTackled;

                //set the ball to is kinematic
                Ball.Instance.Owner = Owner;
                Ball.Instance.Rigidbody.isKinematic = true;

                // set steering
                Owner.RPGMovement.SetRotateFacePosition(_kickTarget);
                Owner.RPGMovement.SetTrackingOn();
            }
        }

        public override void Execute()
        {
            base.Execute();

            // decrement wait time
            waitTime -= Time.deltaTime;

            // if we have exhausted the wait time go to control ball main state
            //place ball infront of me
            if (waitTime <= 0)
                SuperMachine.ChangeState<ControlBallMainState>();
            else
                Owner.PlaceBallInfronOfMe();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if position is now in front of me,
            // pass to it if pass still safe
            // else go back to control ball
            if (Owner.IsInfrontOfPlayer(_kickTarget))
            {
                if (Owner.KickType == KickType.Pass)
                {
                    Machine.ChangeState<CheckKickType>();
                    //if (Owner.IsUserControlled == true)
                    //    Machine.ChangeState<CheckKickType>();
                    //else if(Owner.CanPass(_kickTarget, true, false, Owner.PassReceiver))
                    //    Machine.ChangeState<CheckKickType>();
                    //else
                    //    SuperMachine.ChangeState<ControlBallMainState>();
                }
                else
                    Machine.ChangeState<CheckKickType>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // restore player speed
            Owner.RPGMovement.Speed = Owner.ActualSpeed;

            //listen to game events
            Owner.OnTackled -= Instance_OnTackled;

            //unset the ball to is kinematic
            Ball.Instance.Owner = null;
            Ball.Instance.Rigidbody.isKinematic = false;

            //stop steering
            //Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public void Instance_OnTackled()
        {
            SuperMachine.ChangeState<TackledMainState>();
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
