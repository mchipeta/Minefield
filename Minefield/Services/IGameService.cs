using Minefield.Enums;
using Minefield.Models;

namespace Minefield.Services
{
    public interface IGameService
    {
        GridPosition? StartPosition { get; }
        GridPosition? CurrentPosition { get;}
        int Lives { get; }
        int Score { get; }
        IReadOnlyList<string> Mines { get; }

        /// <summary>
        /// One-time setup of the game service.
        /// </summary>
        void InitialiseService();

        /// <summary>
        /// Creates a new game.
        /// </summary>
        void SetupNewGame();

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <returns>The result of the game after it has ended.</returns>
        GameResult PlayGame();
    }
}
