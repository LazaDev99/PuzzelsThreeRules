using Game.Utilities;
using TMPro;
using UnityEngine;


namespace Game.PuzzleGame.Managers
{
    /// <summary>
    /// Class responsible to control number of rotations and update UI
    /// </summary>
    public class RotationCountManager : Singleton<RotationCountManager>
    {

        private int rotationsLeft;
        [SerializeField] private TextMeshProUGUI rotationText;
        [SerializeField] private int allowedRotationCount = 50;


        /// <summary>
        /// Set up allowed rotations on start.
        /// </summary>
        public void SetRotationCount()
        {
            rotationsLeft = allowedRotationCount;
            UpdateUI();
        }



        /// <summary>
        /// Return num of rotations left.
        /// </summary>
        /// <returns></returns>
        public int GetRotationsLeft()
        {
            return rotationsLeft;
        }



        /// <summary>
        /// Method returns number of rotations / clicks
        /// </summary>
        /// <returns></returns>
        public int GetRotationsCount()
        {
            return allowedRotationCount - rotationsLeft;
        }



        /// <summary>
        /// Called when rotation happend to decrease left rotations.
        /// </summary>
        public void RotationHappend()
        {
            rotationsLeft--;
            UpdateUI();

            if (rotationsLeft == 0)
            {
                GameManager.Instance.GameOver();
                return;
            }
        }



        /// <summary>
        /// Method responsible for rotations UI 
        /// </summary>
        private void UpdateUI()
        {
            if (rotationText == null)
                return;

            int tens = rotationsLeft / 10;
            int ones = rotationsLeft % 10;
            rotationText.text = $"{tens}{ones}";
        }
    }
}
