using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGoal : MonoBehaviour
{
    public AbilityClass ability;

    private void Start()
    {
        ability = GetComponent<AbilityClass>();
        DoShot();
    }

    public void DoShot()
    {
        if (TeamController.instance.ActualPlayer.ball != null)
        {
            TeamController.instance.ActualPlayer.Kicking = true;
            Vector3 ballMoveDirection = new Vector3(TeamController.instance.ActualPlayer.moveHorizontal, 0.25f, TeamController.instance.ActualPlayer.moveVertical).normalized;

            if (TeamController.instance.ActualPlayer.moveHorizontal != 0 && TeamController.instance.ActualPlayer.moveVertical != 0)
            {
                TeamController.instance.ActualPlayer.ball.AddForce(ballMoveDirection * 2500, ForceMode.Force);
            }
            else
            {
                TeamController.instance.ActualPlayer.ball.AddForce(new Vector3(0.75f * Mathf.Sign(transform.rotation.y), 0.1f, 0) * 2500 * 1, ForceMode.Force);


            }
            TeamController.instance.ActualPlayer.ball = null;

            StartCoroutine(TeamController.instance.ActualPlayer.perm());
        }
    }
}
