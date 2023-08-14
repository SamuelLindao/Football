using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Attack.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.SubStates
{
    public class TakeKickOff : BState
    {
        bool executed;
        float waitTime = 1f;

        Action InstructPlayerToTakeKickOff;

        public TeamPlayer ControllingPlayer { get; set; }

        public override void Enter()
        {
            base.Enter();

            // set to unexecuted
            executed = false;

            // uncomment to follow actual procedure in taking kick-off
            //// register player to listening to take-kickoff action
            ControllingPlayer.Player.OnTakeKickOff += Instance_OnPlayerTakeKickOff;
            InstructPlayerToTakeKickOff += ControllingPlayer.Player.Invoke_OnInstructedToTakeKickOff;

        }

        public override void Execute()
        {
            base.Execute();

            // if not executed then run logic
            if (!executed)
            {
                // decrement time
                waitTime -= Time.deltaTime;

                if (waitTime <= 0)
                {
                    // set to executed
                    executed = true;

                    // trigger player to take kick-off
                    ActionUtility.Invoke_Action(InstructPlayerToTakeKickOff);

                    // comment out to stop skipping to game on
                    //Instance_OnPlayerTakeKickOff();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister player from listening to take-kickoff action
            ControllingPlayer.Player.OnTakeKickOff -= Instance_OnPlayerTakeKickOff;
            InstructPlayerToTakeKickOff -= ControllingPlayer.Player.Invoke_OnInstructedToTakeKickOff;

            // reset the home region of the player
            ControllingPlayer.Player.HomeRegion = ControllingPlayer.CurrentHomePosition;
        }

        public void Instance_OnPlayerTakeKickOff()
        {
            // trigger state change to attack
            SuperMachine.ChangeState<AttackMainState>();

            //simply raise that I have taken the kick-off
            ActionUtility.Invoke_Action(Owner.OnTakeKickOff);
        }

        public Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
