using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Football.Gameplay.Manager
{
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager instance;

        public int leftTeamScore;
        public int rightTeamScore;

        public TMP_Text leftSideScoreText;
        public TMP_Text rightSideScoreText;
        public TMP_Text timeText;
        public int initialTime = 120;
        public float timeSpeed = 0.5f;

        float time;

        int minutes;
        int seconds;

        public ParticleSystem confetti;

        private void Awake()
        {
            instance = this;

            time = initialTime;
            UpdateTimerText();
        }

        public void UpdateScore()
        {
            leftSideScoreText.text = leftTeamScore.ToString();
            rightSideScoreText.text = rightTeamScore.ToString();
        }

        public void Update()
        {
            time = Mathf.Clamp(time - (Time.time * timeSpeed), 0, float.MaxValue);
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            minutes = Mathf.FloorToInt(time / 60);
            seconds = Mathf.FloorToInt(time % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = timeString;
        }
    }
}
