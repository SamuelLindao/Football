using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public PlayerMovement.actualTeamEnum MyTeam;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            TeamController.instance.GoalSet(MyTeam);
        }
    }
}
