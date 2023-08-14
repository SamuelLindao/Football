using Assets.RobustFSM.Mono;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Attack.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Defend.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Init.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Wait.MainState;

namespace Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities
{
    public class TeamFSM : MonoFSM<Team>
    {
        public override void AddStates()
        {
            //set the update frequency
            SetUpdateFrequency(0.25f);

            //add the states
            AddState<AttackMainState>();
            AddState<InitMainState>();
            AddState<DefendMainState>();
            AddState<KickOffMainState>();
            AddState<WaitMainState>();

            //set the initial state
            SetInitialState<InitMainState>();
        }
    }
}
