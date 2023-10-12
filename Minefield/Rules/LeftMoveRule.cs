using Minefield.Models;

namespace Minefield.Rules
{
    public class LeftMoveRule : IMoveRule
    {
        /// <summary>
        /// Equivalent of column A on the chess board.
        /// </summary>
        private readonly int _minColumnId = 65;

        public bool IsMatch(ConsoleKey direction)
        {
            return direction == ConsoleKey.LeftArrow;
        }

        public MoveResult MoveFrom(GridPosition currentPosition)
        {
            var result = new MoveResult { IsSuccessful = currentPosition.ColumnIdAsInt != _minColumnId };
            if (result.IsSuccessful)
            {
                result.NewPosition = new GridPosition(currentPosition.ColumnIdAsInt - 1, currentPosition.RowId);
            }
            return result;
        }
    }
}
