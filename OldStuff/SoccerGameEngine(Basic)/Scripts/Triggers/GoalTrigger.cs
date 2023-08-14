using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Triggers
{
    public class GoalTrigger : MonoBehaviour
    {
        public Goal Goal;// { get; set; }

        public Action OnCollidedWithBall;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Ball")
            {
                //invoke that the wall has collided with the ball
                Action temp = OnCollidedWithBall;
                if (temp != null)
                    temp.Invoke();
            }
        }
    }
}
