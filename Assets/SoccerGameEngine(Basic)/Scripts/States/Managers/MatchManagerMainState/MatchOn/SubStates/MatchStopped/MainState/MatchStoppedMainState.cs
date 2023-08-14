using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchStopped.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchStopped.MainState
{
    public class MatchStoppedMainState : BHState
    {
        public override void AddStates()
        {
            //add the states
            AddState<CheckNextMatchStatus>();
            AddState<BroadcastGoalScored>();
            AddState<BroadcastHalfTimeStatus>();
            AddState<HalfTime>();
            AddState<SwitchSides>();
            AddState<TriggerMatchOver>();

            //set the initial state
            SetInitialState<CheckNextMatchStatus>();
        }

        public override void Enter()
        {
            base.Enter();

            ////register the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff += Owner.TeamAway.OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff += Owner.TeamHome.OnMessagedToTakeKickOff;

            ////raise the match stopped event
            ActionUtility.Invoke_Action(Owner.OnStopMatch);
        }

        public override void Exit()
        {
            base.Exit();

            ////deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff -= Owner.TeamAway.OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff -= Owner.TeamHome.OnMessagedToTakeKickOff;
        }

        /// <summary>
        /// Returns the owner of this instance
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
