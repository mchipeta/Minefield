using Minefield.Enums;
using Minefield.Resources;

namespace Minefield.Services
{
    public class GameLaunchService : IGameLaunchService
    {
        private readonly IGameService _gameService;
        private readonly IConsoleService _consoleService;

        public GameLaunchService(IGameService gameService, IConsoleService consoleService)
        {
            _gameService = gameService;
            _consoleService = consoleService;
        }

        public void LaunchGame()
        {
            var keepPlaying = true;

            _gameService.InitialiseService();

            GameResult result;

            while (keepPlaying)
            {
                _gameService.SetupNewGame();

                result = _gameService.PlayGame();

                if (result == GameResult.Quit)
                {
                    keepPlaying = false;
                }
                else
                {
                    keepPlaying = IsNewGameRequired();
                }
            }
        }

        private bool IsNewGameRequired()
        {
            _consoleService.WriteLine(Strings.ContinuePlaying);

            bool? result = null;
            ConsoleKey key;

            while (result.HasValue == false)
            {
                key = _consoleService.ReadKey();

                if (key == ConsoleKey.Y)
                {
                    result = true;
                }
                else if (key == ConsoleKey.N)
                {
                    result = false;
                }
            }

            return result.Value;
        }
    }
}
