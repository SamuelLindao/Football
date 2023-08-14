using Assets.SoccerGameEngine_Basic_.Scripts.Entities;
using Assets.SoccerGameEngine_Basic_.Scripts.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Managers
{
    /// <summary>
    /// Manages the entire game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        HalfTimePanel _halfTimePanel;

        [SerializeField]
        MainPanel _mainPanel;

        [SerializeField]
        MatchInfoPanel _matchInfoPanel;

        [SerializeField]
        MatchOverPanel _matchOverPanel;

        [SerializeField]
        MatchOnPanel _matchOnPanel;

        /// <summary>
        /// Event raised when continuing to second half
        /// </summary>
        public Action OnContinueToSecondHalf;

        /// <summary>
        /// Event raised when switching to match on
        /// </summary>
        public Action OnMessageSwitchToMatchOn;

        private void Awake()
        {
            // register the game manager to some events
            Ball.Instance.OnBallLaunched += SoundManager.Instance.PlayBallKickedSound;
            MatchManager.Instance.OnGoalScored += SoundManager.Instance.PlayGoalScoredSound;

            //register managers to listen to me
            OnContinueToSecondHalf += MatchManager.Instance.Instance_OnContinueToSecondHalf;
            OnMessageSwitchToMatchOn += MatchManager.Instance.Instance_OnMessagedSwitchToMatchOn;

            //listen to match manager events
            MatchManager.Instance.OnBroadcastHalfStart += Instance_OnBroadcastHalfStart;
            MatchManager.Instance.OnBroadcastMatchStart += Instance_OnBroadcastMatchStart;
            MatchManager.Instance.OnEnterHalfTime += Instance_OnEnterHalfTime;
            MatchManager.Instance.OnEnterWaitForMatchOnInstruction += Instance_OnEnterWaitForMatchOnInstruction;
            MatchManager.Instance.OnExitHalfTime += Instance_OnExitHalfTime;
            MatchManager.Instance.OnExitMatchOver += Instance_OnExitMatchOver;
            MatchManager.Instance.OnExitWaitForMatchOnInstruction += Instance_OnExitWaitForMatchOnInstruction;
            MatchManager.Instance.OnFinishBroadcastHalfStart += _Instance_OnFinishBroadcastHalfStart;
            MatchManager.Instance.OnFinishBroadcastMatchStart += Instance_OnFinishBroadcastMatchStart;
            MatchManager.Instance.OnGoalScored += Instance_OnGoalScored;
            MatchManager.Instance.OnMatchOver += Instance_OnMatchOver;
            MatchManager.Instance.OnMatchPlayStart += Instance_OnMatchPlayStart;
            MatchManager.Instance.OnMatchPlayStop += Instance_OnMatchPlayStop;
            MatchManager.Instance.OnTick += Instance_OnTick;
        }

        private void Instance_OnBroadcastHalfStart(string message)
        {
            ShowInfoPanel(message);
        }

        private void Instance_OnBroadcastMatchStart(string message)
        {
            ShowInfoPanel(message);
        }

        private void Instance_OnEnterHalfTime(string message)
        {
            _halfTimePanel.TxtInfo.text = message;
            _halfTimePanel.Root.gameObject.SetActive(true);
        }

        private void Instance_OnEnterWaitForMatchOnInstruction()
        {
            _mainPanel.Root.gameObject.SetActive(true);
        }

        private void Instance_OnExitHalfTime()
        {
            _halfTimePanel.Root.gameObject.SetActive(false);
        }

        private void Instance_OnExitMatchOver()
        {
            _matchOverPanel.Root.gameObject.SetActive(false);
        }

        private void Instance_OnExitWaitForMatchOnInstruction()
        {
            _mainPanel.Root.gameObject.SetActive(false);
        }

        private void _Instance_OnFinishBroadcastHalfStart()
        {
            HideInfoPanel();
        }

        private void Instance_OnFinishBroadcastMatchStart()
        {
            HideInfoPanel();
        }

        private void Instance_OnGoalScored(string message)
        {
            //show the text
            _matchOnPanel.TxtScores.text = message;
        }

        private void Instance_OnMatchOver(string message)
        {
            _matchOverPanel.TxtInfo.text = message;
            _matchOverPanel.Root.gameObject.SetActive(true);
        }

        private void Instance_OnMatchPlayStart()
        {
            _matchOnPanel.Root.gameObject.SetActive(true);
        }

        private void Instance_OnMatchPlayStop()
        {
            _matchOnPanel.Root.gameObject.SetActive(false);
        }

        private void Instance_OnTick(int half, int minutes, int seconds)
        {
            //declare the string
            string timeInfo = string.Empty;

            //prepare the message
            string infoHalf = half == 1 ? "1st" : "2nd";

            timeInfo = string.Format("{0} {1}:{2}", 
                infoHalf, 
                minutes.ToString("00"), 
                seconds.ToString("00"));

            //set the ui
            _matchOnPanel.TxtTime.text = timeInfo;
        }

        private void HideInfoPanel()
        {
            _matchInfoPanel.Root.gameObject.SetActive(false);
        }

        public void Instance_OnContinueToSecondHalf()
        {
            ActionUtility.Invoke_Action(OnContinueToSecondHalf);
        }

        public void Instance_OnMessageSwitchToMatchOn()
        {
            ActionUtility.Invoke_Action(OnMessageSwitchToMatchOn);
        }

        /// <summary>
        /// Quits this game
        /// </summary>
        public void Quit()
        {
            //pasue the editor
#if UNITYEDITOR
            Debug.Break();
#endif
            //quit the game
            Application.Quit();

        }

        /// <summary>
        /// Restart this instance
        /// </summary>
        public void Restart()
        {
            //reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ShowInfoPanel(string message)
        {
            _matchInfoPanel.TxtInfo.text = message;
            _matchInfoPanel.Root.gameObject.SetActive(true);
        }
    }

    [Serializable]
    public struct HalfTimePanel
    {
        public Text TxtInfo;

        public Transform Root;
    }

    [Serializable]
    public struct MainPanel
    {
        public Transform Root;
    }

    [Serializable]
    public struct MatchInfoPanel
    {
        public Text TxtInfo;

        public Transform Root;
    }

    [Serializable]
    public struct MatchOnPanel
    {
        public Text TxtScores;

        public Text TxtTime;

        public Transform Root;
    }

    [Serializable]
    public struct MatchOverPanel
    {
        public Text TxtInfo;

        public Transform Root;
    }
}
