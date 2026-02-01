using UnityEngine;

namespace Minesweeper.Core.Data
{
    public class GameSettings
    {
        public Vector2Int FieldSize;
        public int MinesCount;

        public static GameSettings CreateDefault()
        {
            return new GameSettings()
            {
                FieldSize = new Vector2Int(9, 9),
                MinesCount = 10,
            };
        }
    }
}