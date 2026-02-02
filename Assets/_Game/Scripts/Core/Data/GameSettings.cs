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
        
        public GameSettings(GameSettings other)
        {
            FieldSize = new Vector2Int(other.FieldSize.x, other.FieldSize.y);
            MinesCount = other.MinesCount;
            RevealNumbersOnGameOver = other.RevealNumbersOnGameOver;
        }
    }
}