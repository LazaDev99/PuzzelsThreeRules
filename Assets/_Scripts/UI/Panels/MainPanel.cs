using Game.Controllers;
using UnityEngine.UI;
using UnityEngine;


namespace Game.UI.Panels
{
    /// <summary>
    /// Class representing the main panel in the game.
    /// Inherits from BasePanel and provides functionality for the main menu.
    /// </summary>
    public class MainPanel : BasePanel
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button statsButton;
        [SerializeField] private Button quitButton;
        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Main;

            // Button listeners
            playButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.ShowPanel(PanelType.Rules);
                AudioController.Instance.PlaySound(clickSFX);
            });

            settingsButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.ShowPanel(PanelType.Settings);
                AudioController.Instance.PlaySound(clickSFX);
            });

            statsButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.ShowPanel(PanelType.Stats);
                AudioController.Instance.PlaySound(clickSFX);
            });

            quitButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.ShowPanel(PanelType.Quit);
                AudioController.Instance.PlaySound(clickSFX);
            });
        }
    }
}

