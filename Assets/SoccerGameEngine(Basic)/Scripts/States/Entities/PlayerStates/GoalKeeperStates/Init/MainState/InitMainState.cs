using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Init.MainState
{
    public class InitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // disable the user controlled icon
            Owner.IconUserControlled.SetActive(false);

            //init
            Owner.Init();

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
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
