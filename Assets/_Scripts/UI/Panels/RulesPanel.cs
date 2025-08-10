using UnityEngine.SceneManagement;
using Game.Controllers;
using UnityEngine.UI;
using Game.Utilities;
using UnityEngine;


namespace Game.UI.Panels
{
    /// <summary>
    /// Class is responsible for allowing the player to choose between different mini-games.
    /// Inherits from BasePanel to utilize common panel functionality.
    /// </summary>
    public class RulesPanel : BasePanel
    {
        [SerializeField] private Button miniGame2DButton;
        [SerializeField] private Button closeButton;


        protected override void Awake()
        {
            base.Awake();
            panelType = PanelType.Rules;

            // Button listeners
            miniGame2DButton.onClick.AddListener(() => LoadMiniGame(SceneIndexes.Game));
            closeButton.onClick.AddListener(() =>
            {
                PanelsController.Instance.HidePanel(panelType);
                AudioController.Instance.PlaySound(clickSFX);
            });
        }


        private void LoadMiniGame(SceneIndexes sceneIndex)
        {
            AudioController.Instance.PlaySound(clickSFX);
            SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
        }
    }
}