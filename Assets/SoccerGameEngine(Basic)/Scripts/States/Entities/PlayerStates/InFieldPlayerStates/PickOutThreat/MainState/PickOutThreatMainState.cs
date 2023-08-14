using System.Collections.Generic;
using System.Linq;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState
{
    public class PickOutThreatMainState : BHState
    {
        public Player Threat { get; set; }

        public override void AddStates()
        {
            base.AddStates();

            AddState<GetThreat>();
            AddState<SteerToHome>();
            AddState<SteerToThreat>();
            AddState<WaitAtTarget>();

            SetInitialState<GetThreat>();
        }

        public override void Enter()
        {
            // lets find the threat
            FindThreat();

            // register to on became closest player to ball
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnTeamGainedPossession += Instance_OnTeamGainedPossession;

            base.Enter();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // find threat
            FindThreat();
        }

        public override void Exit()
        {
            base.Exit();

            // register to on became closest player to ball
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnTeamGainedPossession -= Instance_OnTeamGainedPossession;
        }

        public void FindThreat()
        {
            // find a player within my wander raduis who is not picked out
            // and who is very close to my team goal
            Threat = Owner.OppositionMembers
                .Where(oM => oM.IsPickedOut(Owner) == false
                && Owner.IsPositionWithinWanderRadius(oM.Position) == true)
                .OrderBy(oM => Vector3.Distance(oM.Position, Owner.TeamGoal.Position))
                .FirstOrDefault();
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        private void Instance_OnTeamGainedPossession()
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
