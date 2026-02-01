namespace Minesweeper.Core
{
    public enum CellState
    {
        Closed = 0,
        Opened = 1,
        Flagged = 2,
        FlaggedWrong = 3,
        Mine = 4,
        MineRed = 5,
    }
}