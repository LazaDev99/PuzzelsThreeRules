using System.Collections.Generic;
using Game.UI.Panels;
using Game.Utilities;
using UnityEngine;
using System.Linq;

namespace Game.Controllers
{
    /// <summary>
    /// Class responsible for managing panels in the game.
    /// Main responsibilities include showing and hiding panels.
    /// </summary>
    public class PanelsController : Singleton<PanelsController>
    {

        [SerializeField] private List<BasePanel> basePanels;
        [SerializeField] private GameObject panelHolder;
        private Dictionary<PanelType, BasePanel> panelMap;
        

        protected override void Awake()
        {
            base.Awake();

            if (!BuildPanelMap())
                Debug.Log("Failed to build panel map.");
        }



        /// <summary>
        /// This method is called to activate the panel holder and show the panel of the specified type.
        /// Call if you want to ensure the panel holder is active and ready for use.
        /// </summary>
        /// <param name="panelType"></param>
        public void TurnPanelOn(PanelType panelType)
        {
            if (!panelHolder.activeSelf)
                panelHolder.gameObject.SetActive(true);

            HideAllActivePanels();

            ShowPanel(panelType);
        }



        /// <summary>
        /// This method is called to deactivate the panel holder and hide the panel of the specified type.
        /// This is useful when you want to close the panel and hide the panel holder.
        /// </summary>
        public void TurnPanelOff()
        {
            HideAllActivePanels();

            if (panelHolder.activeSelf)
                panelHolder.gameObject.SetActive(false);
        }



        /// <summary>
        /// Displays a panels of the specified type.
        /// Call this method if you want to switch between different panels in the game.
        /// </summary>
        /// <param name="panelType"></param>
        public void ShowPanel(PanelType panelType)
        {
            if (panelMap.TryGetValue(panelType, out var panel))
                panel.Open();
                
        }



        /// <summary>
        /// Closes the panel of the specified type.
        /// Call this method if you want to switch between different panels in the game.
        /// </summary>
        /// <param name="panelType"></param>
        public void HidePanel(PanelType panelType)
        {
            if (panelMap.TryGetValue(panelType, out var panel))
                panel.Close();
        }




        /// <summary>
        /// Retrieves the currently active panel.
        /// This method is created because all panels are opened over MainMenu panel.
        /// </summary>
        /// <returns></returns>
        public BasePanel GetCurrentActivePanel()
        {
            return panelMap.Values.Last(d => d.gameObject.activeInHierarchy);
        }



        /// <summary>
        /// Checks if any panel is currently active.
        /// </summary>
        /// <returns></returns>
        public bool IsAnyPanelActive()
        {
            return panelHolder.gameObject.activeSelf;
        }



        /// <summary>
        /// Hides all currently active panels.
        /// </summary>
        private void HideAllActivePanels()
        {
            foreach (var panel in GetAllActivePanels())
            {
                panel.gameObject.SetActive(false);
            }
        }



        /// <summary>
        /// Gets a list of all currently active panels.
        /// </summary>
        /// <returns></returns>
        private List<BasePanel> GetAllActivePanels()
        {
            return panelMap.Values.Where(d => d.gameObject.activeSelf).ToList();
        }



        /// <summary>
        /// Fills the panelMap dictionary with panels from basePanels based on their PanelType.
        /// Activates each panel in the process, then deactivates them again, just to ensure they are initialized properly.
        /// </summary>
        /// <returns></returns>
        private bool BuildPanelMap()
        {
            if (panelMap != null || basePanels == null)
                return false;

            panelMap = new Dictionary<PanelType, BasePanel>();
            foreach (var panel in basePanels)
            {
                panel.gameObject.SetActive(true);
                panelMap[panel.panelType] = panel;
                panel.gameObject.SetActive(false);
            }
            return true;
        }
    }

}
