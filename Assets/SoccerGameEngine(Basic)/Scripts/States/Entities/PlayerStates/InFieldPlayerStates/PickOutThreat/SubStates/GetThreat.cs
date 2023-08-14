using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class GetThreat : BState
    {
        public override void Enter()
        {
            base.Enter();

            // fetch the threat from the pick out threat main state
            Player threat = ((PickOutThreatMainState)Machine).Threat;

            if (threat == null)
                Machine.ChangeState<SteerToHome>();
            else
                Machine.ChangeState<SteerToThreat>();
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
