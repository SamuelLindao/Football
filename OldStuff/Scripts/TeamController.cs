using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TeamController : MonoBehaviour
{
    public static TeamController instance;

    [Header("Player")]
    public PlayerMovement.actualTeamEnum MyTeam;
    public PlayerMovement ActualPlayer;
    public PlayerMovement PlayerAround;
    public List<PlayerMovement> MyTeamPlayers;
    public List<Vector3> allInitialPos;
    [Header("Joystick")]
    public VariableJoystick Joystick;
    public float Horizontal;
    public float Vertical;
    [Header("Camera")]
    public CinemachineVirtualCamera MyCam;
    [Header("Kick")]
    public ButtonLongPressListener Button;
    public float KickForce;

    [Header("Debug")]
    public Slider slider;

    [Header("Match")]
    public int TeamBlueScore;
    public int TeamRedScore;
    public int ActualTime = 1;
    public int MatchDuration;
    [Space]
    public float PauseTime;
    public bool OnPause;



    [Space]
    public GameObject text;
    public GameObject ball;
    public GameObject ballOnGame;
    public Transform ballSpawn;
    public DateTime finishTime;
    public DateTime startTime;
    public TextMeshProUGUI BluePoints;
    public TextMeshProUGUI RedPoints;
    public TextMeshProUGUI MatchTime;
    public TextMeshProUGUI PowerTime;
    public TextMeshProUGUI PlayerName;
    public GameObject GoalText;

    [Space]
    public List<string> blueTeam;
    public List<string> redTeam;
    [Header("Container")]
    public AbilityContainer container;
    public TacticsContainer tacticsContainer;
    public GameObject player;
    [Space]
    public List<AroundClass> distances = new List<AroundClass>();

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {

        StartMatch();    
        PlayerMovement[] team = FindObjectsOfType<PlayerMovement>();

        for(int i =0; i < team.Length; i++)
        {
            if(team[i].ActualTeam == MyTeam)
            {
                MyTeamPlayers.Add(team[i]);
            }
        }
        if(ActualPlayer == null)
        {
            ActualPlayer = MyTeamPlayers[0];
        }

        for(int i = 0; i < MyTeamPlayers.Count;i++)
        {
            allInitialPos.Add(MyTeamPlayers[i].transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(ballSpawn.transform.position, 0.5f);
    }
    [System.Serializable]
    public class AroundClass
    {

        public GameObject obj;
        public float Dis;
    }

    private void Update()
    {
        ActualPlayer.moveHorizontal = Joystick.Horizontal;
        ActualPlayer.moveVertical = Joystick.Vertical;
        KickForce = Mathf.Clamp(Button.myTime, 0, 1);
        slider.value = KickForce;
        distances = new List<AroundClass>();
        for(int i =0; i  < MyTeamPlayers.Count;i++)
        {
            if (MyTeamPlayers[i] != ActualPlayer)
            {
                MyTeamPlayers[i].moveHorizontal = 0;
                MyTeamPlayers[i].moveVertical = 0;
                distances.Add(new AroundClass());
                distances[distances.Count - 1].Dis = Vector3.Distance(MyTeamPlayers[i].transform.position, ActualPlayer.transform.position);
                distances[distances.Count - 1].obj = MyTeamPlayers[i].gameObject;
           
                
            } 
        }
        if (distances.Count > 0)
        {
            float selected = distances[0].Dis;
            PlayerAround = distances[0].obj.GetComponent<PlayerMovement>();
            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i].Dis < selected)
                {
                    selected = distances[i].Dis;
                    PlayerAround = distances[i].obj.GetComponent<PlayerMovement>();
                }
            }
        }
        MyCam.Follow = ActualPlayer.gameObject.transform;
        MyCam.LookAt = ActualPlayer.gameObject.transform;
        //Match Controll
        MatchTime.text = DateTime.Now.Subtract(startTime).Minutes.ToString("00")+ ":" + DateTime.Now.Subtract(startTime).Seconds.ToString("00");
        BluePoints.text = TeamBlueScore.ToString();
        RedPoints.text = TeamRedScore.ToString();
        PlayerName.text = ActualPlayer.PlayerName;
        if ((int)ActualPlayer.TimeToReturn.Subtract(DateTime.Now).TotalSeconds > 0)
        {
            PowerTime.text = ((int)ActualPlayer.TimeToReturn.Subtract(DateTime.Now).TotalSeconds).ToString();
        }
        else
        {
            PowerTime.text = "Ready!";
        }
    }


    public void PlayerPass()
    {
        ActualPlayer.Pass();
    }

    public void StartMatch()
    {
        ballOnGame = Instantiate(ball, ballSpawn.transform.position, ballSpawn.transform.rotation);
        startTime = DateTime.Now;
        finishTime = DateTime.Now.AddMinutes(MatchDuration);
        SpawnPlayers(blueTeam, PlayerMovement.actualTeamEnum.Blue);
        SpawnPlayers(redTeam, PlayerMovement.actualTeamEnum.Red);
    }
    public void SpawnPlayers(List<string> Team, PlayerMovement.actualTeamEnum MyTeam)
    {
        for(int i =0; i < Team.Count; i++)
        {
            GameObject obj = Instantiate(player, tacticsContainer.AllTactics[0].allPos[i]+ ballSpawn.transform.position, Quaternion.Euler(0,0,0));
            PlayerMovement mov = obj.GetComponent<PlayerMovement>();
            mov.name = Team[i];
            mov.ActualTeam = MyTeam;
            if (MyTeam == PlayerMovement.actualTeamEnum.Blue)
            {
                obj.transform.position = tacticsContainer.AllTactics[0].allPos[i] + ballSpawn.transform.position;
            
            }
            else
            {
                obj.transform.position = (new Vector3( tacticsContainer.AllTactics[0].allPos[i].x * -1, tacticsContainer.AllTactics[0].allPos[i].y , tacticsContainer.AllTactics[0].allPos[i].z * -1) + ballSpawn.transform.position);

            }
        }
    }
    public void GoalSet(PlayerMovement.actualTeamEnum team)
    {
        switch (team)
        {
            case PlayerMovement.actualTeamEnum.Blue:
                TeamRedScore++;
                OnPause = true;
                GoalText.SetActive(true);
                StartCoroutine(PauseStop());
                break;
            case PlayerMovement.actualTeamEnum.Red:
                TeamBlueScore++;
                OnPause = true; 
                GoalText.SetActive(true);
                StartCoroutine(PauseStop());
                break;
        }
    }

    public IEnumerator PauseStop()
    {
        yield return new WaitForSeconds(PauseTime);
        OnPause = false;
        GoalText.SetActive(false);
        ballOnGame.GetComponent<Rigidbody>().MovePosition(ballSpawn.position);
        ballOnGame.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballOnGame.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
        for (int i = 0; i < allInitialPos.Count; i++)
        {
            MyTeamPlayers[i].transform.position = allInitialPos[i];
        }
    }

    public void UsePower()
    {
        if(ActualPlayer.SuperPower)
        {
            AbilityInfo ability = container.myAbilitys[ActualPlayer.MyPower];
            GameObject obj = Instantiate(ability.ActionObj, transform);
            AbilityClass ab = obj.GetComponent<AbilityClass>();
            ab.entity = ActualPlayer.gameObject;
            ab.MyName = ability.AbilityName;

            ActualPlayer.SuperPower = false;
            ActualPlayer.TimeToReturn = DateTime.Now.AddSeconds(ability.AbilityMax);
            GameObject tex = Instantiate(text, text.transform.parent);
            tex.SetActive(true);
            tex.GetComponent<TextMeshProUGUI>().text = ActualPlayer.gameObject.name + " used " + ability.AbilityName;
            if (ActualPlayer.ActualTeam == PlayerMovement.actualTeamEnum.Blue)
            {
                tex.GetComponent<TextMeshProUGUI>().color = Color.blue;
            }
            else
            {
                tex.GetComponent<TextMeshProUGUI>().color = Color.red;

            }
            Destroy(obj, 5f);
            Destroy(tex, 5f);
        }
    }
}
