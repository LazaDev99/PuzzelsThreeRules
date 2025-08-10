using UnityEngine.SceneManagement;
using Game.PuzzleGame.Managers;
using Game.Controllers;
using Game.Utilities;
using UnityEngine.UI;
using UnityEngine;


namespace Game.UI.Panels
{
    /// <summary>
    /// Class representing the pause panel in the game.
    /// Inherits from BasePanel and provides functionality for the pause menu.
    /// </summary>
    public class PausePanel : BasePanel
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button resumeButton;
        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Pause;

            // Button listeners
            homeButton.onClick.AddListener(() => HomeButton(SceneIndexes.EntryUI));
            settingsButton.onClick.AddListener(() =>
            {
                AudioController.Instance.PlaySound(clickSFX);
                PanelsController.Instance.ShowPanel(PanelType.Settings);
            });
            resumeButton.onClick.AddListener(() =>
            {
                AudioController.Instance.PlaySound(clickSFX);
                GameManager.Instance.ResumeGame();
            });
        }

        private void HomeButton(SceneIndexes sceneIndex)
        {
            AudioController.Instance.PlaySound(clickSFX);
            SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
        }
    }
}
