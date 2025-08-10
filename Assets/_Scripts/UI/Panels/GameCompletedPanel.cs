using UnityEngine.SceneManagement;
using Game.PuzzleGame.Managers;
using Game.Controllers;
using Game.Utilities;
using UnityEngine.UI;
using UnityEngine;
using TMPro;




namespace Game.UI.Panels
{
    /// <summary>
    /// Class representing the game completed panel in the game.
    /// Inherits from BasePanel and provides functionality for the game completed state.
    /// </summary>
    public class GameCompletedPanel : BasePanel
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI rotations;

        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.GameCompleted;
            
            // Button listeners
            homeButton.onClick.AddListener(() => HomeButton(SceneIndexes.EntryUI));
            playAgainButton.onClick.AddListener(() =>
            {
                AudioController.Instance.PlaySound(clickSFX);
                GameManager.Instance.RestartGame();
            });
        }

        public override void Open()
        {
            base.Open();
            UpdateTimeAndRotationsUI();
        }

        private void UpdateTimeAndRotationsUI()
        {
            float timeElapsed = TimerManager.Instance.GetElapsedTime();
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            time.text = $"{minutes:00}:{seconds:00}";

            int rotationsCount = RotationCountManager.Instance.GetRotationsCount();
            int tens = rotationsCount / 10;
            int ones = rotationsCount % 10;
            rotations.text = $"{tens}{ones}";
        }

        private void HomeButton(SceneIndexes sceneIndex)
        {
            AudioController.Instance.PlaySound(clickSFX);
            SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
        }
    }
}
