using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Gameplay
{
    [Serializable]
    public class Formation
    {
        public Transform Goalkeeper;
        public Transform P1;
        public Transform P2;
        public Transform P3;
        public Transform P4;

        public Transform GetPositionByIndex(int index)
        {
            index = Mathf.Clamp(index, 0, 3);
            switch (index)
            {
                case 0: return P1;
                case 1: return P2;
                case 2: return P3;
                case 3: return P4;
                default: return null;
            }
        }
    }
}
