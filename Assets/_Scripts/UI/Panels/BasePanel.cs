using System.Collections;
using Game.Scriptables;
using UnityEngine;


namespace Game.UI.Panels
{
    /// <summary>
    /// Base class for all panels types in the game.
    /// This class provides common functionality for panels, such as opening and closing animations.
    /// </summary>

    public class BasePanel : MonoBehaviour
    {
        protected Animator animator;
        [SerializeField] public AudioData clickSFX;
        public PanelType panelType;


        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            
            if (animator == null)
                animator = gameObject.AddComponent<Animator>();
        }



        /// <summary>
        /// Controler for opening the panel with animation.
        /// </summary>
        public virtual void Open()
        {
            gameObject.SetActive(true);
            animator?.Play("PanelOpen");
        }



        /// <summary>
        /// Controler for closing the panel with delay.
        /// </summary>
        public virtual void Close()
        {
            animator?.Play("PanelClose");
            StartCoroutine(DelayedClose());
        }
        private IEnumerator DelayedClose()
        {
            yield return new WaitForSeconds(0.4f);
            gameObject.SetActive(false);
        }

    }
}
