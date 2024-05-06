public struct Position
{
    public Position(int rowIn, int colIn)
    {
        _row = rowIn; _col = colIn;
    }
    public Position(Position posToCopy)
    {
        _row = posToCopy.row; _col = posToCopy.col;
    }
    // to change row and col can be lowe than 0 with new chunks instead map
    public int row { get => _row; set { if (value < 0 && _row + value < 0) { _row = 0; } else { _row += value; } } }
    public int col { get => _col; set { if (value < 0 && _col + value < 0) { _col = 0; } else { _col += value; } } }

    private int _row;
    private int _col;
}
