using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.SubStates
{
    public class WaitAtTarget : BState
    {
        int waitTime;
        SupportSpot _newSupportSpot;
        SupportSpot _supportSpot;

        public override void Enter()
        {
            base.Enter();

            //init wait time
            waitTime = 3;

            // get the support spot
            _supportSpot = Machine.GetState<SteerToSupportSpot>().SupportSpot;

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

            //decrement wait time
            waitTime -= 1;

            //if I waited enough then consider going home
            if (waitTime <= 0)
            {
                //init wait time
                waitTime = 3;

                //get the steering target
                _newSupportSpot = ((SupportAttackerMainState)Machine).SupportSpot;

                if(_newSupportSpot == null)
                {
                    if (_supportSpot == null)
                    {
                        // go to home if not at home position
                        if (!Owner.IsAtTarget(Owner.HomeRegion.position))
                            Machine.ChangeState<SteerToHome>();
                    }
                    else
                    {
                        // go to home if not at support spot
                        if (!Owner.IsAtTarget(_supportSpot.transform.position))
                            Machine.ChangeState<SteerToSupportSpot>();
                    }
                }
                else if(_newSupportSpot == _supportSpot)
                {
                    // go to home if not at support spot
                    if (!Owner.IsAtTarget(_supportSpot.transform.position))
                        Machine.ChangeState<SteerToSupportSpot>();
                }
                else if(_newSupportSpot != _supportSpot)
                {
                    // set is picked to false
                    if(_supportSpot != null)
                        _supportSpot.SetIsNotPickedOut();

                    // update support spot to new support spot
                    // if new support spot is not picked

                    // update to the new support spot
                    _supportSpot = _newSupportSpot;

                    // set new support spot to is picked out
                    _supportSpot.SetIsPickedOut(Owner);

                    // go to steer to support spot
                    Machine.ChangeState<SteerToSupportSpot>();
                }
            }
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
