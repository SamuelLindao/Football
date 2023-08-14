using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class SteerToHome : BState
    {
        int waitTime;
        Vector3 _steeringTarget;
        Player _threat;

        public override void Enter()
        {
            base.Enter();

            //init wait time
            waitTime = 3;

            //get the steering target
            _steeringTarget = Owner.HomeRegion.position;

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
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

                // get the support spot
                _threat = ((PickOutThreatMainState)Machine).Threat;

                // if there is a steering target then go to move to support spot
                if (_threat != null)
                    Machine.ChangeState<SteerToThreat>();
            }

            //update the steering target
            _steeringTarget = Owner.HomeRegion.position;

            //update the rpg movement
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
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
