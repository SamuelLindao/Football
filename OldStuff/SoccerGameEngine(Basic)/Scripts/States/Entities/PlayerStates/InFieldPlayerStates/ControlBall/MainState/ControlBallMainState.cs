using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using RobustFSM.Base;
using static Assets.SoccerGameEngine_Basic_.Scripts.Entities.Player;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState
{
    public class ControlBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<AutomaticControl>();
            AddState<ChooseControlType>();
            AddState<ManualControl>();

            //set inital state
            SetInitialState<ChooseControlType>();
        }

        public override void Enter()
        {
            base.Enter();

            // set new speed
            Owner.RPGMovement.Speed *= 0.95f;

            //listen to game events
            Owner.OnTackled += Instance_OnTackled;

            //set the ball to is kinematic
            Ball.Instance.Owner = Owner;
            Ball.Instance.Rigidbody.isKinematic = true;

            // raise event that I'm controlling the ball
            ControlBallDel temp = Owner.OnControlBall;
            if (temp != null)
                temp.Invoke(Owner);
        }

        public override void Execute()
        {
            base.Execute();

            //place ball infront of me
            Owner.PlaceBallInfronOfMe();
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
