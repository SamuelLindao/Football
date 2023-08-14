using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class SteerToThreat : BState
    {
        float _waitTime;
        Vector3 _steeringTarget;

        Player _newThreat;

        public override void Enter()
        {
            base.Enter();

            //set wait time
            _waitTime = 2f;

            //set steering target
            Threat = ((PickOutThreatMainState)Machine).Threat;

            if (Threat == null)
                Machine.ChangeState<SteerToHome>();
            else
            {
                _steeringTarget = Threat.Position;

                // set threat is picked out
                Threat.SupportSpot.SetIsPickedOut(Owner);

                //set steering on
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

            _waitTime -= 1;

            if (_waitTime <= 0)
            {
                //reset the wait time
                _waitTime = 2;

                //get the steering target
                _steeringTarget = GetSteeringTarget();

                // get the support spot
                _newThreat = ((PickOutThreatMainState)Machine).Threat;

                if(_newThreat != Threat)
                {
                    // set is threat is picked out to true
                    Threat.SupportSpot.SetIsNotPickedOut();

                    // if there is no threat then go to home
                    if(_newThreat == null)
                    {
                        Threat = null;
                        Machine.ChangeState<SteerToHome>();
                    }
                    else
                    {
                        // update threat to new threat
                        Threat = _newThreat;

                        // pick out the new threat
                        Threat.SupportSpot.SetIsPickedOut(Owner);

                        // update the steering target
                        _steeringTarget = GetSteeringTarget();
                    }
                }

                //update the steering to target
                Owner.RPGMovement.SetMoveTarget(_steeringTarget);
                Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            }
        }

        public override void Exit()
        {
            base.Exit();

            //set steering off
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public Vector3 GetSteeringTarget()
        {
            //find direction to goal
            Vector3 directionOfThreatToGoal = Owner.TeamGoal.Position - Threat.Position;

            //the spot is somewhere between the threat and my goal
            Vector3 steeringTarget = Threat.Position
                + directionOfThreatToGoal.normalized
                * (Owner.ThreatTrackDistance + Owner.Radius);

            // return result
            return steeringTarget;
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }

        public Player Threat { get; set; }
    }
}
