using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState
{
    public class KickBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<CheckKickType>();
            AddState<PassBall>();
            AddState<RecoverFromKick>();
            AddState<RotateToFaceTarget>();
            AddState<ShootBall>();

            //set initial state
            SetInitialState<RotateToFaceTarget>();
        }

        public override void Enter()
        {
            base.Enter();

            // set the Ball's rigidbody
            Ball.Instance.Rigidbody.isKinematic = false;
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
