using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.SubStates;
using RobustFSM.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState
{
    /// <summary>
    /// The player finds the support spot for the controlling player
    /// </summary>
    public class SupportAttackerMainState : BHState
    {
        public Vector3 PositionInConsideration { get; set; }
        public SupportSpot SupportSpot { get; set; }

        public override void AddStates()
        {
            base.AddStates();

            //add the states
            AddState<GetSupportSport>();
            AddState<SteerToHome>();
            AddState<SteerToSupportSpot>();
            AddState<WaitAtTarget>();

            //set the initial state
            SetInitialState<GetSupportSport>();
        }

        public override void Enter()
        {
            base.Enter();

            // set new speed
            Owner.RPGMovement.Speed *= 1.2f;

            //find support spot
            FindSupportSpot();

            //listen to variaus events
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToReceiveBall += Instance_OnInstructedToReceiveBall;
            Owner.OnTeamLostControl += Instance_OnTeamLostControl;
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //find support spot
            FindSupportSpot();
        }

        public override void Exit()
        {
            base.Exit();

            // restore player speed
            Owner.RPGMovement.Speed = Owner.ActualSpeed;

            //stop listening to variaus events
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToReceiveBall -= Instance_OnInstructedToReceiveBall;
            Owner.OnTeamLostControl -= Instance_OnTeamLostControl;

            if (SupportSpot != null)
                SupportSpot.SetIsNotPickedOut();
        }

        public void FindSupportSpot()
        {
            Player ballOwner = Ball.Instance.Owner;
            Vector3 target = ballOwner == null ? Ball.Instance.NormalizedPosition : ballOwner.Position;

            // set the distance to goal
            SupportSpot = Owner.PlayerSupportSpots
                .Where(p => p.IsPickedOut(Owner) == false
                    && Owner.IsPositionWithinPassRange(target, p.transform.position)
                    && Owner.IsPositionWithinWanderRadius(p.transform.position)
                    && Owner.IsTeamMemberWithinMinPassDistance(p.transform.position) == false
                    && Owner.IsPositionThreatened(p.transform.position) == false)
                .OrderBy(p => Vector3.Distance(p.transform.position, Owner.OppGoal.Position))
                .FirstOrDefault();
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        public void Instance_OnInstructedToReceiveBall(float time, Vector3 position)
        {
            //get the receive ball state and init the steering target
            Machine.GetState<ReceiveBallMainState>().SetSteeringTarget(time, position);
            Machine.ChangeState<ReceiveBallMainState>();
        }

        private void Instance_OnTeamLostControl()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
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
