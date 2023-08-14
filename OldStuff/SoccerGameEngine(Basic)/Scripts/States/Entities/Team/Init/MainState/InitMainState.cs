using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.KickOff.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.Team.Init.MainState
{
    public class InitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();
            
            //listen to some ball events
            Ball.Instance.OnBallLaunched += Owner.Invoke_OnBallLaunched;

            //listen to some team events
            Owner.OnMessagedToTakeKickOff += Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnOppFinishedInit += Instance_OnOppFinishedInit;
            Owner.Opponent.OnGainPossession += Owner.Invoke_OnLostPossession;

            // create players
            CreateTeamPlayers();

            //init the team
            Init();

            //raise the on init event
            ActionUtility.Invoke_Action(Owner.OnInit);
        }

        public override void Exit()
        {
            base.Exit();

            // instruct every player to go to wait
            ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);

            //stop listening to some team events
            Owner.OnMessagedToTakeKickOff -= Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnOppFinishedInit -= Instance_OnOppFinishedInit;
        }

        private void Init()
        {
            // init players
            InitTeamMemberOppGoal();
            InitTeamMemberPlayerSupportSpots();
            InitTeamMemberTeamGoal();
            InitTeamPlayerTeamMembers();

            // register team members to team events
            RegisterTeamMemberToOnGainPossessionEvent();
            RegisterTeamMemberToOnLoosePossessionEvent();
            RegisterTeamMemberToPlayerOnInstructedToWaitEvent();

            // register team to team player events
            RegisterGoalKeeperToOnBallLaunchedEvent();
            //RegisterTeamToOppGoalEvents();
            RegisterTeamToTeamPlayerOnChaseBallEvent();
            RegisterTeamToTeamPlayerOnGainPossessionEvent();

            // set some player variables
            SetPlayerControl();
        }

        public void CreateTeamPlayers()
        {
            //create eleven team players
            for (int i = 0; i <= 10; i++)
            {
                //get the index
                Player player = Owner.RootPlayers.GetChild(i).GetComponent<Player>();
                Transform attackOffTransfrom = Owner.Formation.PositionsAttackingRoot.GetChild(i);
                Transform defendTransfrom = Owner.Formation.PositionsDefendingRoot.GetChild(i);
                Transform currentHomeTransfrom = Owner.Formation.PositionsCurrentHomeRoot.GetChild(i);
                Transform kickOffTransfrom = Owner.Formation.PositionsKickOffRoot.GetChild(i);

                //create the team player
                TeamPlayer newPlayer = new TeamPlayer(player,
                    Owner,
                    attackOffTransfrom,
                    defendTransfrom,
                    currentHomeTransfrom,
                    kickOffTransfrom,
                    Owner.DistancePassMax,
                    Owner.DistancePassMin,
                    Owner.DistanceShotValidMax,
                    Owner.DistanceTendGoal,
                    Owner.DistanceThreatMax,
                    Owner.DistanceThreatMin,
                    Owner.DistanceThreatTrack,
                    Owner.DistanceWonderMax,
                    Owner.VelocityPassArrive,
                    Owner.VelocityShotArrive,
                    Owner.Power,
                    Owner.Speed);

                //add it to list
                Owner.Players.Add(newPlayer);

                // enable player
                newPlayer.Player.gameObject.SetActive(true);
            }
        }

        public void InitTeamMemberOppGoal()
        {
            // init the opponent goal of each player
            Owner.Players.ForEach(tM => tM.Player.OppGoal = Owner.Opponent.Goal);
        }

        public void InitTeamMemberOppTeamMembers()
        {
            // get the full list of the team's members
            Owner.Players.ForEach(tM => tM.Player.OppositionMembers = Owner.Opponent.Players
            .Select(tMS => tMS.Player)
            .ToList());
        }

        public void InitTeamPlayerTeamMembers()
        {
            // get the full list of the team's members
            Owner.Players.ForEach(tM => tM.Player.TeamMembers = Owner.Players
            .Select(tMS => tMS.Player)
            .ToList());
        }

        public void InitTeamMemberPlayerSupportSpots()
        {
            //set the positions
            Owner.Players.ForEach(tM => tM.Player.PlayerSupportSpots = Owner.PlayerSupportSpots
            .GetComponentsInChildren<SupportSpot>()
            .ToList());
        }

        public void InitTeamMemberTeamGoal()
        {
            // init the opponent goal of each player
            Owner.Players.ForEach(tM => tM.Player.TeamGoal = Owner.Goal);
        }

        public void RegisterGoalKeeperToOnBallLaunchedEvent()
        {
            // get the goalkeeper
            TeamPlayer goalKeeper = Owner.Players
                .Where(tM => tM.Player.PlayerType == PlayerTypes.Goalkeeper)
                .FirstOrDefault();

            if (goalKeeper != null)
                Ball.Instance.OnBallShot += goalKeeper.Player.Invoke_OnBallLaunched;
        }

        public void RegisterTeamMemberToOnGainPossessionEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnGainPossession += tM.Player.Invoke_OnTeamGainedPossession);
        }

        public void RegisterTeamMemberToOnLoosePossessionEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnLostPossession += tM.Player.Invoke_OnTeamLostControl);
        }

        public void RegisterTeamToOppGoalEvents()
        {
            Owner.Opponent.Goal.OnCollideWithBall += Owner.OnTeamScoredAGoal;
        }

        public void RegisterTeamToTeamPlayerOnChaseBallEvent()
        {
            Owner.Players.ForEach(tM => tM.Player.OnChaseBall += Owner.Invoke_OnPlayerChaseBall);
        }

        public void RegisterTeamToTeamPlayerOnGainPossessionEvent()
        {
            Owner.Players.ForEach(tM => tM.Player.OnControlBall += Owner.Invoke_OnGainPossession);
        }

        public void RegisterTeamMemberToPlayerOnInstructedToWaitEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnInstructPlayersToWait += tM.Player.Invoke_OnInstructedToWait);
        }

        public void SetPlayerControl()
        {
            Owner.Players.ForEach(tM => tM.Player.IsUserControlled = Owner.IsUserControlled);
        }

        private void Instance_OnMessagedSwitchToTakeKickOff()
        {
            Machine.ChangeState<KickOffMainState>();
        }

        private void Instance_OnOppFinishedInit()
        {
            InitTeamMemberOppTeamMembers();
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
