using Football.Gameplay.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Football.Gameplay
{
    public class Player : MonoBehaviour
    {
        [Header("Character Settings")]
        public float rotationSmooth = 5f;
        public float moveSpeed = 5f;
        public float ballRotateSpeed = 15f;
        public Transform foot;
        [Header("Ball")]
        public float ballDetectionRange = 1f;
        public Vector3 ballDetectionCenter;
        public LayerMask ballDetectionMask;

        public Animator animator;
        [HideInInspector] public new Rigidbody rigidbody;
        private PlayerInput input;
        private CapsuleCollider capsuleCollider;
        private Vector3 capsuleCenter;

        private bool moving;
        private float yRotation;
        private bool detectingBall;

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
        }

        public void SetupCollision()
        {
            Physics.IgnoreCollision(capsuleCollider, Ball.instance.collider);
        }

        public void GetAnimatorHashes()
        {
            animatorMoving = Animator.StringToHash("Moving");
            animatorShoot = Animator.StringToHash("Shoot");
        }

        private void Update()
        {
            detectingBall = Physics.CheckSphere(transform.position + ballDetectionCenter, ballDetectionRange, ballDetectionMask);

            Ball.instance.detectingBall = detectingBall;

            if (detectingBall)
            {
                Ball.instance.currentPlayer = this;
            }

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

            if (detectingBall)
            {
                Ball.instance.Move(foot.position);
                Ball.instance.Rotate(input.inputData.move, ballRotateSpeed);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + ballDetectionCenter, ballDetectionRange);
        }
    }
}
