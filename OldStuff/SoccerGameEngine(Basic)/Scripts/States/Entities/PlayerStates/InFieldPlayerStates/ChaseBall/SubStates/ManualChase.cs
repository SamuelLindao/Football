using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TacklePlayer.MainState;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.SubStates
{
    public class ManualChase : BState
    {
        bool updateLogic;

        /// <summary>
        /// The steering target
        /// </summary>
        public Vector3 SteeringTarget { get; set; }

        public override void Enter()
        {
            base.Enter();

            // enable the user controlled icon
            Owner.IconUserControlled.SetActive(true);

            // set update logic
            updateLogic = false;

            //get the steering target
            SteeringTarget = Ball.Instance.NormalizedPosition;

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(SteeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
        }

        public override void Execute()
        {
            base.Execute();

            // update logic
            if(updateLogic)
            {
                //check if ball is within control distance
                if (Ball.Instance.Owner != null
                    && Owner.IsBallWithinControlableDistance())
                {
                    //tackle player
                    SuperMachine.ChangeState<TackleMainState>();
                }
                else if (Owner.IsBallWithinControlableDistance())
                {
                    // control ball
                    SuperMachine.ChangeState<ControlBallMainState>();
                }

                //get the steering target
                SteeringTarget = Ball.Instance.NormalizedPosition;

                //set the steering to on
                Owner.RPGMovement.SetMoveTarget(SteeringTarget);
                Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
            }

            // listen to key events
            if(Input.GetButton("Pass/Press"))
            {
                // set update logic
                if(updateLogic == false)
                    updateLogic = true;

                // set steering
                if(Owner.RPGMovement.Steer == false)
                    Owner.RPGMovement.SetSteeringOn();
                if(Owner.RPGMovement.Track == false)
                    Owner.RPGMovement.SetTrackingOn();
            }
            else if(Input.GetButtonUp("Pass/Press"))
            {
                // set update logic
                updateLogic = false;

                // set steering
                Owner.RPGMovement.SetSteeringOff();
                Owner.RPGMovement.SetTrackingOff();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // disable the user controlled icon
            Owner.IconUserControlled.SetActive(false);

            //set the steering to on
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
