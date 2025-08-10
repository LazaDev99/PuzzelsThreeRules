using UnityEngine.SceneManagement;
using Game.PuzzleGame.Managers;
using Game.UI.Panels;
using Game.Utilities;
using UnityEngine;
using Unity.VisualScripting;



namespace Game.Controllers
{
    /// <summary>
    /// Responsible for handling input related to panel interactions.
    /// It listens for the Escape key to hide panels or show the quit panel.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex == (int)SceneIndexes.EntryUI)
                {
                    var controller = PanelsController.Instance;
                    var currentPanel = controller.GetCurrentActivePanel();
                    if (currentPanel != null)
                    {
                        if (currentPanel.panelType == PanelType.Main)
                            controller.ShowPanel(PanelType.Quit);
                        else
                            controller.HidePanel(currentPanel.panelType);
                    }
                }
                else if (SceneManager.GetActiveScene().buildIndex == (int)SceneIndexes.Game)
                {
                    var controller = PanelsController.Instance;
                    if (!controller.IsAnyPanelActive())
                        GameManager.Instance.PauseGame();
                }
            }
        }
    }
}
