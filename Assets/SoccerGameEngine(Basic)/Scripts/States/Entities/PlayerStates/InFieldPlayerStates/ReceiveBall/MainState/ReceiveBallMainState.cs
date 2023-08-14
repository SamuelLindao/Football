using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState
{
    /// <summary>
    /// The player steers to the pass target and waits for the ball
    /// there. If ball comes within range the player controls the ball. If
    /// the player receives a message that the team has lost control he
    /// goes back to home
    /// </summary>
    public class ReceiveBallMainState : BHState
    {
        float _ballTime;

        public override void AddStates()
        {
            base.AddStates();

            //add the state
            AddState<SteerToReceiveTarget>();
            AddState<WaitForBallAtReceiveTarget>();

            //set the initial state
            SetInitialState<SteerToReceiveTarget>();
        }

        public override void Enter()
        {
            base.Enter();

            // set me as the ball owner
            // Ball.Instance.Owner = Owner;

            //register to some player events
            Owner.OnTeamLostControl += Instance_OnTeamLostControl;
        }

        public override void Execute()
        {
            base.Execute();

            // decrement ball trap time
            if(_ballTime > 0)
                _ballTime -= Time.deltaTime;

            //trap the ball if it is now in a trapping distance
             if (Owner.IsBallWithinControlableDistance())
                 Machine.ChangeState<ControlBallMainState>();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if we have exhausted ball time, chase down ball
            if (_ballTime <= 0f)
                Machine.ChangeState<ChaseBallMainState>();
        }

        public override void Exit()
        {
            base.Exit();

            //stop listing to some player events
            Owner.OnTeamLostControl -= Instance_OnTeamLostControl;
        }

        private void Instance_OnTeamLostControl()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
        }

        public void SetSteeringTarget(float ballTime, Vector3 position)
        {
            _ballTime = ballTime;
            
            GetState<SteerToReceiveTarget>().SteeringTarget = position;
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
