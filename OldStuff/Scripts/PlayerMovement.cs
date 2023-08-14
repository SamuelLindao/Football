using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public string PlayerName;
    [System.Serializable]
    public enum actualTeamEnum { Blue, Red};
    [Space]

    public actualTeamEnum ActualTeam;


    public float startSpeed = 5f;
    public float maxSpeed = 10f;
    public float maxSpeedWBall = 8f;
    public float maxSpeedWithout = 10f;
    public float acceleration = 2f;
    public float deceleration = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    public float moveHorizontal;
    public float moveVertical;

    public GameObject dir;

    public Rigidbody ball;
    public bool Kicking;
    public System.DateTime TimeToReturn;
    public bool SuperPower;

    public Transform foot;
    public GameObject canvas;
    public TMPro.TextMeshProUGUI myName;
    public GameObject selected;
    [Space]
    public int MyPower;
    private void Start()
    {
        TimeToReturn = System.DateTime.Now;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(System.DateTime.Now > TimeToReturn)
        {
            SuperPower = true;
        }
        selected.SetActive(TeamController.instance.ActualPlayer == this);
        myName.text = PlayerName;
       
    }
    private void FixedUpdate()
    {

        moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        maxSpeed = maxSpeedWBall;
        GetBall();

        if (!Kicking)
        {
            if (moveDirection.magnitude > 0f)
            {
                float currentSpeed = rb.velocity.magnitude;
                float targetSpeed = maxSpeed;
                float accelerationDirection = Mathf.Sign(targetSpeed - currentSpeed);
                float newSpeed = currentSpeed + accelerationDirection * acceleration * Time.fixedDeltaTime;
                newSpeed = Mathf.Clamp(newSpeed, startSpeed, maxSpeed);

                moveDirection *= newSpeed;

                rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
                if (moveHorizontal < 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Round(-180), 0);
                }
                else if (moveHorizontal > 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Round(0), 0);

                }

            }
            else
            {
                float currentSpeed = rb.velocity.magnitude;
                float decelerationDirection = -Mathf.Sign(currentSpeed);
                float newSpeed = currentSpeed + decelerationDirection * deceleration * Time.fixedDeltaTime;
                newSpeed = Mathf.Clamp(newSpeed, 0f, maxSpeed);

                rb.velocity = new Vector3(newSpeed * rb.velocity.normalized.x, rb.velocity.y, newSpeed * rb.velocity.normalized.z);
            }
        }
        print(Mathf.Sign(moveHorizontal));
        canvas.transform.localScale = new Vector3(1 * Mathf.Sign(moveHorizontal), 0.5526045f, 1);
        dir.transform.localRotation = Quaternion.Euler(0, (90 * moveVertical * Mathf.Sign(transform.rotation.y)) * -1, 0);
    }
    public void GetBall()
    {
        if (!Kicking)
        {
            if (ball == null)
            {
                maxSpeed = maxSpeedWithout;

                if (Vector3.Distance(FindObjectOfType<Ball>().transform.position, foot.position) <= 1.5f)
                {
                    ball = FindObjectOfType<Ball>().GetComponent<Rigidbody>();
                    ball.MovePosition(new Vector3(foot.position.x + 0.32f * Mathf.Sign(transform.rotation.y), foot.position.y, foot.position.z));

                    maxSpeed = maxSpeedWBall;
                    if (TeamController.instance.MyTeamPlayers.Contains(this)) ;
                    {
                        TeamController.instance.ActualPlayer = this;
                        TeamController.instance.PlayerAround = null;
                    }

                    PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i] != this)
                        {
                            players[i].ball = null;
                        }
                    }
                }
              
            }
            else
            {
                                    ball.MovePosition(new Vector3(foot.position.x + 0.32f * Mathf.Sign(transform.rotation.y), foot.position.y, foot.position.z));

            }
        }
    }

    public void Kick()
    {
        if (ball != null)
        {

            Vector3 ballMoveDirection = new Vector3(moveHorizontal, 0.25f, moveVertical).normalized;

            if (moveHorizontal != 0 && moveVertical != 0)
            {
                ball.AddForce(ballMoveDirection * 1500 * TeamController.instance.KickForce, ForceMode.Force);
            }
            else
            {
                ball.AddForce(new Vector3(0.75f * Mathf.Sign(transform.rotation.y), 0.1f, 0) * 1500 * TeamController.instance.KickForce, ForceMode.Force);


            }
            ball = null;

            StartCoroutine(perm());
        }
    }

    public void Pass()
    {
        if (ball != null)
        {
            Kicking = true;

            if (TeamController.instance.PlayerAround != null)
            {
                Vector3 ballMoveDirection = TeamController.instance.PlayerAround.transform.position - transform.position;
                ball.AddForce(ballMoveDirection.normalized * 750, ForceMode.Force);

            }
            else
            {
                ball.AddForce(new Vector3(0.75f * Mathf.Sign(transform.rotation.y), 0, 0) * 1250, ForceMode.Force);
            }
            ball = null;

            StartCoroutine(perm());
        }
        else
        {
            if (TeamController.instance.PlayerAround != null)
            {
                TeamController.instance.ActualPlayer = TeamController.instance.PlayerAround;
                TeamController.instance.PlayerAround = null;

            }
            else
            {
                TeamController.instance.ActualPlayer = TeamController.instance.MyTeamPlayers[Random.Range(0, TeamController.instance.MyTeamPlayers.Count )];

            }
        }
    
    }
    public IEnumerator perm()
    {
        yield return new WaitForSeconds(2.2f);
        Kicking = false;    
    }
}
