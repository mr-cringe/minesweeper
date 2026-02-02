using System;
using UnityEngine;

namespace Minesweeper.Core.Data
{
    [Serializable]
    public class GameSettings
    {
        public Vector2Int FieldSize;
        public int MinesCount;
        public bool RevealNumbersOnGameOver;
        
        public GameSettings(GameSettings settings)
        {
            FieldSize = new Vector2Int(settings.FieldSize.x, settings.FieldSize.y);
            MinesCount = settings.MinesCount;
            RevealNumbersOnGameOver = settings.RevealNumbersOnGameOver;
        }
    }
}