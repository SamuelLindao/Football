using Assets.SimpleSteering.Scripts.Movement;
using Assets.SoccerGameEngine_Basic_.Scripts.StateMachines.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities.Enums;
using RobustFSM.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Entities
{
    //[RequireComponent(typeof(InFieldPlayerFSM))]
    [RequireComponent(typeof(RPGMovement))]
    [RequireComponent(typeof(SupportSpot))]
    public class Player : MonoBehaviour
    {
        [Header("Control Variables")]

        [SerializeField]
        bool _isUserControlled;

        [SerializeField]
        float _ballControlDistance = 0.5f;

        [SerializeField]
        float _maxWanderDistance = 10f;

        [SerializeField]
        float _distancePassMax = 15f;

        [SerializeField]
        float _distancePassMin = 5f;

        [SerializeField]
        float _distanceShotMaxValid = 20f;

        [SerializeField]
        float _distanceThreatMax = 0.5f;

        [SerializeField]
        float _distanceThreatMin = 1f;

        [SerializeField]
        float _ballPassArriveVelocity = 5f;

        [SerializeField]
        float _ballShotArriveVelocity = 10f;

        [SerializeField]
        float _threatTrackDistance = 1f;

        [SerializeField]
        float _tendGoalDistance = 1f;

        [SerializeField]
        Goal _oppGoal;

        [SerializeField]
        Goal _teamGoal;

        [Header("Player Attributes")]

        [SerializeField]
        [Range(0.1f, 1f)]
        float _accuracy;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _goalKeeping = 0.8f;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _power;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _speed;

        [SerializeField]
        [Range(0.1f, 5f)]
        float _tendGoalSpeed = 4f;

        [SerializeField]
        Transform _homeRegion;

        [SerializeField]
        List<Player> _oppositionMembers;

        [SerializeField]
        List<Player> _teamMembers;

        [SerializeField]
        List<SupportSpot> _pitchPoints;

        [SerializeField]
        float _ballContrallableDistance = 1f;

        [SerializeField]
        float _ballTacklableDistance = 3f;

        [SerializeField]
        PlayerTypes _playerType;

        [SerializeField]
        GameObject _iconUserControlled;

        float _radius;

        float _rotationSpeed = 6f;

        Player _prevPassReceiver;
        RPGMovement _rpgMovement;

        public Action OnBecameTheClosestPlayerToBall;
        public Action OnInstructedToGoToHome;
        public Action OnInstructedToTakeKickOff;
        public Action OnInstructedToWait;
        public Action OnIsNoLongerClosestPlayerToBall;
        public Action OnTackled;
        public Action OnTakeKickOff;
        public Action OnTeamGainedPossession;
        public Action OnTeamLostControl;

        public delegate void BallLaunched(float flightPower, float velocity, Vector3 initial, Vector3 target);
        public delegate void ChaseBallDel(Player player);
        public delegate void ControlBallDel(Player player);
        public delegate void InstructedToReceiveBall(float time, Vector3 position);

        public BallLaunched OnShotTaken;
        public ChaseBallDel OnChaseBall;
        public ControlBallDel OnControlBall;
        public InstructedToReceiveBall OnInstructedToReceiveBall;

        [SerializeField]
        public bool IsTeamInControl;// { get; set; }

        public bool IsUserControlled { get => _isUserControlled; set => _isUserControlled = value; }

        public float ActualAccuracy { get; set; }

        public float ActualPower { get; set; }

        public float ActualSpeed { get; set; }

        public float BallTime { get; set; }

        public float KickPower { get; set; }

        public float KickTime { get; set; }

        public Vector3? KickTarget { get; set; }

        public IFSM GoalKeeperFSM { get; set; }

        public IFSM InFieldPlayerFSM { get; set; }

        public KickType KickType { get; set; }

        public Player PassReceiver;// { get; set; }

        public SupportSpot SupportSpot { get; set; }

        public Transform HomeRegion { get => _homeRegion; set => _homeRegion = value; }
        public List<Player> OppositionMembers { get => _oppositionMembers; set => _oppositionMembers = value; }
        public List<Player> TeamMembers { get => _teamMembers; set => _teamMembers = value; }

        private void Awake()
        {
            //get some components
            GoalKeeperFSM = GetComponent<GoalKeeperFSM>();
            InFieldPlayerFSM = GetComponent<InFieldPlayerFSM>();
            RPGMovement = GetComponent<RPGMovement>();
            SupportSpot = GetComponent<SupportSpot>();

            // cache some component data
            _radius = GetComponent<CapsuleCollider>().radius;

            //initialize some data
            _accuracy = Mathf.Clamp(Random.value, 0.6f, 0.9f);
            _goalKeeping = Mathf.Clamp(Random.value, 0.6f, 0.9f);
            _power = Mathf.Clamp(Random.value, 0.6f, 0.9f);
            _speed = Mathf.Clamp(Random.value, 0.8f, 0.9f);
        }

        public bool CanBallReachPoint(Vector3 position, float power, out float time)
        {
            //calculate the time
            time = TimeToTarget(Ball.Instance.NormalizedPosition,
                       position,
                       power,
                       Ball.Instance.Friction);

            //return result
            return time > 0;
        }

        /// <summary>
        /// Checks whether a player can pass
        /// </summary>
        /// <returns></returns>
        /// ToDo::Implement logic to cache players to message so that they can intercept the pass
        public bool CanPass(bool considerPassSafety = true)
        {
            //set the pass target
            bool passToPlayerClosestToMe = false;// Random.value <= 0.1f;

            //set the pass target
            KickTarget = null;

            //loop through each team player and find a pass for each
            foreach (Player player in TeamMembers)
            {
                // can't pass to myself
                bool isPlayerMe = player == this;
                if (isPlayerMe)
                    continue;

                // we don't want to pass to the last receiver
                bool isPlayePrevPassReceiver = player == _prevPassReceiver;
                if (isPlayePrevPassReceiver)
                    continue;

                // can't pass to the goalie
                bool isPlayerGoalKeeper = player.PlayerType == PlayerTypes.Goalkeeper;
                if (isPlayerGoalKeeper)
                    continue;

                // check if player can pass
                CanPass(player.Position, considerPassSafety, passToPlayerClosestToMe, player);
            }

            //return result
            //Player can pass if there is a pass target
            return KickTarget != null;
        }

        public bool CanPass(Vector3 position, bool considerPassSafety = true, bool considerPlayerClosestToMe = false, Player player = null)
        {
            //get the possible pass options
            List<Vector3> passOptions = GetPassPositionOptions(position);

            //loop through each option and search if it is possible to 
            //pass to it. Consider positions higher up the pitch
            foreach (Vector3 passOption in passOptions)
            {
                // check if position is within pass range
                bool isPositionWithinPassRange = IsPositionWithinPassRange(passOption);

                // we consider a target which is out of our min pass distance
                if (isPositionWithinPassRange == true)
                {
                    //find power to kick ball
                    float power = FindPower(Ball.Instance.NormalizedPosition,
                        passOption,
                        BallPassArriveVelocity,
                        Ball.Instance.Friction);

                    //clamp the power to the player's max power
                    power = Mathf.Clamp(power, 0f, this.ActualPower);

                    //find if ball can reach point
                    float ballTimeToTarget = 0f;
                    bool canBallReachTarget = CanBallReachPoint(passOption,
                            power,
                            out ballTimeToTarget);

                    //return false if the time is less than zero
                    //that means the ball can't reach it's target
                    if (canBallReachTarget == false)
                        return false;

                    // get time of player to point
                    float timeOfReceiverToTarget = TimeToTarget(position,
                        passOption,
                        ActualSpeed);

                    // pass is not safe if receiver can't reach target before the ball
                    if (timeOfReceiverToTarget > ballTimeToTarget)
                        return false;

                    // check if pass is safe from all opponents
                    bool isPassSafeFromAllOpponents = false;
                    if (considerPassSafety)
                    {
                        // check pass safety
                        isPassSafeFromAllOpponents = IsPassSafeFromAllOpponents(Ball.Instance.NormalizedPosition,
                            position,
                            passOption,
                            power,
                            ballTimeToTarget);
                    }

                    //if pass is safe from all opponents then cache it
                    if (considerPassSafety == false ||
                        (considerPassSafety == true && isPassSafeFromAllOpponents == true))
                    {
                        if (considerPlayerClosestToMe)
                        {
                            //set the pass-target to be the initial position
                            //check if pass is closer to goal and save it
                            if (KickTarget == null
                                || IsPositionCloserThanPosition(Position,
                                                        passOption,
                                                        (Vector3)KickTarget))
                            {
                                BallTime = ballTimeToTarget;
                                KickPower = power;
                                KickTarget = passOption;
                                PassReceiver = player;
                            }
                        }
                        else
                        {
                            //set the pass-target to be the initial position
                            //check if pass is closer to goal and save it
                            if (KickTarget == null
                                || IsPositionCloserThanPosition(OppGoal.transform.position,
                                                        passOption,
                                                        (Vector3)KickTarget))
                            {
                                BallTime = ballTimeToTarget;
                                KickPower = power;
                                KickTarget = passOption;
                                PassReceiver = player;
                            }
                        }
                    }
                }
            }

            //return result
            //Player can pass if there is a pass target
            return KickTarget != null;
        }

        public bool CanScore(bool considerGoalDistance = true, bool considerShotSafety = true)
        {
            // shoot if distance to goal is valid
            if (considerGoalDistance)
            {
                float distanceToGoal = Vector3.Distance(OppGoal.Position, Position);
                if (distanceToGoal > _distanceShotMaxValid)
                    return false;
            }

            //define some positions to be local to the goal
            //get the shot reference point. It should be a point some distance behinde the 
            //goal-line/goal
            Vector3 refShotTarget = _oppGoal.ShotTargetReferencePoint;

            //number of tries to find a shot
            float numOfTries = Random.Range(1, 6);

            //loop through and find a valid shot
            for (int i = 0; i < numOfTries; i++)
            {
                //find a random target
                Vector3 randomGoalTarget = FindRandomShot();

                float power = FindPower(Ball.Instance.NormalizedPosition,
                    randomGoalTarget,
                    _ballShotArriveVelocity);

                //clamp the power
                power = Mathf.Clamp(power, 0f, ActualPower);
              
                //check if ball can reach the target
                float time = 0f;
                bool canBallReachPoint = CanBallReachPoint(randomGoalTarget,
                    power,
                    out time);

                // if ball can't reach target then return false
                if (time < 0)
                    return false;

                //check if shot to target is possible
                bool isShotPossible = false;
                if (considerShotSafety)
                {
                    isShotPossible = IsPassSafeFromAllOpponents(Ball.Instance.NormalizedPosition,
                        randomGoalTarget,
                        randomGoalTarget,
                        power,
                        time);
                }

                //if shot is possible set the data
                if(isShotPossible == false && considerShotSafety == false
                    || isShotPossible && considerShotSafety)
                {
                    //set the data
                    KickPower = power;
                    KickTarget = randomGoalTarget;
                    KickTime = time;

                    //return result
                    return true;
                }
            }

            return false;
        }

        public bool CanPlayerReachTargetBeforePlayer(Vector3 target, Player player001, Player player002)
        {
            return IsPositionCloserThanPosition(target,
                player001.Position,
                player002.Position);
        }

        public bool CanPassInDirection(Vector3 direction)
        {
            //set the pass target
            bool passToPlayerClosestToMe = Random.value <= 0.75f;

            //set the pass target
            KickTarget = null;

            //loop through each team player and find a pass for each
            foreach (Player player in TeamMembers)
            {
                // find a pass to a player who isn't me
                // who isn't a goal keeper
                // who is in this direction
                if (player != this
                    && player.PlayerType == PlayerTypes.InFieldPlayer
                    && IsPositionInDirection(direction, player.Position, 22.5f))
                {
                    CanPass(player.Position, true, passToPlayerClosestToMe, player);
                }
            }

            // if there is no pass simply find a team member in this direction
            if (KickTarget == null)
            {
                //loop through each team player and find a pass for each
                foreach (Player player in TeamMembers)
                {
                    // find a pass to a player who isn't me
                    // who isn't a goal keeper
                    // who is in this direction
                    if (player != this
                        && player.PlayerType == PlayerTypes.InFieldPlayer
                        && IsPositionInDirection(direction, player.Position, 22.5f))
                    {
                        CanPass(player.Position, false, passToPlayerClosestToMe, player);
                    }
                }
            }

            // if still there is no player simply find a player to pass to

            //return result
            //Player can pass if there is a pass target
            return KickTarget != null;

        }

        public bool IsPassSafeFromAllOpponents(Vector3 initialPosition, Vector3 receiverPosition, Vector3 target, float initialBallVelocity, float time)
        {
            //look for a player threatening the pass
            foreach(Player player in OppositionMembers)
            {
                bool isPassSafeFromOpponent = IsPassSafeFromOpponent(initialPosition,
                    target,
                    player.Position,
                    receiverPosition,
                    initialBallVelocity,
                    time);

                //return false if the pass is not safe
                if (isPassSafeFromOpponent == false)
                    return false;
            }

            //return result
            return true;
        }

        public bool IsPassSafeFromOpponent(Vector3 initialPosition, Vector3 target, Vector3 oppPosition, Vector3 receiverPosition, float initialBallVelocity, float timeOfBall)
        {
            #region Consider some logic that might threaten the pass

            //we might not want to pass to a player who is highly threatened(marked)
            if (IsPositionAHighThreat(receiverPosition, oppPosition))
                return false;

            //return false if opposition is closer to target than reciever
            if (IsPositionCloserThanPosition(target, oppPosition, receiverPosition))
                return false;

            //If oppossition is not between the passing lane then he is behind the passer
            //receiver and he can't intercept the ball
            if (IsPositionBetweenTwoPoints(initialPosition, receiverPosition, oppPosition) == false)
                return true;

            #endregion

            #region find if opponent can intercept ball

            //check if pass to position can be intercepted
            Vector3 orthogonalPoint = GetPointOrthogonalToLine(initialPosition,
                target,
                oppPosition);

            //get time of ball to point
            float timeOfBallToOrthogonalPoint = 0f;
            CanBallReachPoint(orthogonalPoint, initialBallVelocity, out timeOfBallToOrthogonalPoint);

            //get time of opponent to target
            float timeOfOpponentToTarget = TimeToTarget(oppPosition,
            orthogonalPoint,
            ActualSpeed);

            //ball is safe if it can reach that point before the opponent
            bool canBallReachOrthogonalPointBeforeOpp = timeOfBallToOrthogonalPoint < timeOfOpponentToTarget;

            if (canBallReachOrthogonalPointBeforeOpp == true)
                return true;
            else
                return false;
            // return true;
            #endregion
        }

        /// <summary>
        /// Checks whether this instance is picked out or not
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPickedOut(Player player)
        {
            return SupportSpot.IsPickedOut(player);
        }

        public bool IsPositionBetweenTwoPoints(Vector3 A, Vector3 B, Vector3 point)
        {
            //find some direction vectors
            Vector3 fromAToPoint = point - A;
            Vector3 fromBToPoint = point - B;
            Vector3 fromBToA = A - B;
            Vector3 fromAToB = -fromBToA;

            //check if point is inbetween and return result
            return Vector3.Dot(fromAToB.normalized, fromAToPoint.normalized) > 0
                && Vector3.Dot(fromBToA.normalized, fromBToPoint.normalized) > 0;
        }

        /// <summary>
        /// Checks whether the first position is closer to target than the second position
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position001"></param>
        /// <param name="position002"></param>
        /// <returns></returns>
        public bool IsPositionCloserThanPosition(Vector3 target, Vector3 position001, Vector3 position002)
        {
            return Vector3.Distance(position001, target) < Vector3.Distance(position002, target);
        }

        public bool IsPositionInDirection(Vector3 forward, Vector3 position, float angle)
        {
            // find direction to target
            Vector3 directionToTarget = position - Position;

            // find angle between forward and direction to target
            float angleBetweenDirections = Vector3.Angle(forward.normalized, directionToTarget.normalized);

            // return result
            return angleBetweenDirections <= angle / 2;
        }

        public bool IsPositionThreatened(Vector3 position)
        {
            //search for threatening player
            foreach (Player player in OppositionMembers)
            {
                if (IsPositionWithinHighThreatDistance(position, player.Position))
                    return true;
            }

            //return false
            return false;

        }

        public bool IsPositionWithinMinPassDistance(Vector3 position)
        {
            return IsWithinDistance(Position,
                position,
                _distancePassMin);
        }

        public bool IsPositionWithinMinPassDistance(Vector3 center, Vector3 position)
        {
            return IsWithinDistance(center,
                position,
                _distancePassMin);
        }

        public bool IsPositionWithinWanderRadius(Vector3 position)
        {
            return IsWithinDistance(_homeRegion.position,
                position,
                _maxWanderDistance);
        }

        /// <summary>
        /// Finds the power
        /// </summary>
        /// <param name="from">initial position</param>
        /// <param name="to">target</param>
        /// <param name="arriveVelocity">required velocity on arrival to target</param>
        /// <param name="friction">force acting against motion</param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float arriveVelocity, float friction)
        {
            // v^2 = u^2 + 2as => u^2 = v^2 - 2as => u = root(v^2 - 2as)

            //calculate some values
            float vSquared = Mathf.Pow(arriveVelocity, 2f);
            float twoAS = 2 * friction * Vector3.Distance(from, to);
            float uSquared = vSquared - twoAS;

            //find result
            float result = Mathf.Sqrt(uSquared);

            //return result
            return result;
        }

        public float TimeToTarget(Vector3 initial, Vector3 target, float velocityInitial)
        {
            //use S = D/T => T = D/S
            return Vector3.Distance(initial, target) / velocityInitial;
        }

        /// <summary>
        /// Calculates the time it will take to reach the target
        /// </summary>
        /// <param name="inital">start position</param>
        /// <param name="target">final position</param>
        /// <param name="initialVelocity">initial velocity</param>
        /// <param name="acceleration">force acting aginst motion</param>
        /// <returns></returns>
        public float TimeToTarget(Vector3 initial, Vector3 target, float velocityInitial, float acceleration)
        {
            //using  v^2 = u^2 + 2as 
            float distance = Vector3.Distance(initial, target);
            float uSquared = Mathf.Pow(velocityInitial, 2f);
            float v_squared = uSquared + (2 * acceleration * distance);

            //if v_squared is less thaSn or equal to zero it means we can't reach the target
            if (v_squared <= 0)
                return -1.0f;

            //find the final velocity
            float v = Mathf.Sqrt(v_squared);

            //find time to travel 
            return TimeToTravel(velocityInitial, v, acceleration);
        }

        public float TimeToTravel(float initialVelocity, float finalVelocity, float acceleration)
        {
            // t = v-u
            //     ---
            //      a
            float time = (finalVelocity - initialVelocity) / acceleration;

            //return result
            return time;
        }

        /// <summary>
        /// Finds a random target on the goal
        /// </summary>
        /// <returns></returns>
        public Vector3 FindRandomShot()
        {
            //define some positions to be local to the goal
            //get the shot reference point. It should be a point some distance behinde the 
            //goal-line/goal
            Vector3 refShotTarget = _oppGoal.transform.InverseTransformPoint(_oppGoal.ShotTargetReferencePoint);

            //find an x-position within the goal mouth
            float randomXPosition = Random.Range(_oppGoal.BottomLeftRelativePosition.x,
                _oppGoal.BottomRightRelativePosition.x);

            //set result
            Vector3 goalLocalTarget = new Vector3(randomXPosition, refShotTarget.y, refShotTarget.z);
            Vector3 goalGlobalTarget = _oppGoal.transform.TransformPoint(goalLocalTarget);

            //return result
            return goalGlobalTarget;
        }

        /// <summary>
        /// Calculates a point on line a-b that is at right angle to a point
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 GetPointOrthogonalToLine(Vector3 from, Vector3 to, Vector3 point)
        {
            //this is the normal
            Vector3 fromTo = to - from;

            //this is the vector/direction
            Vector3 fromPoint = point - from;

            //find projection
            Vector3 projection = Vector3.Project(fromPoint, fromTo);

            //find point on normal
            return projection + from;
        }

        /// <summary>
        /// Gets the options to pass the ball
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<Vector3> GetPassPositionOptions(Vector3 position)
        {
            //create a list to hold the results
            List<Vector3> result = new List<Vector3>();

            //the first position is the current position
            result.Add(position);

            //set some data
            float incrementAngle = 45;
            float iterations = 360 / incrementAngle;

            //find some positions around the player
            for (int i = 0; i < iterations; i++)
            {
                //get the direction
                float angle = incrementAngle * i;

                //rotate the direction
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

                //get point
                Vector3 point = position + direction * _distancePassMin * 0.5f;

                //add to list
                result.Add(point);
            }

            //return results
            return result;
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        public void Init()
        {
            ActualPower *= _power;
            ActualSpeed *= _speed;

            //Init the RPGMovement
            RPGMovement.Init(ActualSpeed, 
                ActualSpeed, 
                _rotationSpeed * _speed, 
                ActualSpeed);
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="power"></param>
        /// <param name="speed"></param>
        public void Init(float distancePassMax,
           float distancePassMin,
           float distanceShotValidMax,
           float distanceTendGoal,
           float distanceThreatMax,
           float distanceThreatMin,
           float distanceThreatTrack,
           float distanceWonderMax,
           float velocityPassArrive,
           float velocityShotArrive,
           float power,
           float speed)
        {
            _distancePassMax = distancePassMax;
            _distancePassMin = distancePassMin;
            _distanceShotMaxValid = distanceShotValidMax;
            _tendGoalDistance = distanceTendGoal;
            _threatTrackDistance = distanceThreatTrack;
            _maxWanderDistance = distanceWonderMax;
            _ballPassArriveVelocity = velocityPassArrive;
            _ballShotArriveVelocity = velocityShotArrive;
            _distanceThreatMax = distanceThreatMax;
            _distanceThreatMin = distanceThreatMin;
            ActualSpeed = speed;
            ActualPower = power;
        }

        /// <summary>
        /// Checks whether the player is at target
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsAtTarget(Vector3 position)
        {
            return IsWithinDistance(Position, position, 0.25f);
        }

        public bool IsBallWithinControlableDistance()
        {
            return IsWithinDistance(Position, Ball.Instance.NormalizedPosition, _ballContrallableDistance + Radius);
        }

        public bool IsBallWithinTacklableDistance()
        {
            return IsWithinDistance(Position, Ball.Instance.NormalizedPosition, _ballTacklableDistance);
        }

        public bool IsInfrontOfPlayer(Vector3 position)
        {
            // find the direction to target
            Vector3 direction = position - Position;

            float dot = Vector3.Dot(direction.normalized, transform.forward);

            return dot > 0;
            ////transfrom point to local
            //Vector3 localDirection = transform.InverseTransformDirection(direction);
 
            ////return result
            //return localDirection.z >= 1;
        }

        /// <summary>
        /// Checks whether a player is a threat
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerAThreat(Player player)
        {
            return IsWithinDistance(Position, player.Position, DistanceThreatMin);
            // check if position is infront of me
            bool isThreatInFrontOfMe = IsInfrontOfPlayer(player.Position);

            // find the threat
            bool isThreatHigh = IsPositionAHighThreat(player.Position);
            bool isThreatLow = IsPositionALowThreat(player.Position) && isThreatHigh == false;

            // check the various threat types
            bool isPlayerHighlyThreatened = isThreatHigh && isThreatInFrontOfMe == false;
            bool isPlayerLowlylyThreatened = isThreatLow && isThreatInFrontOfMe == true;

            // return the result
            return isPlayerHighlyThreatened || isPlayerLowlylyThreatened;
        }

        public bool IsPositionAHighThreat(Vector3 position)
        {
            return IsPositionWithinHighThreatDistance(Position, position);
        }

        public bool IsPositionALowThreat(Vector3 position)
        {
            return IsPositionWithinLowThreatDistance(Position, position);
        }

        public bool IsPositionWithinHighThreatDistance(Vector3 center, Vector3 position)
        {
            return IsWithinDistanceRange(center, position, 0f, DistanceThreatMax);
        }

        public bool IsPositionWithinLowThreatDistance(Vector3 center, Vector3 position)
        {
            return IsWithinDistanceRange(center, position, DistanceThreatMin, DistanceThreatMax);
        }

        public bool IsPositionWithinPassRange(Vector3 position)
        {
            return IsWithinDistanceRange(Position,
                position,
                _distancePassMin,
                _distancePassMax);
        }

        public bool IsPositionWithinPassRange(Vector3 center, Vector3 position)
        {
            return IsWithinDistanceRange(center,
                position,
                _distancePassMin,
                _distancePassMax);
        }

        /// <summary>
        /// Check whether a position is a threat or not
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsPositionAThreat(Vector3 position)
        {
            // position is a threat if its within saftey distance
            return IsWithinDistance(Position, position, DistanceThreatMax);
        }

        public bool IsPositionAThreat(Vector3 center, Vector3 position)
        {
            return IsWithinDistance(center, position, DistanceThreatMax);
        }

        public bool IsPositionAHighThreat(Vector3 center, Vector3 position)
        {
            return IsWithinDistance(center, position, DistanceThreatMax);
        }

        public bool IsTeamMemberWithinMinPassDistance(Vector3 position)
        {
           foreach(Player tM in _teamMembers)
            {
                if (tM != this && IsPositionWithinMinPassDistance(position, tM.transform.position))
                    return true;
            }

            return false;
        }

        public bool IsThreatened()
        {
            //search for threatening player
            foreach(Player player in OppositionMembers)
            {
                if (IsPlayerAThreat(player))
                    return true;
            }

            //return false
            return false;
        }

        public bool IsWithinDistance(Vector3 center, Vector3 position, float distance)
        {
            return Vector3.Distance(center, position) <= distance;
        }

        public bool IsWithinDistanceRange(Vector3 center, Vector3 position, float minDistance, float maxDistance)
        {
            return !IsWithinDistance(center, position, minDistance) && IsWithinDistance(center, position, maxDistance);
        }

        /// <summary>
        /// Finds the power needed to kick the ball and make it reach
        /// a particular target with a particular velocity
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="arrivalVelocity"></param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float arrivalVelocity)
        {
            //find the power to target
            float power = Ball.Instance.FindPower(from,
                to,
                arrivalVelocity);

            //return result
            return power;
        }

        public void Invoke_OnBallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target)
        {
            BallLaunched temp = OnShotTaken;
            if (temp != null)
                temp.Invoke(flightTime, velocity, initial, target);
        }

        public void Invoke_OnBecameTheClosestPlayerToBall()
        {
            ActionUtility.Invoke_Action(OnBecameTheClosestPlayerToBall);
        }

        public void Invoke_OnInstructedToTakeKickOff()
        {
            ActionUtility.Invoke_Action(OnInstructedToTakeKickOff);
        }

        public void Invoke_OnInstructedToWait()
        {
            ActionUtility.Invoke_Action(OnInstructedToWait);
        }

        public void Invoke_OnIsNoLongerTheClosestPlayerToBall()
        {
            ActionUtility.Invoke_Action(OnIsNoLongerClosestPlayerToBall);
        }

        public void Invoke_OnTeamGainedPossession()
        {
            // set that my team is in control
            IsTeamInControl = true;

            // raise event that team is now in control
            ActionUtility.Invoke_Action(OnTeamGainedPossession);
        }

        public void Invoke_OnTeamLostControl()
        {
            // set team no longer in control
            IsTeamInControl = false;

            // invoke team has lost control
            ActionUtility.Invoke_Action(OnTeamLostControl);
        }

        /// <summary>
        /// Player kicks the ball from his position to the target
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void MakePass(Vector3 from, Vector3 to, Player receiver, float power, float time)
        {
            //kick the ball to target
            Ball.Instance.Kick(to, power);

            //message the receiver to receive the ball
            InstructedToReceiveBall temp = receiver.OnInstructedToReceiveBall;
            if (temp != null)
                temp.Invoke(time, to);
        }

        public void MakeShot(Vector3 from, Vector3 to, float power, float time)
        {
            //launch the ball
            Ball.Instance.Kick(to, power);

            // raise the ball shot event
            Ball.BallLaunched temp = Ball.Instance.OnBallShot;
            if (temp != null)
                temp.Invoke(time, power, from, to);
        }

        /// <summary>
        /// Puts the ball infront of this player
        /// </summary>
        public void PlaceBallInfronOfMe()
        {
            Ball.Instance.NormalizedPosition = Position + transform.forward * (Radius + _ballControlDistance);
            Ball.Instance.transform.rotation = transform.rotation;
        }

        public List<Player> GetTeamMembersInRadius(float radius)
        {
            //get the players
            List<Player> result = _teamMembers.FindAll(tM => Vector3.Distance(this.Position, tM.Position) <= radius 
            && this != tM);

            //retur result
            return result;
        }

        public Player GetRandomTeamMemberInRadius(float radius)
        {
            //get the list
            List<Player> players = GetTeamMembersInRadius(radius);

            //return random player
            if (players == null)
                return null;
            else
                return players[Random.Range(0, players.Count - 1)];
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }

            set
            {
                transform.rotation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(transform.position.x, 0f, transform.position.z);
            }

            set
            {
                transform.position = new Vector3(value.x, 0f, value.z);
            }
        }

        public float BallControlDistance { get => _ballControlDistance; set => _ballControlDistance = value; }
        public Goal OppGoal { get => _oppGoal; set => _oppGoal = value; }
        public float BallPassArriveVelocity { get => _ballPassArriveVelocity; set => _ballPassArriveVelocity = value; }
        public List<SupportSpot> PlayerSupportSpots { get => _pitchPoints; set => _pitchPoints = value; }

        public float DistancePassMin
        {
            get => _distancePassMin;
            set => _distancePassMin = value;
        }

        public float DistanceThreatMin
        {
            get => _distanceThreatMin + _radius;
            set => _distanceThreatMin = value;
        }

        public float DistanceThreatMax
        {
            get => _distanceThreatMax + _radius;
            set => _distanceThreatMax = value;
        }

        public RPGMovement RPGMovement
        {
            get
            {
                // set the rpg movement
                if (_rpgMovement == null)
                {
                    gameObject.AddComponent<RPGMovement>();
                    _rpgMovement = GetComponent<RPGMovement>();
                }

                // return result
                return _rpgMovement;
            }

            set
            {
                _rpgMovement = value;
            }
        }

        public float Radius { get => _radius; set => _radius = value; }
        public Goal TeamGoal { get => _teamGoal; set => _teamGoal = value; }
        public PlayerTypes PlayerType { get => _playerType; set => _playerType = value; }
        public float ThreatTrackDistance { get => _threatTrackDistance; set => _threatTrackDistance = value; }
        public float TendGoalSpeed { get => _tendGoalSpeed; set => _tendGoalSpeed = value; }
        public float TendGoalDistance { get => _tendGoalDistance; set => _tendGoalDistance = value; }
        public float GoalKeeping { get => _goalKeeping; set => _goalKeeping = value; }
        public float DistancePassMax { get => _distancePassMax; set => _distancePassMax = value; }
        public Player PrevPassReceiver { get => _prevPassReceiver; set => _prevPassReceiver = value; }
        public GameObject IconUserControlled { get => _iconUserControlled; set => _iconUserControlled = value; }
    }
}
