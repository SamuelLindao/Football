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
        public PlayerFormation formation;
        public float maxGoalKeeperPosition = 3f;
        public float shootToGoalDistance = 5f;
        public Vector3 offset;
        PlayerInput input;
        Player player;
        bool shoot;
        bool returnToInitial;
        Vector3 direction;
        Vector3 initialPosition;
        float initialX;
        float flip;
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            player = GetComponent<Player>();
            initialPosition = transform.position;
            initialX = initialPosition.x;
        }

        private void Start()
        {
            if (formation == PlayerFormation.Goalkeeper)
            {
                player.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                player.overrideAnimatorSpeed = true;
                player.animator.speed = 0;
            }
        }

        private void FixedUpdate()
        {
            if (formation == PlayerFormation.Line)
            {
                if (returnToInitial)
                {
                    direction = ((transform.position + offset) - initialPosition).normalized;

                    if (Vector3.Distance(transform.position, initialPosition) < 1f)
                    {
                        returnToInitial = false;
                    }

                }
                else if (Vector3.Distance(transform.position, (Vector3)(Ball.instance?.transform.position)) > 0.1f && !player.detectingBall && !shoot)
                {
                    direction = ((transform.position + offset) - (Vector3)Ball.instance?.transform.position).normalized;
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

                if (!player.playerIsControllingThis)
                {
                    input.inputData.move = new Vector2(-direction.x, -direction.z);
                }
            }
            else
            {
                direction = (transform.position - (Vector3)Ball.instance?.transform.position).normalized;
                if (side == GoalSide.Left)
                {
                    flip = 1;
                }
                else
                {
                    flip = -1;
                }
                input.inputData.move = new Vector2(flip, -direction.z);
                transform.position = new Vector3(initialX, transform.position.y, transform.position.z);

                if (player.detectingBall)
                {
                    input.inputData.shoot = true;
                }
                else
                {
                    input.inputData.shoot = false;
                }
            }
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
