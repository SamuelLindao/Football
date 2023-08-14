using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerPassBallBasic : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;
        public Player _oppositionPlayer;
        public GameObject _gameObject;
        public Transform _passTarget;

        private void Awake()
        {
           // _primaryPlayer.Init(15f, 5f, 3f, 5f, 30f, 5f);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                bool canPass = _primaryPlayer.IsPassSafeFromOpponent(_primaryPlayer.Position,
                    _secondaryPlayer.Position,
                    _oppositionPlayer.Position,
                    _secondaryPlayer.Position,
                    20f,
                    _primaryPlayer.TimeToTarget(_primaryPlayer.Position,
                    _secondaryPlayer.Position,
                    20f,
                    Ball.Instance.Friction));

               // bool canPass = _primaryPlayer.CanPass();

                if (canPass)
                {
                    _passTarget.position = _secondaryPlayer.Position;
                    _passTarget.gameObject.SetActive(true);
                    _gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    _passTarget.gameObject.SetActive(false);
                    _gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }

                Debug.Log(string.Format("Can pass: {0}", canPass));
            }
        }
    }
}
