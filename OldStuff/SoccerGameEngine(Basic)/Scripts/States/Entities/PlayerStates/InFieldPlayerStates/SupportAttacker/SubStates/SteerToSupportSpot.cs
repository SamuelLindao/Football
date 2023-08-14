using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.SubStates
{
    public class SteerToSupportSpot : BState
    {
        int waitTime;
        SupportSpot _newSupportSpot;

        /// <summary>
        /// The steering target
        /// </summary>
        public Vector3 _steeringTarget { get; set; }

        public override void Enter()
        {
            base.Enter();

            //init wait time
            waitTime = 3;

            //get the steering target
            SupportSpot = ((SupportAttackerMainState)Machine).SupportSpot;

            if (SupportSpot == null)
                Machine.ChangeState<SteerToHome>();
            else
            {
                _steeringTarget = SupportSpot.transform.position;

                // set some data
                SupportSpot.SetIsPickedOut(Owner);

                //set the steering to on
                Owner.RPGMovement.SetMoveTarget(_steeringTarget);
                Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
                Owner.RPGMovement.SetSteeringOn();
                Owner.RPGMovement.SetTrackingOn();
            }
        }

        public override void Execute()
        {
            base.Execute();

            //check if now at target and switch to wait for ball
            if (Owner.IsAtTarget(_steeringTarget))
                Machine.ChangeState<WaitAtTarget>();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //decrement wait time
            waitTime -= 1;

            //if I waited enough then consider going home
            if (waitTime <= 0)
            {
                //init wait time
                waitTime = 3;

                // get the new support spot
                _newSupportSpot = ((SupportAttackerMainState)Machine).SupportSpot;

                // get the parent steering target
                if (SupportSpot != _newSupportSpot)
                {
                    // set is picked to false
                    SupportSpot.SetIsNotPickedOut();

                    // if there is no longer any steering target then go to home
                    if (_newSupportSpot == null)
                    {
                        // set support spot to be null
                        SupportSpot = null;

                        // go to steer to home state
                        Machine.ChangeState<SteerToHome>();
                    }
                    else
                    {
                        // update support spot to new support spot
                        // if new support spot is not picked
                        //if (_newSupportSpot.IsPickedOut == false)
                        {
                            // update to the new support spot
                            SupportSpot = _newSupportSpot;

                            // set new support spot to is picked out
                            SupportSpot.SetIsPickedOut(Owner);

                            // update the steering target to the support spot position
                            _steeringTarget = SupportSpot.transform.position;
                        }
                    }
                }
            }

            //update the rpg movement
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }

        public SupportSpot SupportSpot { get; set; }
    }
}
