using Minefield.Models;

namespace Minefield.Rules
{
    public interface IMoveRule
    {
        bool IsMatch(ConsoleKey direction);
        MoveResult MoveFrom(GridPosition currentPosition);
    }
}
