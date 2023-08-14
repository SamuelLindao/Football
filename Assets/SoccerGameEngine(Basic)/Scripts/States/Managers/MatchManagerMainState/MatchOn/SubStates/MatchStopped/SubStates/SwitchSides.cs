using Assets.SoccerGameEngine_Basic_.Scripts.Managers;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Managers;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates
{
    public class SwitchSides : BState
    {
        public override void Enter()
        {
            base.Enter();

            //set-up the game scene
            Owner.CurrentHalf = 2;
            Owner.NextStopTime += Owner.NormalHalfLength;
            Owner.RootTeam.transform.Rotate(Owner.RootTeam.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

            // set the team's kickoff
            Owner.TeamAway.HasKickOff = !Owner.TeamAway.HasInitialKickOff;
            Owner.TeamHome.HasKickOff = !Owner.TeamHome.HasInitialKickOff;

            //got back to wait for kick-off state
            ((IState)Machine).Machine.ChangeState<BroadcastHalfStatus>();
        }

        /// <summary>
        /// Access the super state machine
        /// </summary>
        public IFSM SuperFSM
        {
            get
            {
                return (MatchManagerFSM)SuperMachine;
            }
        }

        /// <summary>
        /// Access the owner of the state machine
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
