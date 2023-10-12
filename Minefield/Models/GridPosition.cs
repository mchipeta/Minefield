namespace Minefield.Models
{
    public class GridPosition
    {
        public int RowId { get; private set; }
        public char ColumnId { get; private set; }
        public int ColumnIdAsInt { get; private set; }

        public GridPosition(char columnId, int rowId)
        {
            ColumnId = columnId;
            ColumnIdAsInt = columnId;
            RowId = rowId;
        }

        public GridPosition(int columnId, int rowId)
        {
            ColumnId = (char)columnId;
            ColumnIdAsInt = columnId;
            RowId = rowId;
        }

        public override string ToString()
        {
            return $"{ColumnId}{RowId}";
        }
    }
}
