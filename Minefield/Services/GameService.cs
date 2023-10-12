using Minefield.Constants;
using Minefield.Enums;
using Minefield.Models;
using Minefield.Resources;
using Minefield.Rules;

namespace Minefield.Services;

public class GameService : IGameService
{
    private readonly IConsoleService _consoleService;
    private readonly IGameSetupService _gameSetupService;
    private readonly IMoveRule[] _moveRules;
    public GridPosition? StartPosition { get; private set; }
    public GridPosition? CurrentPosition { get; private set; }

    public int Lives { get; private set; }
    public int Score { get; private set; }

    public IReadOnlyList<string> Mines { get; private set; } = new List<string>();

    public GameService(IConsoleService consoleService, IGameSetupService gameSetupService, IMoveRule[] moveRules)
    {
        _consoleService = consoleService;
        _gameSetupService = gameSetupService;
        _moveRules = moveRules;
    }

    public void InitialiseService()
    {
        _consoleService.WriteLine(Strings.WelcomeMessage);
    }

    public void SetupNewGame()
    {
        // Reset lives and score.
        Lives = GameConstants.Lives;
        Score = 0;

        var boardSetup = _gameSetupService.GetBoardSetup();
        StartPosition = boardSetup.StartPosition;
        Mines = boardSetup.Mines;
    }

    public GameResult PlayGame()
    {
        CurrentPosition = StartPosition;
        var gameState = new GameState { GameResult = GameResult.Unknown, IsPlaying = true };
        PrintGameState(CurrentPosition!);

        while (gameState.IsPlaying)
        {
            var key = _consoleService.ReadKey();

            var moveRule = _moveRules.FirstOrDefault(r => r.IsMatch(key));
            if (moveRule != null)
            {
                var moveResult = moveRule.MoveFrom(CurrentPosition!);
                if (moveResult.IsSuccessful)
                {
                    CurrentPosition = moveResult.NewPosition;
                    UpdateGameStateOnSuccessfulMove(moveResult.NewPosition!, gameState);
                }
                else
                {
                    _consoleService.WriteLine(Strings.MoveNotAllowed);
                }
            }
            else if (key == ConsoleKey.Q)
            {
                gameState.IsPlaying = false;
                gameState.GameResult = GameResult.Quit;
            }
        }

        return gameState.GameResult;
    }

    private void UpdateGameStateOnSuccessfulMove(GridPosition newPosition, GameState gameState)
    {
        Score++;

        var message = string.Empty;
        if (HasLandedOnMine(newPosition))
        {
            Lives--;
            if (Lives == 0)
            {
                gameState.IsPlaying = false;
                gameState.GameResult = GameResult.Fail;
                message = Strings.GameOver;
            }
            else
            {
                message = Strings.LifeLost;
            }
        }

        if (gameState.IsPlaying && HasReachedTheEnd(newPosition))
        {
            gameState.IsPlaying = false;
            gameState.GameResult = GameResult.Success;
            message = Strings.GameCompleted;
        }

        PrintGameState(newPosition);
        if (!string.IsNullOrEmpty(message))
        {
            _consoleService.WriteLine(message);
        }
    }

    private void PrintGameState(GridPosition position)
    {
        _consoleService.WriteLine("\n===============================================================");
        _consoleService.WriteLine($"Current Position: {position} | Lives: {Lives} | Score: {Score}");
        _consoleService.WriteLine("===============================================================");
    }

    private bool HasLandedOnMine(GridPosition position)
    {
        return Mines.Contains(position.ToString());
    }

    private bool HasReachedTheEnd(GridPosition position)
    {
        return position.ColumnId == 'H';
    }
}


