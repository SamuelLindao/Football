using Football.Gameplay.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay
{
    public class Goal : MonoBehaviour
    {
        [Header("Settings")]
        public GoalSide side;

        public static Goal leftGoal;
        public static Goal rightGoal;

        private void Awake()
        {
            switch (side)
            {
                case GoalSide.Left:
                    leftGoal = this;
                    break;
                case GoalSide.Right:
                    rightGoal = this;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball") && !Ball.instance.detectingBall)
            {
                switch (side)
                {
                    case GoalSide.Left:
                        if (Player.mainPlayer.scoreSide == GoalSide.Right)
                        {
                            MatchManager.instance.rightTeamScore++;
                        }
                        else
                        {
                            MatchManager.instance.leftTeamScore++;
                        }
                        break;
                    case GoalSide.Right:

                        if (Player.mainPlayer.scoreSide == GoalSide.Left)
                        {
                            MatchManager.instance.rightTeamScore++;
                        }
                        else
                        {
                            MatchManager.instance.leftTeamScore++;
                        }
                        break;
                }

                MatchManager.instance.confetti.Play(true);
                MatchManager.instance.UpdateScore();
            }
        }
    }

    public enum GoalSide
    {
        Left,
        Right
    }
}
