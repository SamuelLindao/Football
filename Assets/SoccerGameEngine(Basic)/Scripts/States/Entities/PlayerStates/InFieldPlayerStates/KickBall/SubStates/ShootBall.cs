using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class ShootBall : BState
    {
        public override void Enter()
        {
            base.Enter();

            //make a shot
            Owner.MakeShot(Ball.Instance.NormalizedPosition,
                (Vector3)Owner.KickTarget,
                Owner.KickPower,
                Owner.BallTime);

            //got to recover state
            Machine.ChangeState<RecoverFromKick>();
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
