using System;
using UnityEngine;

namespace Minesweeper.Core.Data
{
    [Serializable]
    public class MinMaxValues
    {
        public Vector2Int MinMaxColumns;
        public Vector2Int MinMaxRows;
        public Vector2Int MinMaxMinesCount;

        public MinMaxValues(MinMaxValues other)
        {
            MinMaxColumns = new Vector2Int(other.MinMaxColumns.x, other.MinMaxColumns.y);
            MinMaxRows = new Vector2Int(other.MinMaxRows.x, other.MinMaxRows.y);
            MinMaxMinesCount = new Vector2Int(other.MinMaxMinesCount.x, other.MinMaxMinesCount.y);
        }
    }
}