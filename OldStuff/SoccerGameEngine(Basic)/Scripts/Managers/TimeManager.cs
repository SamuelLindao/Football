using Patterns.Singleton;
using System.Collections;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Managers
{
    public class TimeManager : Singleton<TimeManager>
    {
        /// <summary>
        /// A reference to the time update frequency of this instance
        /// </summary>
        public float TimeUpdateFrequency { get; set; }

        /// <summary>
        /// A reference to the played minutes in the game
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// A reference to the played seconds in the game
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Tick delegate
        /// </summary>
        /// <param name="minutes">the current minutes</param>
        /// <param name="seconds">the currenct seconds</param>
        public delegate void Tick(int minutes, int seconds);

        /// <summary>
        /// The OnTick event
        /// </summary>
        public Tick OnTick;

        /// <summary>
        /// Ticks the time
        /// </summary>
        /// <returns></returns>
        public IEnumerator TickTime()
        {
            while (true)
            {
                //wait for the time update frequency
                yield return new WaitForSeconds(TimeUpdateFrequency);

                //if seconds reaches 60
                //reset seconds, increment minutes
                if (Seconds >= 59)
                {
                    Seconds = 0;
                    ++Minutes;
                }
                else
                {
                    //increment seconds
                    ++Seconds;
                }

                //invoke the delegate
                OnTick.Invoke(Minutes, Seconds);
            }
        }

        /// <summary>
        /// Formatted representation of the current time
        /// </summary>
        public string FormattedGameTime
        {
            get
            {
                //prepare time
                string formattedTime = string.Format("{0}:{1}",
                    Minutes.ToString("00"),
                    Seconds.ToString("00"));

                //return result
                return formattedTime;
            }
        }
    }
}
