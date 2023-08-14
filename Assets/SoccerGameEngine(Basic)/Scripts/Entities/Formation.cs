using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Entities
{
    public class Formation : MonoBehaviour
    {
        [SerializeField]
        Transform _positionsAttackingRoot;

        [SerializeField]
        Transform _positionsDefendingRoot;

        [SerializeField]
        Transform _positionCurrentHomeRoot;

        [SerializeField]
        Transform _positionsKickOffRoot;

        public Transform PositionsAttackingRoot
        {
            get
            {
                return _positionsAttackingRoot;
            }
            set
            {
                _positionsAttackingRoot = value;
            }
        }

        public Transform PositionsDefendingRoot
        {
            get
            {
                return _positionsDefendingRoot;
            }
            set
            {
                _positionsDefendingRoot = value;
            }
        }

        public Transform PositionsCurrentHomeRoot
        {
            get
            {
                return _positionCurrentHomeRoot;
            }
            set
            {
                _positionCurrentHomeRoot = value;
            }
        }

        public Transform PositionsKickOffRoot
        {
            get
            {
                return _positionsKickOffRoot;
            }
            set
            {
                _positionsKickOffRoot = value;
            }
        }
    }
}
