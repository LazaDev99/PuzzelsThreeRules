using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


namespace Game.Utilities
{
    public class LoadingScene : StaticInstance<LoadingScene>
    {
        [SerializeField] private Slider bar;
        [SerializeField] private Button continueButton;
        private AsyncOperation operation;

        protected override void Awake()
        {
            continueButton.onClick.AddListener(() => operation.allowSceneActivation = true);
        }
        void Start()
        {
            continueButton.gameObject.SetActive(false);
            StartCoroutine(LoadSceneAsync(SceneIndexes.EntryUI));
        }

        IEnumerator LoadSceneAsync(SceneIndexes index)
        {
            operation = SceneManager.LoadSceneAsync((int)index);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                bar.value = progress;
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            bar.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
        }
    }
}
