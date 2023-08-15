using Football.Gameplay.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay.AI
{
    public class FSMSoccer : MonoBehaviour
    {
        [Header("Settings")]
        public GoalSide side;
        public float shootToGoalDistance = 5f;
        public Vector3 offset;

        PlayerInput input;
        Player player;
        bool shoot;
        bool returnToInitial;
        Vector3 direction;
        Vector3 initialPosition;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            player = GetComponent<Player>();
            initialPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (returnToInitial)
            {
                direction = ((transform.position + offset) - initialPosition).normalized;

                if (Vector3.Distance(transform.position, initialPosition) < 1f)
                {
                    returnToInitial = false;
                }

            }
            else if (Vector3.Distance(transform.position, Ball.instance.transform.position) > 0.1f && !player.detectingBall && !shoot)
            {
                direction = ((transform.position + offset) - Ball.instance.transform.position).normalized;
            }
            else if (player.detectingBall && !shoot)
            {
                switch (side)
                {
                    case GoalSide.Left:
                        direction = ((transform.position + offset) - Goal.rightGoal.transform.position).normalized;

                        if (Vector2.Distance(transform.position, Goal.rightGoal.transform.position) < shootToGoalDistance)
                        {
                            shoot = true;
                            StartCoroutine(ShootBall());
                        }

                        Debug.Log(Vector2.Distance(transform.position, Goal.rightGoal.transform.position));

                        break;
                    case GoalSide.Right:
                        direction = ((transform.position + offset) - Goal.leftGoal.transform.position).normalized;

                        if (Vector2.Distance(transform.position, Goal.leftGoal.transform.position) < shootToGoalDistance)
                        {
                            shoot = true;
                            StartCoroutine(ShootBall());
                        }

                        Debug.Log(Vector2.Distance(transform.position, Goal.leftGoal.transform.position));

                        break;
                }

            }

            input.inputData.move = new Vector2(-direction.x, -direction.z);
        }


        IEnumerator ShootBall()
        {
            yield return new WaitForSeconds(0.5f);
            shoot = false;
            input.inputData.shoot = true;
            yield return new WaitForSeconds(1f);
            input.inputData.shoot = false;
            yield return new WaitForSeconds(0.1f);
            returnToInitial = true;
        }
    }
}
