namespace Minesweeper.Core.Data
{
    public class CellData
    {
        public CellState State;
        public readonly int MinesCountAround;
        
        public bool IsFlagged { get; private set; }
        public bool IsMined => MinesCountAround == -1;

        public CellData(int minesCountAround)
        {
            State = CellState.Closed;
            MinesCountAround = minesCountAround;
        }
    }
}