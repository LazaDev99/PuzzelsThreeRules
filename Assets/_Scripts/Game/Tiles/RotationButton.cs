using Game.PuzzleGame.Managers;
using UnityEngine;

namespace Game.PuzzleGame.Tiles
{
    /// <summary>
    /// This class handles the interaction with the tile rotation buttons.
    /// When clicked, it plays a sound and rotates the tiles in the specified group.
    /// </summary>
    public class RotationButton : MonoBehaviour
    {
        [SerializeField] private TileRotationGroup groupID;

        private void OnMouseDown()
        {
            GameManager.Instance.Rotate(groupID);
        }
    }
}
