using Game.Controllers;
using UnityEngine.UI;
using UnityEngine;

namespace Game.UI.Panels
{
    /// <summary>
    /// Class representing the quit panel in the game.
    /// Inherits from BasePanel and provides functionality for quitting the game.
    /// </summary>
    public class QuitPanel : BasePanel
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button closeButton;

        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Quit;

            // Button listeners
            yesButton.onClick.AddListener(OnYesButtonClicked);

            noButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.HidePanel(panelType);
                AudioController.Instance.PlaySound(clickSFX);
            });

            closeButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.HidePanel(panelType);
                AudioController.Instance.PlaySound(clickSFX);
            });
        }

        private void OnYesButtonClicked()
        {
            AudioController.Instance.PlaySound(clickSFX);
            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
