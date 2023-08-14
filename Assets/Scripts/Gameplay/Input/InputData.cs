using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay.Input
{
    [Serializable]
    public struct InputData : IGameInput
    {
        public Vector2 move;
        public bool through;
        public bool shoot;
        public bool pass;
        public bool sprintAndSkill;
    }
}
