using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerType playerType;
        public InputData inputData;
        private void FixedUpdate()
        {
            if (playerType == PlayerType.Local)
            {
                inputData = InputManager.input;
            }
        }
    }
}
