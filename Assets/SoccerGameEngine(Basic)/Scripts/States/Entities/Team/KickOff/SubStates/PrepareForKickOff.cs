using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.SubStates
{
    public class PrepareForKickOff : BState
    {
        public override void Enter()
        {
            base.Enter();

            // set player current psotion to kick-off position
            SetPlayerCurrentHomePositionToKickOffPosition();

            //place every player on the kick off position
            PlaceEveryPlayerAtKickOffPosition();

            //go to the next state
            if (Owner.HasKickOff)
            {
                //comment to follow kickoff procedure
                PlaceKickOffTakerAtTakeKickOffPosition();
                Machine.ChangeState<TakeKickOff>();
            }
            else
                Machine.ChangeState<WaitForKickOff>();
        }

        void PlaceEveryPlayerAtKickOffPosition()
        {
            Owner.Players.ForEach(tM =>
            {
                tM.Player.Position = tM.CurrentHomePosition.position;
                tM.Player.Rotation = tM.KickOffHomePosition.rotation;
            });
        }

        void PlaceKickOffTakerAtTakeKickOffPosition()
        {
            // get the last player
            TeamPlayer teamPlayer = Owner.Players.Last();

            //get the take kick of state and set the controlling player
            Machine.GetState<TakeKickOff>().ControllingPlayer = teamPlayer;

            //place player a kick off position
            teamPlayer.CurrentHomePosition.position = Pitch.Instance.CenterSpot.position + (Owner.Goal.transform.forward * (teamPlayer.Player.BallControlDistance + teamPlayer.Player.Radius));
            teamPlayer.Player.transform.position = teamPlayer.CurrentHomePosition.position;
            Owner.KickOffRefDirection.position = teamPlayer.Player.transform.position;
            teamPlayer.Player.HomeRegion = Owner.KickOffRefDirection;

            // rotate the player to face the ball
            teamPlayer.Player.transform.rotation = Owner.KickOffRefDirection.rotation;
        }

        void SetPlayerCurrentHomePositionToKickOffPosition()
        {
            Owner.Players.ForEach(tM => tM.CurrentHomePosition.transform.position = tM.KickOffHomePosition.transform.position);
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
