using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay.Input
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerType playerType;
        public InputData inputData;
        Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            if (player.playerIsControllingThis)
            {
                inputData = InputManager.input;
            }
        }
    }
}
