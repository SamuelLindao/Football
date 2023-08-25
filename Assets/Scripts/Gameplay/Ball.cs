using Football.Gameplay;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football
{
    public class Ball : MonoBehaviour
    {
        public static Ball instance;

        public float moveSmooth = 5f;

        [HideInInspector] public bool detectingBall;
        [HideInInspector] public Player currentPlayer;
        [HideInInspector] public new Rigidbody rigidbody;
        [HideInInspector] public MeshCollider collider;

        private void Awake()
        {
            instance = this;
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<MeshCollider>();
        }

        public void Move(Vector3 position)
        {
            transform.position = Vector3.Lerp(transform.position, position, moveSmooth * Time.deltaTime);
        }

        public void Destroy()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
            }
        }

        public void Rotate(Vector3 input, float speed)
        {
            if (input.x > 0)
            {
                transform.Rotate(transform.right * speed);
            }
            else if (input.x < 0)
            {
                transform.Rotate(-transform.right * speed);
            }
            else if (input.y > 0)
            {
                transform.Rotate(transform.forward * speed);
            }
            else if (input.y < 0)
            {
                transform.Rotate(-transform.forward * speed);
            }
        }
    }
}
