using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.SubStates
{
    public class ChooseChaseType : BState
    {
        public override void Enter()
        {
            base.Enter();

            if (Owner.IsUserControlled)
                Machine.ChangeState<ManualChase>();
            else
                Machine.ChangeState<AutomaticChase>();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
