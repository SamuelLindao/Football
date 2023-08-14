using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates
{
    /// <summary>
    /// Waits for the kick-off event to be raised by either of the teams
    /// If the kickoff event is raised then it triggers a state change to
    /// the exhaust first half state
    /// </summary>
    public class WaitForKickOffToComplete : BState
    {
        bool hasInvokedKickOffEvent;
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set hasn't invoked kick off event
            hasInvokedKickOffEvent = false;
            waitTime = 1f;

            // put the ball at the kick-off position
            Ball.Instance.Trap();
            Ball.Instance.Position = Owner.TransformCentreSpot.position;

            //register the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff += Owner.TeamAway.Invoke_OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff += Owner.TeamHome.Invoke_OnMessagedToTakeKickOff;

            //listen to team OnTakeKickOff events
            Owner.TeamAway.OnTakeKickOff += Instance_OnTeamTakeKickOff;
            Owner.TeamHome.OnTakeKickOff += Instance_OnTeamTakeKickOff;
        }

        public override void Execute()
        {
            base.Execute();

            if(hasInvokedKickOffEvent == false)
            {
                waitTime -= Time.deltaTime;
                if(waitTime < 0)
                    ActionUtility.Invoke_Action(Owner.OnBroadcastTakeKickOff);
            }
        }

        public override void Exit()
        {
            base.Exit();

            //deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff -= Owner.TeamAway.Invoke_OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff -= Owner.TeamHome.Invoke_OnMessagedToTakeKickOff;

            //stop listening to team OnInit events
            Owner.TeamAway.OnTakeKickOff -= Instance_OnTeamTakeKickOff;
            Owner.TeamHome.OnTakeKickOff -= Instance_OnTeamTakeKickOff;
        }

        private void Instance_OnTeamTakeKickOff()
        {
            Machine.ChangeState<ExhaustHalf>();
        }

        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
