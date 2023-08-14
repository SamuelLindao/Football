using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Base;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class CheckKickType : BState
    {
        public override void Enter()
        {
            base.Enter();

            //trigger thr right state transition
            if (Owner.KickType == KickType.Pass)
                Machine.ChangeState<PassBall>();
            else if (Owner.KickType == KickType.Shot)
                Machine.ChangeState<ShootBall>();
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
