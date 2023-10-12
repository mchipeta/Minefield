using Minefield.Constants;
using Minefield.Factories;
using Minefield.Models;

namespace Minefield.Services
{
    public class GameSetupService : IGameSetupService
    {
        private readonly IRandomFactory _randomFactory;

        public GameSetupService(IRandomFactory randomFactory)
        {
            _randomFactory = randomFactory;
        }

        public BoardSetup GetBoardSetup()
        {
            // Get new start position.
            var rand = _randomFactory.Create();
            var startPositionRowId = rand.Next(GameConstants.MinRowId, GameConstants.MaxRowId);
            var startPosition = new GridPosition('A', startPositionRowId);

            // Generate mines.
            var indexToGridPosition = new Dictionary<int, GridPosition>();

            var columns = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var rows = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var index = 1;
            foreach (var column in columns)
            {
                foreach (var row in rows)
                {
                    indexToGridPosition[index++] = new GridPosition(column, row);
                }
            }

            var mines = new List<string>();
            var isGeneratingMines = true;
            while (isGeneratingMines)
            {
                // 8 x 8 grid has 64 cells.
                index = rand.Next(1, 64);
                var minePosition = indexToGridPosition[index];

                var mine = minePosition.ToString();
                if (mine != startPosition.ToString() && !mines.Contains(mine))
                {
                    mines.Add(mine);
                }
                if (mines.Count == GameConstants.Mines)
                {
                    isGeneratingMines = false;
                }
            }

            return new BoardSetup { Mines = mines, StartPosition = startPosition };
        }
    }
}
