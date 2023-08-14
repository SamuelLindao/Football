using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerControlBallTester : MonoBehaviour
    {
        bool canRun = false;

        public GameObject _threatIndicator;
        public LineRenderer _lineRenderer001;
        public LineRenderer _lineRenderer002;
        public Player _primaryPlayer;
        public Player _secondaryPlayer;
        public Player _oppositionPlayer;

        public Action InstructToWait;

        private void Awake()
        {
            InstructToWait += _secondaryPlayer.Invoke_OnInstructedToWait;
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }
        public void Update()
        {
            _threatIndicator.transform.position = _primaryPlayer.transform.position;

            //if (_primaryPlayer.IsPositionAHighThreat(_oppositionPlayer.Position))
            //    _threatIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
            //else if (_primaryPlayer.IsPositionALowThreat(_oppositionPlayer.Position))
            //    _threatIndicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
            //else
            //    _threatIndicator.GetComponent<MeshRenderer>().material.color = Color.green;

            //if (canRun)
            //{
            //    bool canPass = _primaryPlayer.CanPass();
            //    Vector3 orthPoint = Vector3.zero;

            //   //set position to target
            //   _lineRenderer001.SetPositions(new Vector3[]
            //    {
            //        Ball.Instance.NormalizedPosition,
            //        canPass ? (Vector3)_primaryPlayer.PassTarget : _secondaryPlayer.Position
            //    });

            //    //render line
            //    if (canPass)
            //    {
            //        _lineRenderer001.material.color = Color.green;

            //        //draw line to orth-point
            //        orthPoint = _primaryPlayer.GetPointOrthogonalToLine(Ball.Instance.NormalizedPosition,
            //            (Vector3)_primaryPlayer.PassTarget,
            //            _oppositionPlayer.Position);
            //    }
            //    else
            //    {
            //        _lineRenderer001.material.color = Color.red;

            //        //draw line to orth-point
            //        orthPoint = _primaryPlayer.GetPointOrthogonalToLine(Ball.Instance.NormalizedPosition,
            //            _secondaryPlayer.Position,
            //            _oppositionPlayer.Position);
            //    }

            //    //draw line 2
            //    _lineRenderer002.SetPositions(new Vector3[]
            //    {
            //        _oppositionPlayer.Position,
            //        orthPoint
            //    });
            //}

            //TestLogicToRetreiveOrthogonalPoint();
            //TestIfPassIsSafeFromOpponent(_oppositionPlayer);
            //TestIsPassSafeFromAllOpponents();
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer.Init(30f, 5f, 3, 5f, 30f, 5f);
            _primaryPlayer.Init();

            //init secondary player
            //_secondaryPlayer.Init(30f, 5f, 3, 5f, 30f, 5f);
            _secondaryPlayer.Init();

            ActionUtility.Invoke_Action(InstructToWait);
            _primaryPlayer.InFieldPlayerFSM.ChangeState<ControlBallMainState>();

            canRun = true;
        }

        public void TestIsPassSafeFromAllOpponents()
        {
            foreach(Player opponent in _primaryPlayer.OppositionMembers)
            {
                TestIfPassIsSafeFromOpponent(opponent);
            }
        }

        public void TestIfPassIsSafeFromOpponent(Player opponent)
        {
            Vector3 position = _secondaryPlayer.Position + _secondaryPlayer.transform.forward * 5f;

            //
            float initialVel = 15f;
            float time = _primaryPlayer.TimeToTarget(Ball.Instance.NormalizedPosition,
                position,
                initialVel,
                5f);

            //test if pass is safe
            if(_primaryPlayer.IsPassSafeFromOpponent(Ball.Instance.NormalizedPosition,
                position,
                opponent.Position,
                _secondaryPlayer.Position,
                initialVel,
                time))
            {
                _lineRenderer001.material.color = Color.green;
            }
            else
            {
                _lineRenderer001.material.color = Color.red;
            }

            //draw lines
            TestLogicToRetreiveOrthogonalPoint(position);
        }

        public void TestLogicToRetreiveOrthogonalPoint(Vector3 position)
        {
            //set position to target
            _lineRenderer001.SetPositions(new Vector3[]
            {
                    Ball.Instance.NormalizedPosition,
                    position
            });

            if (_primaryPlayer.IsPositionBetweenTwoPoints(Ball.Instance.NormalizedPosition, position, _oppositionPlayer.Position))
            {
                if (!_lineRenderer002.gameObject.activeInHierarchy)
                    _lineRenderer002.gameObject.SetActive(true);

                //draw line to orth-point
                Vector3 orthPoint = _primaryPlayer.GetPointOrthogonalToLine(Ball.Instance.NormalizedPosition,
                    position,
                    _oppositionPlayer.Position);

                //draw line 2
                _lineRenderer002.SetPositions(new Vector3[]
                {
                    _oppositionPlayer.Position,
                    orthPoint
                });
            }
            else
            {
                _lineRenderer002.gameObject.SetActive(false);
            }
        }
    }
}
