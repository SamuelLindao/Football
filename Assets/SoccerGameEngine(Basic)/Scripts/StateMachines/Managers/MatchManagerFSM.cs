using Assets.RobustFSM.Mono;
using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.Init.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOver.MainState;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers
{
    public class MatchManagerFSM : MonoFSM<MatchManager>
    {
        public override void AddStates()
        {
            //add the states
            AddState<InitMainState>();
            AddState<MatchOnMainState>();
            AddState<MatchOverMainState>();

            //set the initial state
            SetInitialState<InitMainState>();
        }
    }
}
