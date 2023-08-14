using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeKickOff.MainState
{
    /// <summary>
    /// Player takes the kick-off and broadcasts that he has done so
    /// </summary>
    public class TakeKickOffMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            //get a player to pass to
            Player receiver = Owner.GetRandomTeamMemberInRadius(20f);

            //find the power to target
            float power = Owner.FindPower(Ball.Instance.NormalizedPosition,
                receiver.Position,
                Owner.BallPassArriveVelocity,
                Ball.Instance.Friction);

            //clamp the power
            power = Mathf.Clamp(power, 0f, Owner.ActualPower);

            float time = Owner.TimeToTarget(Ball.Instance.Position,
                receiver.Position,
                power,
                Ball.Instance.Friction);

            //make a normal pass to the player
            Owner.MakePass(Ball.Instance.NormalizedPosition,
                receiver.Position,
                receiver,
                power, 
                time);

            ////broadcast that I have taken kick-off
            ActionUtility.Invoke_Action(Owner.OnTakeKickOff);

            //go to home state
            Machine.ChangeState<GoToHomeMainState>();
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
    }
}
