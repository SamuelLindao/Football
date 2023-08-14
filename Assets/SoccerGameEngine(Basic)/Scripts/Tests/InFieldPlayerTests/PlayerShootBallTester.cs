using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerShootBallTester : MonoBehaviour
    {
        public Goal _goal;
        public LineRenderer _lineRenderer001;
        public LineRenderer _lineRenderer002;
        public Player _primaryPlayer;
        public Transform _shotTargetReferralPoint;

        private void Awake()
        {
            _primaryPlayer.PlaceBallInfronOfMe();
        }

        private void Start()
        {
            _shotTargetReferralPoint.position = _goal.ShotTargetReferencePoint;
            Invoke("Init", 1f);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(_primaryPlayer.CanScore())
                {
                    Debug.Log("Can score");
                    _lineRenderer001.gameObject.SetActive(true);
                    _lineRenderer001.SetPositions(new Vector3[]
                    {
                        _primaryPlayer.Position,
                        (Vector3)_primaryPlayer.KickTarget
                    });
                }
                else
                {
                    Debug.Log("Can't score");
                    _lineRenderer001.gameObject.SetActive(false);
                }
            }
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer.Init(15f, 5f, 15f, 5f, 30f, 5f);
            _primaryPlayer.Init();
        }
    }
}
