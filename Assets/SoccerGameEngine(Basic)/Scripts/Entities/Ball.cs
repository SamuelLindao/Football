using System;
using Patterns.Singleton;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Entities
{
    public class Ball : Singleton<Ball>
    {
        [SerializeField]
        [Min(0)]
        float _friction = 3f;

        [SerializeField]
        [Min(0)]
        float _gravity = 9.11f;

        [SerializeField]
        string _groundMaskName;

        bool _isGrounded;
        float _rayCastDistance;
        int _groundMask;
        RaycastHit _hit;
        Vector3 _frictionVector;
        Vector3 _rayCastStartPosition;

        public delegate void BallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target);

        public BallLaunched OnBallLaunched;
        public BallLaunched OnBallShot;

        public float Friction { get => -_friction; set => _friction = value; }

        public Player Owner { get; set; }

        public Rigidbody Rigidbody { get; set; }
        public SphereCollider SphereCollider { get; set; }

        public override void Awake()
        {
            base.Awake();

            //get the components
            Rigidbody = GetComponent<Rigidbody>();
            SphereCollider = GetComponent<SphereCollider>();

            //init some variables
            _groundMask = LayerMask.GetMask(_groundMaskName);
            _rayCastDistance = SphereCollider.radius + 0.05f;
        }

        private void FixedUpdate()
        {
            ApplyFriction();
        }

        /// <summary>
        /// Applies friction to this instance
        /// </summary>
        public void ApplyFriction()
        {
            //get the direction the ball is travelling
            _frictionVector = Rigidbody.velocity.normalized;
            _frictionVector.y = 0f;

            //calculate the actual friction
            _frictionVector *= -1 * _friction;

            //calculate the raycast start positiotn
            _rayCastStartPosition = transform.position + SphereCollider.radius * Vector3.up;

            //check if the ball is touching with the pitch
            //if yes apply the ground friction force
            //else apply the air friction
            _isGrounded = Physics.Raycast(_rayCastStartPosition,
                Vector3.down,
                out _hit,
                _rayCastDistance,
                _groundMask);

            //apply friction if grounded
            if (_isGrounded)
                Rigidbody.AddForce(_frictionVector);

#if UNITY_EDITOR
            Debug.DrawRay(_rayCastStartPosition, 
                Vector3.down * _rayCastDistance, 
                Color.red);
#endif

        }

        /// <summary>
        /// Finds the power needed to kick an entity to reach it's destination
        /// with the specifed velocity
        /// </summary>
        /// <param name="from">The initial position</param>
        /// <param name="to">The final position</param>
        /// <param name="finalVelocity">The initial velocity</param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float finalVelocity)
        {
            // v^2 = u^2 + 2as => u^2 = v^2 - 2as => u = root(v^2 - 2as)
            return Mathf.Sqrt(Mathf.Pow(finalVelocity, 2f) - (2 * -_friction * Vector3.Distance(from, to)));
        }

        /// <summary>
        /// Kicks the ball to the target
        /// </summary>
        /// <param name="to"></param>
        /// <param name="power"></param>
        public void Kick(Vector3 to, float power)
        {
            Vector3 direction = to - NormalizedPosition;
            direction.Normalize();

            //change the velocity
            Rigidbody.velocity = direction * power;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(0f, power, NormalizedPosition, to);
        }

        public void Launch(float power, Vector3 final)
        {
            //set the initial position
            Vector3 initial = Position;

            //find the direction vectors
            Vector3 toTarget = final - initial;
            Vector3 toTargetXZ = toTarget;
            toTargetXZ.y = 0;

            //find the time to target
            float time = toTargetXZ.magnitude / power;

            // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
            // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
            // so xz = v0xz * t => v0xz = xz / t
            // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
            toTargetXZ = toTargetXZ.normalized * toTargetXZ.magnitude / time;

            //set the y-velocity
            Vector3 velocity = toTargetXZ;
            velocity.y = toTarget.y / time + (0.5f * _gravity * time);

            //return the velocity
            Rigidbody.velocity = velocity;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(time, power, initial, final);
        }

        public void Trap()
        {
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
        }

        public float TimeToCoverDistance(Vector3 from, Vector3 to, float initialVelocity, bool factorInFriction = true)
        {
            //find the distance
            float distance = Vector3.Distance(from, to);

            //if I'm not factoring friction or I'm factoring in friction but no friction has been specified
            //simply assume there is no friction(ball is self accelerating)
            if(!factorInFriction || (factorInFriction && _friction == 0))
            {
                return distance / initialVelocity;
            }
            else
            {
                // v^2 = u^2 + 2as
                float v_squared = Mathf.Pow(initialVelocity, 2f) + (2 * _friction * Vector3.Distance(from, to));

                //if v_squared is less thatn or equal to zero it means we can't reach the target
                if (v_squared <= 0)
                    return -1.0f;

                // t = v-u
                //     ---
                //      a
                return (Mathf.Sqrt(v_squared) - initialVelocity) / (_friction);
            }
        }

        /// <summary>
        /// Get the normalized ball position
        /// </summary>
        public Vector3 NormalizedPosition
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

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }
    }
}
