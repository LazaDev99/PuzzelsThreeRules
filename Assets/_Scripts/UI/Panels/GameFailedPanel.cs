using System;
using Game.Controllers;
using Game.PuzzleGame.Managers;
using Game.UI.Panels;
using Game.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFailedPanel : BasePanel
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button rulesButton;


    protected override void Awake()
    {
        base.Awake();
        panelType = PanelType.GameFailed;

        // Button listeners
        homeButton.onClick.AddListener(() => HomeButton(SceneIndexes.EntryUI));
        rulesButton.onClick.AddListener(() => PanelsController.Instance.ShowPanel(PanelType.Rules));
        playAgainButton.onClick.AddListener(() =>
        {
            AudioController.Instance.PlaySound(clickSFX);
            GameManager.Instance.RestartGame();
        });
        
    }

    private void HomeButton(SceneIndexes sceneIndex)
    {
        AudioController.Instance.PlaySound(clickSFX);
        SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
    }
}
