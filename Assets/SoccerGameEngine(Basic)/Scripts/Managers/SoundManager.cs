using System;
using Patterns.Singleton;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource _ballKickAS;

        public AudioSource _goalAS;

        public AudioSource _matchAmbience;

        public void PlayBallKickedSound(float flightTime, float velocity, Vector3 initial, Vector3 target)
        {
            _ballKickAS.Play();
        }

        public void PlayGoalScoredSound(string message)
        {
            _goalAS.Play();
        }
    }
}
