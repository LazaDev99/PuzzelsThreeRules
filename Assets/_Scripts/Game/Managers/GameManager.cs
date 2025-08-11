using System.Collections.Generic;
using Game.PuzzleGame.Tiles;
using System.Collections;
using Game.Controllers;
using Game.Scriptables;
using Game.UI.Panels;
using Game.Utilities;
using UnityEngine;
using System.Linq;


namespace Game.PuzzleGame.Managers
{
    /// <summary>
    /// Class responsible for managing the puzzle game
    /// Main responsibilities include initializing tiles, rotating groups of tiles, and checking game completion.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        private Dictionary<TileRotationGroup, List<Vector2>> groupReferencePositions = new Dictionary<TileRotationGroup, List<Vector2>>
        {
            { TileRotationGroup.LEFT_UP, new List<Vector2>() { TileStaticPositions.LEFT_UP_1, TileStaticPositions.LEFT_UP_2, TileStaticPositions.LEFT_UP_3, TileStaticPositions.LEFT_UP_4, TileStaticPositions.LEFT_UP_5, TileStaticPositions.LEFT_UP_6}},
            { TileRotationGroup.RIGHT_UP, new List<Vector2>() { TileStaticPositions.RIGHT_UP_1, TileStaticPositions.RIGHT_UP_2, TileStaticPositions.RIGHT_UP_3, TileStaticPositions.RIGHT_UP_4, TileStaticPositions.RIGHT_UP_5, TileStaticPositions.RIGHT_UP_6}},
            { TileRotationGroup.CENTER, new List<Vector2>() { TileStaticPositions.LEFT_UP_3, TileStaticPositions.RIGHT_UP_6, TileStaticPositions.RIGHT_UP_5, TileStaticPositions.RIGHT_DOWN_6, TileStaticPositions.LEFT_DOWN_3, TileStaticPositions.LEFT_UP_4}},
            { TileRotationGroup.LEFT_DOWN, new List<Vector2>() { TileStaticPositions.LEFT_UP_5, TileStaticPositions.LEFT_UP_4, TileStaticPositions.LEFT_DOWN_3, TileStaticPositions.LEFT_DOWN_4, TileStaticPositions.LEFT_DOWN_5, TileStaticPositions.LEFT_DOWN_6}},
            { TileRotationGroup.RIGHT_DOWN, new List<Vector2>() { TileStaticPositions.RIGHT_UP_5, TileStaticPositions.RIGHT_UP_4, TileStaticPositions.RIGHT_DOWN_3, TileStaticPositions.RIGHT_DOWN_4, TileStaticPositions.RIGHT_DOWN_5, TileStaticPositions.RIGHT_DOWN_6} }
        };

        [SerializeField] List<Tile> tiles;
        [SerializeField] private AudioData SFXAudio;
        private bool rotationInProgress = false;
        private bool gamePaused = false;
        private const float ROTATION_DELAY = 0.5f;
        private const float POSITION_TOLERANCE = 0.1f;



        protected override void Awake()
        {
            base.Awake();
            InitializeTiles();
        }



        /// <summary>
        /// Function to rotate the tiles in a specific group.
        /// Fist we get the tiles in the specified group, then we rotate them by moving the last tile to the front.
        /// Than we update the positions of the tiles in the group with the new positions.
        /// At the end we check if the game is completed after the rotation.
        /// </summary>
        /// <param name="groupId"></param>
        public void Rotate(TileRotationGroup groupId)
        {
            if (PanelsController.Instance.IsAnyPanelActive() || rotationInProgress)
                return;

            List<Tile> groupTiles = GetTilesInGroup(groupId);
            if (groupTiles.Count == 0)
                return;

            if (!TimerManager.Instance.IsTimerRunning())
                TimerManager.Instance.StartTimer();

            rotationInProgress = true;

            AudioController.Instance.PlaySound(SFXAudio);

            List<Vector2> positions = groupTiles.Select(tile => tile.GetPosition()).ToList();
            Vector2 lastPosition = positions[positions.Count - 1];
            
            for (int i = positions.Count - 1; i > 0; i--)
            {
                positions[i] = positions[i - 1];
            }
            positions[0] = lastPosition;

            for (int i = 0; i < groupTiles.Count; i++)
            {
                groupTiles[i].SetPosition(positions[i]);
            }

            RotationCountManager.Instance.RotationHappend();

            StartCoroutine(CheckGameCompletion());
        }



        /// <summary>
        /// Function to restart the game. Called when the player wants to start over in the game completed panel.
        /// Inirtializes the tiles again, resetting their positions.
        /// Resets the timer and hides the game completed panel.
        /// </summary>
        public void RestartGame()
        {
            InitializeTiles();
            TimerManager.Instance.ResetTimer();
            RotationCountManager.Instance.SetRotationCount();
            PanelsController.Instance.TurnPanelOff();
        }



        /// <summary>
        /// Resumes the game by checking if any panel is active and turning it off.
        /// Resumes the timer to continue the game.
        /// </summary>
        public void ResumeGame()
        {
            if (PanelsController.Instance.IsAnyPanelActive())
                PanelsController.Instance.TurnPanelOff();

            gamePaused = false;
            TimerManager.Instance.ResumeTimer();
        }



        /// <summary>
        /// Pauses the game by stopping the timer and showing the pause panel.
        /// </summary>
        public void PauseGame()
        {
            if (gamePaused)
                return;

            gamePaused = true;
            TimerManager.Instance.StopTimer();
            PanelsController.Instance.TurnPanelOn(PanelType.Pause);
        }



        /// <summary>
        /// Getter and setter for the rotationInProgress variable.
        /// Important to prevent multiple rotations from being triggered at the same time.
        /// </summary>
        /// <returns></returns>
        public void AllowRotation()
        {
            if (rotationInProgress)
                rotationInProgress = false;
        }



        /// <summary>
        /// Method which control if player fail to complete game
        /// </summary>
        public void GameOver()
        {
            TimerManager.Instance.StopTimer();
            PanelsController.Instance.TurnPanelOn(PanelType.GameFailed);
        }



        /// <summary>
        /// Method which ensure all tiles are initialized and have valid positions. 
        /// </summary>
        private void InitializeTiles()
        {
            if (tiles == null || tiles.Count == 0)
                return;


            foreach (Tile tile in tiles)
            {
                tile.Initialize();
            }
        }



        /// <summary>
        /// Method to control game when it's completed.
        /// Responsible for Timer, Panels and GameStats
        /// </summary>
        private void GameCompleted()
        {
            TimerManager.Instance.StopTimer();
            GameStatsController.Instance.UpdateGameStats(TimerManager.Instance.GetElapsedTime(), RotationCountManager.Instance.GetRotationsCount());
            PanelsController.Instance.TurnPanelOn(PanelType.GameCompleted);
        }



        /// <summary>
        /// Method to get tiles in a specific group based on their reference positions.
        /// Matches tiles to their closest reference position in the group's order.
        /// <summary>
        /// <param name="groupId"></param>
        private List<Tile> GetTilesInGroup(TileRotationGroup groupId)
        {
            if (!groupReferencePositions.ContainsKey(groupId))
            {
                Debug.Log($"Group ID {groupId} does not exist in groupReferencePositions.");
                return new List<Tile>();
            }

            List<Tile> groupTiles = new List<Tile>();
            List<Vector2> referencePositions = groupReferencePositions[groupId];

            foreach (Vector2 referencePos in referencePositions)
            {
                Tile matchedTile = tiles.FirstOrDefault(tile => tile != null && Vector2.Distance(tile.GetPosition(), referencePos) < POSITION_TOLERANCE);

                if (matchedTile != null)
                    groupTiles.Add(matchedTile);
            }

            return groupTiles;
        }



        /// <summary>
        /// Return tile based on provided position.
        /// Important for CheckGroupPositions function
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Tile GetTileAtPosition(Vector2 position)
        {
            return tiles.FirstOrDefault(tile => tile != null && Vector2.Distance(tile.GetPosition(), position) < POSITION_TOLERANCE);
        }



        /// <summary>
        /// This method is called to check types of tiles on key positions of rotation group.
        /// If their types are equal as required -> That rotation group is finished successfully.
        /// </summary>
        /// <param name="rotationGroup"></param>
        /// <param name="keyPositions"></param>
        /// <param name="requiredType"></param>
        /// <returns></returns>
        private bool CheckGroupPositions(TileRotationGroup rotationGroup, int[] keyPositions, TileType requiredType)
        {
            var positions = groupReferencePositions[rotationGroup];

            foreach (int index in keyPositions)
            {
                var tile = GetTileAtPosition(positions[index]);
                if (tile != null && tile.GetTileType() != requiredType)
                {
                    return false;
                }
            }

            return true;
        }



        /// <summary>
        /// Check if the game is completed by getting the tiles in each group and verifying their types.
        /// If all groups are valid, the game is considered completed, than we show the game completed panel.
        ///</summary>
        IEnumerator CheckGameCompletion()
        {
            yield return new WaitForSeconds(ROTATION_DELAY); // Wait for the end of the frame to ensure all tile positions are updated


            int[] leftUpKeyPositions = new int[] { 0, 1, 5 };
            bool lefUpDone = CheckGroupPositions(TileRotationGroup.LEFT_UP, leftUpKeyPositions, TileType.RUG);

            int[] rightUpKeyPositions = new int[] { 0, 1, 2 };
            bool rightUpDone = CheckGroupPositions(TileRotationGroup.RIGHT_UP, rightUpKeyPositions, TileType.CAMEL);
            if (rightUpDone && !lefUpDone)
            {
                GameOver();
                yield break;
            }

            int[] leftDownKeyPositions = new int[] { 3, 4, 5 };
            bool leftDownDone = CheckGroupPositions(TileRotationGroup.LEFT_DOWN, leftDownKeyPositions, TileType.SNAKE);
            if(leftDownDone && !lefUpDone)
            {
                GameOver();
                yield break;
            }

            int[] rightDownKeyPositions = new int[] { 2, 3, 4 };
            bool rightDownDone = CheckGroupPositions(TileRotationGroup.RIGHT_DOWN, rightDownKeyPositions, TileType.LAMP);
            if(rightDownDone && !lefUpDone)
            {
                GameOver();
                yield break;
            }

            if (lefUpDone && rightUpDone && leftDownDone && rightDownDone)
                GameCompleted();
        }
    }
}

