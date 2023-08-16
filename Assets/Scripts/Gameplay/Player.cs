using Football.Gameplay.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Football.Gameplay
{
    public class Player : MonoBehaviour
    {
        public static Player mainPlayer;

        [Header("Character Settings")]
        public float rotationSmooth = 5f;
        public float moveSpeed = 5f;
        public float ballRotateSpeed = 15f;
        public float shootForce = 2f;
        public Transform foot;
        public GameObject arrow;
        public bool playerIsControllingThis;
        public GoalSide scoreSide;
        [Header("Ball")]
        public float ballDetectionRange = 1f;
        public Vector3 ballDetectionCenter;
        public LayerMask ballDetectionMask;
        public Animator animator;
        [HideInInspector] public new Rigidbody rigidbody;
        private PlayerInput input;
        private CapsuleCollider capsuleCollider;
        private Vector3 capsuleCenter;
        [HideInInspector] internal bool overrideAnimatorSpeed;
        private bool moving;
        private float yRotation;
        [HideInInspector] internal bool detectingBall;

        int animatorMoving;
        int animatorShoot;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            capsuleCenter = capsuleCollider.center;

            GetAnimatorHashes();
            SetupCollision();

            if (input.playerType == PlayerType.Local)
            {
                mainPlayer = this;
            }
        }

        public void SetupCollision()
        {
            Physics.IgnoreCollision(capsuleCollider, Ball.instance?.collider);
        }

        public void GetAnimatorHashes()
        {
            animatorMoving = Animator.StringToHash("Moving");
            animatorShoot = Animator.StringToHash("Shoot");
        }

        private void Update()
        {
            detectingBall = Physics.CheckSphere(transform.position + ballDetectionCenter, ballDetectionRange, ballDetectionMask);

            if (Ball.instance != null)
            {
                Ball.instance.detectingBall = detectingBall;
                if (detectingBall)
                {
                    Ball.instance.currentPlayer = this;
                }
            }

            arrow.gameObject.SetActive(playerIsControllingThis);
        }

        public void FixedUpdate()
        {
            if (input.inputData.move.x > 0)
            {
                capsuleCollider.center = capsuleCenter;
                yRotation = 0;
            }
            else if (input.inputData.move.x < 0)
            {
                capsuleCollider.center = new Vector3(capsuleCenter.x * -1, capsuleCenter.y, capsuleCenter.z);
                yRotation = 180;
            }

            moving = input.inputData.move.magnitude > 0;

            animator.SetBool(animatorMoving, moving);

            Vector3 move = new Vector3(input.inputData.move.x, 0, input.inputData.move.y);
            rigidbody.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.fixedDeltaTime);

            if (!overrideAnimatorSpeed)
            {
                if (moving)
                {
                    animator.speed = input.inputData.move.normalized.magnitude;
                }
                else
                {
                    animator.speed = 1;
                }
            }

            if (input.inputData.shoot && detectingBall)
            {
                Ball.instance?.rigidbody.AddForce(move * shootForce, ForceMode.Impulse);
            }
            else
            if (detectingBall)
            {
                Ball.instance?.Move(foot.position);
                Ball.instance?.Rotate(input.inputData.move, ballRotateSpeed);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + ballDetectionCenter, ballDetectionRange);
        }
    }
}
