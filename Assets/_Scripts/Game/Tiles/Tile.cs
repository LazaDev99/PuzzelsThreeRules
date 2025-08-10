using System.Collections;
using Game.PuzzleGame.Managers;
using UnityEngine;

namespace Game.PuzzleGame.Tiles
{
    /// <summary>
    /// Represents a tile in the game world, providing functionality to manage its position and type.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        private Vector2 position;
        private static float animationDuration = 0.4f;
        [SerializeField] private TileType tileType;
        [SerializeField] private Vector2 startPosition;


        /// <summary>
        /// Initialize position of tile. 
        /// Important if we create more levels.
        /// </summary>
        public void Initialize()
        {
            position = startPosition;
            transform.localPosition = new Vector2(startPosition.x, startPosition.y);
        }



        /// <summary>
        /// Method to return position of tile.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            position = transform.localPosition;
            return position;
        }



        /// <summary>
        /// Method to set new tile position.
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition)
        {
           StartCoroutine(MoveToPosition(newPosition));
        }



        /// <summary>
        /// Coroutine responsible to move tile on next position (target).
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerator MoveToPosition(Vector2 target)
        {
            Vector3 start = transform.localPosition;
            Vector3 end = new Vector3(target.x, target.y, transform.localPosition.z);
            float elapsed = 0f;

            while (elapsed < animationDuration)
            {
                transform.localPosition = Vector3.Lerp(start, end, elapsed / animationDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = end;
            GameManager.Instance.AllowRotation();
        }



        /// <summary>
        /// Method which return type of tile. 
        /// Used to check if tile is on right position.
        /// </summary>
        /// <returns></returns>
        public TileType GetTileType()
        {
            return tileType;
        }
    }
}
