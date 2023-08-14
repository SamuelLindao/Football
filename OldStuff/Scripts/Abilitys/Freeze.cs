using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private void Start()
    {
        DoFreeze();
    }
    public void DoFreeze()
    {
        TeamController.instance.ballOnGame.GetComponent<Rigidbody>().velocity = Vector3.zero;
        TeamController.instance.ballOnGame.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
