using Game.Utilities;
using UnityEngine;
using TMPro;


namespace Game.PuzzleGame.Managers
{
    /// <summary>
    /// This class is responsible for managing the time while playing the game.
    /// It can be used to start, stop, resume and reset the timer.
    /// </summary>
    public class TimerManager : Singleton<TimerManager>
    {
        [SerializeField] TextMeshProUGUI timer;
        [SerializeField] private float startTime = 50f;
        private float time;
        private bool startTimer = false;



        /// <summary>
        /// Control and update elapsed time
        /// </summary>
        private void Update()
        {
            if (startTimer)
            {
                time -= Time.deltaTime;
                CheckTime();
                UpdateTimerDisplay();
            }
        }



        /// <summary>
        /// Set up timer and display it well.
        /// </summary>
        public void SetUpTimer()
        {
            if (time != startTime)
                time = startTime;
            UpdateTimerDisplay();
        }



        /// <summary>
        /// Method responsible to set up timer and start it.
        /// </summary>
        public void StartTimer()
        {
            if (time != startTime)
                time = startTime;
            UpdateTimerDisplay();
            startTimer = true;
        }



        /// <summary>
        /// Stop, Resume and Reset methos are created for timer flow control.
        /// </summary>
        public void StopTimer()
        {
            startTimer = false;
        }
        public void ResumeTimer()
        {
            if (time == startTime)
                return;

            startTimer = true;
            UpdateTimerDisplay();
        }
        public void ResetTimer()
        {
            if (time != startTime)
                time = startTime;

            UpdateTimerDisplay();
        }



        /// <summary>
        /// Getters for elapsed time and check if timer is running
        /// </summary>
        /// <returns></returns>
        public float GetLeftTime()
        { return time; }
        public float GetElapsedTime()
        { return startTime - time; }
        public bool IsTimerRunning()
        { return startTimer; }

      

        /// <summary>
        /// Method responsible to update time displayed on board in game scene. 
        /// </summary>
        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timer.text = $"{minutes:00}:{seconds:00}";
        }



        /// <summary>
        /// Method called in update, responsible to check it time runs out.
        /// </summary>
        private void CheckTime()
        {
            if (time <= 0f)
            {
                StopTimer();
                time = 0f;
                GameManager.Instance.GameOver();
            }
        }
    }
}
