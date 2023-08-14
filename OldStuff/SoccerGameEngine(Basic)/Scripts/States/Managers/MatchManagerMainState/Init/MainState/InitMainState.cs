using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.Init.SubStates;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.Init.MainState
{
    public class InitMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<Initialize>();
            AddState<WaitForMatchOnInstruction>();

            //set initial state
            SetInitialState<Initialize>();
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
