using UnityEngine;

namespace Game.PuzzleGame.Tiles
{
    /// <summary>
    /// Provides static positions for tiles in the game.
    /// </summary>
    public static class TileStaticPositions
    {
        // Left Up Group
        public static readonly Vector2 LEFT_UP_1 = new Vector2(-3.76f, 3.18f);
        public static readonly Vector2 LEFT_UP_2 = new Vector2(-1.88f, 3.18f);  
        public static readonly Vector2 LEFT_UP_3 = new Vector2(-0.93f, 1.59f); 
        public static readonly Vector2 LEFT_UP_4 = new Vector2(-1.88f, -0.03f);  
        public static readonly Vector2 LEFT_UP_5 = new Vector2(-3.76f, -0.03f);  
        public static readonly Vector2 LEFT_UP_6 = new Vector2(-4.69f, 1.59f);  

        // Right Up Group
        public static readonly Vector2 RIGHT_UP_1 = new Vector2(1.89f, 3.18f); 
        public static readonly Vector2 RIGHT_UP_2 = new Vector2(3.77f, 3.18f);
        public static readonly Vector2 RIGHT_UP_3 = new Vector2(4.72f, 1.59f);
        public static readonly Vector2 RIGHT_UP_4 = new Vector2(3.77f, -0.03f);
        public static readonly Vector2 RIGHT_UP_5 = new Vector2(1.89f, -0.03f);
        public static readonly Vector2 RIGHT_UP_6 = new Vector2(0.96f, 1.59f);

        // Left Down Gorup
        public static readonly Vector2 LEFT_DOWN_3 = new Vector2(-0.93f, -1.59f);
        public static readonly Vector2 LEFT_DOWN_4 = new Vector2(-1.88f, -3.18f);
        public static readonly Vector2 LEFT_DOWN_5 = new Vector2(-3.76f, -3.18f);
        public static readonly Vector2 LEFT_DOWN_6 = new Vector2(-4.69f, -1.59f);

        // Right Down Group
        public static readonly Vector2 RIGHT_DOWN_3 = new Vector2(4.72f, -1.59f);
        public static readonly Vector2 RIGHT_DOWN_4 = new Vector2(3.77f, -3.18f);
        public static readonly Vector2 RIGHT_DOWN_5 = new Vector2(1.89f, -3.18f);
        public static readonly Vector2 RIGHT_DOWN_6 = new Vector2(0.96f, -1.59f);

    }
}