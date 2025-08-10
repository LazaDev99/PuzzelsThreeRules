using System.Runtime.InteropServices.WindowsRuntime;
using Game.Utilities;
using UnityEngine;


namespace Game.Controllers
{
    /// <summary>
    /// Singleton that keeps track a player statistics game
    /// </summary>
    public class GameStatsController : Singleton<GameStatsController>
    {

        private const string PREFS_BEST_TIME = "BestTime";
        private const string PREFS_WORST_TIME = "WorstTime";
        private const string PREFS_BEST_TIME_ROTATIONS = "BestTimeRotations";
        private const string PREFS_WORST_TIME_ROTATIONS = "WorstTimeRotations";

        private float bestTime = float.MaxValue;
        private int bestTimeRotations;
        private float worstTime;
        private int worstTimeRotations;

        protected override void Awake()
        {
            base.Awake();
            InitializeStats();
        }



        /// <summary>
        /// Updates stats after a game session.
        /// </summary>
        public void UpdateGameStats(float elapsedTime, int rotationsCount)
        {
            // Best Time
            if (elapsedTime < bestTime)
            {
                bestTime = elapsedTime;
                bestTimeRotations = rotationsCount;
                PlayerPrefs.SetFloat(PREFS_BEST_TIME, bestTime);
                PlayerPrefs.SetInt(PREFS_BEST_TIME_ROTATIONS, bestTimeRotations);
            }

            // Worst Time
            if (elapsedTime > worstTime)
            {
                worstTime = elapsedTime;
                worstTimeRotations = rotationsCount;
                PlayerPrefs.SetFloat(PREFS_WORST_TIME, worstTime);
                PlayerPrefs.SetInt(PREFS_WORST_TIME_ROTATIONS, worstTimeRotations);
            }

            PlayerPrefs.Save();
        }



        /// <summary>
        /// Resets all stored statistics to initial values.
        /// </summary>
        public void ResetGameStats()
        {
            bestTime = float.MaxValue;
            worstTime = 0f;
            bestTimeRotations = 0;
            worstTimeRotations = 0;

            PlayerPrefs.SetFloat(PREFS_BEST_TIME, bestTime);
            PlayerPrefs.SetFloat(PREFS_WORST_TIME, worstTime);
            PlayerPrefs.SetInt(PREFS_BEST_TIME_ROTATIONS, bestTimeRotations);
            PlayerPrefs.SetInt(PREFS_WORST_TIME_ROTATIONS, worstTimeRotations);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// Getters
        /// </summary>
        /// <returns></returns>
        public float GetBestTime() => bestTime;
        public float GetWorstTime() => worstTime;
        public int GetBestTimeRotations() => bestTimeRotations;
        public int GetWorstTimeRotations() => worstTimeRotations;



        /// <summary>
        /// Loads saved statistics or sets defaults.
        /// </summary>
        private void InitializeStats()
        {
            bestTime = PlayerPrefs.GetFloat(PREFS_BEST_TIME, float.MaxValue);
            worstTime = PlayerPrefs.GetFloat(PREFS_WORST_TIME, 0f);
            bestTimeRotations = PlayerPrefs.GetInt(PREFS_BEST_TIME_ROTATIONS, 0);
            worstTimeRotations = PlayerPrefs.GetInt(PREFS_WORST_TIME_ROTATIONS, 0);      
        }
    }
}
