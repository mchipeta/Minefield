using Minefield.Models;

namespace Minefield.Rules
{
    public class DownMoveRule : IMoveRule
    {
        private readonly int _minRowId = 1;

        public bool IsMatch(ConsoleKey direction)
        {
            return direction == ConsoleKey.DownArrow;
        }

        public MoveResult MoveFrom(GridPosition currentPosition)
        {
            var result = new MoveResult { IsSuccessful = currentPosition.RowId != _minRowId };
            if (result.IsSuccessful)
            {
                result.NewPosition = new GridPosition(currentPosition.ColumnId, currentPosition.RowId - 1);
            }
            return result;
        }
    }
}
