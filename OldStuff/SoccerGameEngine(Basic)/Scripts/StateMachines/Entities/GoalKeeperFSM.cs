using Assets.RobustFSM.Mono;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Init.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;

namespace Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities
{
    public class GoalKeeperFSM : MonoFSM<Player>
    {
        public override void AddStates()
        {
            base.AddStates();

            //set the manual sexecute time
            SetUpdateFrequency(0.5f);

            // add states
            AddState<GoToHomeMainState>();
            AddState<InitMainState>();
            AddState<InterceptShotMainState>();
            AddState<TendGoalMainState>();
            AddState<WaitMainState>();

            // set initial states
            SetInitialState<InitMainState>();
        }
    }
}
