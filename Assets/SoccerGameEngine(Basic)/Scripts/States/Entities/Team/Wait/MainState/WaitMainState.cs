using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Wait.MainState
{
    public class WaitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to some team events
            Owner.OnMessagedToTakeKickOff += Instance_OnMessagedSwitchToTakeKickOff;

            // raise the team wait event
            ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);

        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to some team events
            Owner.OnMessagedToTakeKickOff -= Instance_OnMessagedSwitchToTakeKickOff;
        }

        private void Instance_OnMessagedSwitchToTakeKickOff()
        {
            Machine.ChangeState<KickOffMainState>();
        }

        public Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
