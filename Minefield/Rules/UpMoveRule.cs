using Minefield.Models;

namespace Minefield.Rules
{
    public class UpMoveRule : IMoveRule
    {
        private readonly int _maxRowId = 8;

        public bool IsMatch(ConsoleKey direction)
        {
            return direction == ConsoleKey.UpArrow;
        }

        public MoveResult MoveFrom(GridPosition currentPosition)
        {
            var result = new MoveResult { IsSuccessful = currentPosition.RowId != _maxRowId };
            if (result.IsSuccessful) 
            {
                result.NewPosition = new GridPosition(currentPosition.ColumnId, currentPosition.RowId + 1);
            }
            return result;
        }
    }
}
