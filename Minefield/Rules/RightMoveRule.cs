using Minefield.Models;

namespace Minefield.Rules
{
    public class RightMoveRule : IMoveRule
    {
        /// <summary>
        /// Equivalent of column H on the chess board.
        /// </summary>
        private readonly int _maxColumnId = 72;

        public bool IsMatch(ConsoleKey direction)
        {
            return direction == ConsoleKey.RightArrow;
        }

        public MoveResult MoveFrom(GridPosition currentPosition)
        {
            var result = new MoveResult { IsSuccessful = currentPosition.ColumnIdAsInt != _maxColumnId };
            if (result.IsSuccessful)
            {
                result.NewPosition = new GridPosition(currentPosition.ColumnIdAsInt + 1, currentPosition.RowId);
            }
            return result;
        }
    }
}
