using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using RobustFSM.Interfaces;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchStopped.SubStates
{
    public class CheckNextMatchStatus : BState
    {
        public override void Enter()
        {
            base.Enter();

            // run the logic depending on match status
            if (Owner.MatchStatus == MatchStatuses.GoalScored)
            {
                Machine.ChangeState<BroadcastGoalScored>();
            }
            else if (Owner.MatchStatus == MatchStatuses.HalfExhausted)
            {
                //if it's the first half then we have to switch sides
                if (Owner.CurrentHalf == 1)
                    Machine.ChangeState<BroadcastHalfTimeStatus>();
                else if (Owner.CurrentHalf == 2)
                    Machine.ChangeState<TriggerMatchOver>();
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
