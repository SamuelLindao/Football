using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class WaitAtTarget : BState
    {
        int waitTime;
        Player _newThreat;
        Player _threat;

        public override void Enter()
        {
            base.Enter();

            //init wait time
            waitTime = 3;

            // get the support spot
            _threat = Machine.GetState<SteerToThreat>().Threat;

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
                _newThreat = ((PickOutThreatMainState)Machine).Threat;

                //if the new threat is equal to old threat steer to it
                //if threat has changed update to pick out new threat
                //if threat is null go to home
                if(_newThreat == null)
                {
                    if(_threat == null)
                    {
                        // steer to home if not at target
                        if (!Owner.IsAtTarget(Owner.HomeRegion.position))
                            Machine.ChangeState<SteerToHome>();
                    }
                    else
                    {
                        // set the threat for the SteerToThreat state
                        // Machine.GetState<SteerToThreat>().Threat = _newThreat;

                        // get the steering target
                        Vector3 steeringTarget = Machine.GetState<SteerToThreat>().GetSteeringTarget();

                        // find new pickout position
                        if (!Owner.IsAtTarget(steeringTarget))
                            Machine.ChangeState<SteerToThreat>();
                    }
                }
                else if(_newThreat == _threat)
                {
                    // get the steering target
                    Vector3 steeringTarget = Machine.GetState<SteerToThreat>().GetSteeringTarget();

                    // find new pickout position
                    if (!Owner.IsAtTarget(steeringTarget))
                        Machine.ChangeState<SteerToThreat>();
                }
                else if(_newThreat != _threat)
                {
                    // set old threat that he is no longer picked out
                    if(_threat != null)
                        _threat.SupportSpot.SetIsNotPickedOut();

                    // update support spot to new support spot
                    // if new support spot is not picked

                    // update to the new support spot
                    _threat = _newThreat;

                    // set new support spot to is picked out
                    _threat.SupportSpot.SetIsPickedOut(Owner);

                    // go to steer to support spot
                    Machine.ChangeState<SteerToThreat>();
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
