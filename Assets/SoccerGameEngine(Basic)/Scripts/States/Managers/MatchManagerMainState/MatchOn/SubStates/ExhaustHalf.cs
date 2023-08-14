using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.Init.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOver.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchStopped.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using UnityEngine;
using static Assets.SoccerGameEngine_Basic_.Scripts.Managers.MatchManager;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates
{
    public class ExhaustHalf : BState
    {
        Coroutine _executingCoroutine;

        public override void Enter()
        {
            base.Enter();

            //raise the match play start event
            ActionUtility.Invoke_Action(Owner.OnMatchPlayStart);

            // register the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall += Owner.TeamAway.OnOppScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall += Owner.TeamHome.OnOppScoredAGoal;

            Owner.TeamAway.Goal.OnCollideWithBall += Owner.TeamHome.OnTeamScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall += Owner.TeamAway.OnTeamScoredAGoal;

            //listen to the OnTick event of the Time Manager
            Owner.TeamAway.Goal.OnCollideWithBall += Instance_OnGoalScored;
            Owner.TeamHome.Goal.OnCollideWithBall += Instance_OnGoalScored;
            TimeManager.Instance.OnTick += Instance_TimeManagerOnTick;

            //start the counter
            _executingCoroutine = Owner.StartCoroutine(TimeManager.Instance.TickTime());
        }

        public override void Exit()
        {
            base.Exit();

            //raise the match play stop event
            ActionUtility.Invoke_Action(Owner.OnMatchPlayStop);

            // deregister the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall -= Owner.TeamAway.OnOppScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall -= Owner.TeamHome.OnOppScoredAGoal;

            // deregister the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall -= Owner.TeamHome.OnTeamScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall -= Owner.TeamAway.OnTeamScoredAGoal;

            //stop listening to the OnTick event of the Time Manager
            Owner.TeamAway.Goal.OnCollideWithBall -= Instance_OnGoalScored;
            Owner.TeamHome.Goal.OnCollideWithBall -= Instance_OnGoalScored;
            TimeManager.Instance.OnTick -= Instance_TimeManagerOnTick;

            //stop the counter
            Owner.StopCoroutine(_executingCoroutine);
        }

        private void Instance_OnGoalScored()
        {
            //prepare the text
            string info = string.Format("TeamA {0}-{1} TeamH", Owner.TeamAway.Goals, Owner.TeamHome.Goals);

            //invoke the goal-scored event
            GoalScored temp = Owner.OnGoalScored;
            if (temp != null) temp.Invoke(info);

            // set the match status
            Owner.MatchStatus = MatchStatuses.GoalScored;

            // trigger state change
            Machine.ChangeState<MatchStoppedMainState>();
        }

        private void Instance_TimeManagerOnTick(int minutes, int seconds)
        {
            //raise the on tick event of the match manager
            Tick temp = Owner.OnTick;
            if (temp != null) temp.Invoke(Owner.CurrentHalf, minutes, seconds);

            //compare the next stop time and the current time
            //stop game if current time is now equal to the next stop time
            if (minutes >= Owner.NextStopTime)
            {
                // set the match status
                Owner.MatchStatus = MatchStatuses.HalfExhausted;

                // trigger state change
                Machine.ChangeState<MatchStoppedMainState>();
            }
        }

        /// <summary>
        /// Access the super state machine
        /// </summary>
        public IFSM SuperFSM
        {
            get
            {
                return (MatchManagerFSM)SuperMachine;
            }
        }

        /// <summary>
        /// Access the owner of the state machine
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
