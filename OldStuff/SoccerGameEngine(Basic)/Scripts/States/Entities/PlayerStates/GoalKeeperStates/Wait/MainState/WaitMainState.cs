using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState
{
    public class WaitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            //listen to variaus events
            Owner.OnInstructedToGoToHome += Instance_OnInstructedToGoToHome;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to variaus events
            Owner.OnInstructedToGoToHome -= Instance_OnInstructedToGoToHome;
        }

        public void Instance_OnInstructedToGoToHome()
        {
            Machine.ChangeState<TendGoalMainState>();
        }

        public Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
