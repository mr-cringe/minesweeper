namespace Minesweeper.Core.Data
{
    public class CellData
    {
        public CellState State;
        
        public int MinesCountAround { get; private set; }
        
        public bool IsFlagged => State == CellState.Flagged;
        public bool IsMined => MinesCountAround == -1;

        public CellData(int minesCountAround)
        {
            State = CellState.Closed;
            MinesCountAround = minesCountAround;
        }

        public void IncMinesCountAround()
        {
            MinesCountAround++;
        }
    }
}