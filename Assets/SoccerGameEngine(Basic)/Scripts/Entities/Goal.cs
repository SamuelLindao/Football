using Assets.SoccerGameEngine_Basic_.Scripts.Triggers;
using System;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Entities
{
    public class Goal : MonoBehaviour
    {
        [SerializeField]
        GoalMouth _goalMouth;

        [SerializeField]
        GoalTrigger _goalTrigger;

        [SerializeField]
        Transform _shotTargetReferencePoint;

        /// <summary>
        /// Action raised when goal collides with the ball
        /// </summary>
        public Action OnCollideWithBall;

        ///ToDo::Speak about why you put them here as an initialization
        public Vector3 BottomLeftRelativePosition { get; set; }
        public Vector3 BottomRightRelativePosition { get; set; }
        public Vector3 Position { get => transform.position; }
        public Vector3 ShotTargetReferencePoint { get => _shotTargetReferencePoint.position; }

        private void Awake()
        {
            //init some data 
            BottomLeftRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomLeft.position);
            BottomRightRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomRight.position);

            _goalTrigger.Goal = this;

            //listen to the goal-trigger events
            _goalTrigger.OnCollidedWithBall += Instance_OnCollidedWithBall;
        }

        private void Instance_OnCollidedWithBall()
        {
            //raise the on collision with ball event
            Action temp = OnCollideWithBall;
            if (temp != null)
                temp.Invoke();
        }

        public bool IsPositionWithinGoalMouthFrustrum(Vector3 position)
        {
            //find the relative position to goal
            Vector3 relativePosition = transform.InverseTransformPoint(position);

            //find the relative position of each goal mouth
            Vector3 pointBottomLeftRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomLeft.position);
            Vector3 pointBottomRightRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomRight.position);
            Vector3 pointTopLeftRelativePosition = transform.InverseTransformPoint(_goalMouth._pointTopLeft.position);

            //check if the x- coordinate of the relative position lies within the goal mouth
            bool isPositionWithTheXCoordinates = relativePosition.x > pointBottomLeftRelativePosition.x && relativePosition.x < pointBottomRightRelativePosition.x;
            bool isPositionWithTheYCoordinates = relativePosition.y > pointBottomLeftRelativePosition.y && relativePosition.y < pointTopLeftRelativePosition.y;

            //the result is the combination of the two tests
            return isPositionWithTheXCoordinates && isPositionWithTheYCoordinates;
        }

    }

    [Serializable]
    public struct GoalMouth
    {
        public Transform _pointBottomLeft;
        public Transform _pointBottomRight;
        public Transform _pointTopLeft;
        public Transform _pointTopRight;
    }
}
