using System;
using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Defend.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Wait.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Attack.MainState
{
    /// <summary>
    /// The team finds positions higher up the pitch and instructs it's players to
    /// move up the pitch into goal scoring positions
    /// </summary>
    public class AttackMainState : BState
    {
        float _lengthPitch = 90;

        public override void Enter()
        {
            base.Enter();

            // enable the support spots root
            Owner.PlayerSupportSpots.gameObject.SetActive(true);

            //listen to some team events
            Owner.OnLostPossession += Instance_OnLoosePossession;
            Owner.OnMessagedToStop += Instance_OnMessagedToStop;

            // init the players home positions
            Owner.Players.ForEach(tM => ActionUtility.Invoke_Action(tM.Player.OnInstructedToGoToHome));
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //loop through each player and update it's position
            foreach(TeamPlayer teamPlayer in Owner.Players)
            {
                //find the percentage to move the player upfield
                Vector3 ballGoalLocalPosition = Owner.Goal.transform.InverseTransformPoint(Ball.Instance.transform.position);
                float playerMovePercentage = Mathf.Clamp01((ballGoalLocalPosition.z / _lengthPitch) + 0.25f);

                //move the home position a similar percentage up the field
                Vector3 currentPlayerHomePosition = Vector3.Lerp(teamPlayer.DefendingHomePosition.transform.position,
                    teamPlayer.AttackingHomePosition.position,
                    playerMovePercentage);

                //update the current player home position position
                if(Vector3.Distance(currentPlayerHomePosition, teamPlayer.CurrentHomePosition.position) >= 2)
                    teamPlayer.CurrentHomePosition.position = currentPlayerHomePosition;
            }
        }

        public override void Exit()
        {
            base.Exit();

            // enable the support spots root
            Owner.PlayerSupportSpots.gameObject.SetActive(false);

            //stop listening to some team events
            Owner.OnLostPossession -= Instance_OnLoosePossession;
            Owner.OnMessagedToStop -= Instance_OnMessagedToStop;
        }

        private void Instance_OnLoosePossession()
        {
            Machine.ChangeState<DefendMainState>();
        }

        private void Instance_OnMessagedToStop()
        {
            SuperMachine.ChangeState<WaitMainState>();
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
