using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOver.MainState;
using RobustFSM.Base;
using RobustFSM.Interfaces;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchStopped.SubStates
{
    public class TriggerMatchOver : BState
    {
        public override void Enter()
        {
            base.Enter();

            //got to Match Over
            SuperFSM.ChangeState<MatchOverMainState>();
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

    }
}
