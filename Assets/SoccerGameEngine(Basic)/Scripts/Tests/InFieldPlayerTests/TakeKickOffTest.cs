using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    /// <summary>
    /// We test the kick-off. On success, the controlling player is instructed to take the kick-off
    /// He goes into go to home state
    /// The receiveing player becomes the controlling player and he receives the ball and goes into control ball
    /// The OnTakeKickOff evevent is raised by the player
    /// </summary>
    public class TakeKickOffTest : MonoBehaviour
    {

        public Player PlayerControlling;
        public Player PlayerSupporting;


        public Action InstructToTakeKickOff;
        public Action InstructToWait;

        private void Awake()
        {
            PlayerControlling.OnTakeKickOff += Instance_OnControllingPlayerTakeKickOff;

            InstructToWait += PlayerControlling.Invoke_OnInstructedToWait;
            InstructToWait += PlayerSupporting.Invoke_OnInstructedToWait;

            InstructToTakeKickOff += PlayerControlling.Invoke_OnInstructedToTakeKickOff;
        }

        private void Instance_OnControllingPlayerTakeKickOff()
        {
            Debug.Log("<color=blue>Player taken kick-off.</color>");
        }

        private void Update()
        {
            //invoke wait
            if (Input.GetKeyDown(KeyCode.W))
                ActionUtility.Invoke_Action(InstructToWait);

            //invoke go to kick-off
            if (Input.GetKeyDown(KeyCode.K))
                ActionUtility.Invoke_Action(InstructToTakeKickOff);

        }
    }
}
