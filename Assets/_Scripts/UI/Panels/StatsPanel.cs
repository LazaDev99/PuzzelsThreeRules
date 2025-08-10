using Game.Controllers;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Game.UI.Panels
{
    /// <summary>
    /// Class representing the stats panel in the game.
    /// Inherits from BasePanel and provides functionality for displaying and resetting game statistics.
    /// </summary>
    public class StatsPanel : BasePanel
    {
        [SerializeField] private Button resetStatsButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI bestTime;
        [SerializeField] private TextMeshProUGUI worstTime;
        [SerializeField] private TextMeshProUGUI rotationsBT;
        [SerializeField] private TextMeshProUGUI rotationsWT;
        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Stats;

            // Button listeners
            resetStatsButton.onClick.AddListener(() => ResetStats());
            closeButton.onClick.AddListener(() => 
            { 
                PanelsController.Instance.HidePanel(panelType); 
                AudioController.Instance.PlaySound(clickSFX); 
            });
        }

        public override void Open()
        {
            base.Open();
            SetStats();
        }

        private void SetStats()
        {
            float time = GameStatsController.Instance.GetBestTime();
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            if (time != float.MaxValue)
                bestTime.text = $"{minutes:00}:{seconds:00}";
            else
                bestTime.text = $"{00:00}:{00:00}";

            time = GameStatsController.Instance.GetWorstTime();
            minutes = Mathf.FloorToInt(time / 60);
            seconds = Mathf.FloorToInt(time % 60);
            worstTime.text = $"{minutes:00}:{seconds:00}";

            int rotationsCount = GameStatsController.Instance.GetBestTimeRotations();
            int tens = rotationsCount / 10;
            int ones = rotationsCount % 10;
            rotationsBT.text = $"{tens}{ones}";

            rotationsCount = GameStatsController.Instance.GetWorstTimeRotations();
            tens = rotationsCount / 10;
            ones = rotationsCount % 10;
            rotationsWT.text = $"{tens}{ones}";
        }

        private void ResetStats()
        {
            AudioController.Instance.PlaySound(clickSFX);
            GameStatsController.Instance.ResetGameStats();
            SetStats();
        }
    }
}
