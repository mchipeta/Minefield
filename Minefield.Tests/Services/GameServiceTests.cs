using FluentAssertions;
using Minefield.Models;
using Minefield.Rules;
using Minefield.Services;
using Moq;

namespace Minefield.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private const int _initialLives = 3;

        [Test]
        public void InitialiseService_PrintAWelcomeMessage()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            var gameSetupServiceMock = new Mock<IGameSetupService>();
            var moveRules = new IMoveRule[0];
            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();

            consoleServiceMock.Verify(c => c.WriteLine(It.IsAny<string>()));
        }

        [Test]
        public void SetupNewGame_ResetsGameState()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            
            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string>(),
                StartPosition = new GridPosition('A', 1)
            });

            var moveRules = new IMoveRule[0];
            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();

            gameService.Lives.Should().Be(_initialLives);
            gameService.Score.Should().Be(0);
        }

        [Test]
        public void SetupNewGame_InitialisesMinesAndStartPosition()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            
            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string>(),
                StartPosition = new GridPosition('A', 1)
            });
            
            var moveRules = new IMoveRule[0];
            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();

            gameSetupServiceMock.Verify(g => g.GetBoardSetup());
            gameService.StartPosition.Should().NotBeNull();
            gameService.Mines.Should().NotBeNull();
        }

        [Test]
        public void PlayGame_IllegalMove_ShouldNotUpdateLivesAndMoves()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.UpArrow)
                .Returns(ConsoleKey.Q);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string>(),
                StartPosition = new GridPosition('A', 1)
            });
            var moveRules = new IMoveRule[1];
            
            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.UpArrow))).Returns(true);
            mockMoveRule.Setup(m => m.MoveFrom(It.IsAny<GridPosition>())).Returns(new MoveResult { IsSuccessful = false });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            gameService.PlayGame();

            gameService.Lives.Should().Be(_initialLives);
            gameService.Score.Should().Be(0);
            gameService.CurrentPosition!.ToString().Should().Be(gameService.StartPosition!.ToString());
        }

        [Test]
        public void PlayGame_ValidMove_ShouldUpdateMoves()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.UpArrow)
                .Returns(ConsoleKey.Q);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string>(),
                StartPosition = new GridPosition('A', 1)
            });
            var moveRules = new IMoveRule[1];

            var newPosition = new GridPosition('B', 1);
            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.UpArrow))).Returns(true);
            mockMoveRule.Setup(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = newPosition });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            gameService.PlayGame();

            gameService.Score.Should().Be(1);
            gameService.CurrentPosition!.ToString().Should().Be(newPosition.ToString());
        }

        [Test]
        public void PlayGame_MoveToMine_ShouldUpdateLivesAndMoves()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.UpArrow)
                .Returns(ConsoleKey.Q);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string> { "B1" },
                StartPosition = new GridPosition('A', 1)
            });

            var moveRules = new IMoveRule[1];

            var newPosition = new GridPosition('B', 1);
            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.UpArrow))).Returns(true);
            mockMoveRule.Setup(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = newPosition });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            gameService.PlayGame();

            gameService.Score.Should().Be(1);
            gameService.Lives.Should().Be(_initialLives - 1);
            gameService.CurrentPosition!.ToString().Should().Be(newPosition.ToString());
        }

        [Test]
        public void PlayGame_MoveToMineZeroLives_ShouldEndGameAsFail()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.UpArrow)
                .Returns(ConsoleKey.UpArrow)
                .Returns(ConsoleKey.UpArrow);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string> { "A2", "A3", "A4" },
                StartPosition = new GridPosition('A', 1)
            }); 

            var moveRules = new IMoveRule[1];

            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.UpArrow))).Returns(true);
            mockMoveRule.SetupSequence(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('A', 2) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('A', 3) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('A', 4) });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            var result = gameService.PlayGame();

            gameService.Score.Should().Be(3);
            gameService.Lives.Should().Be(0);
            gameService.CurrentPosition!.ToString().Should().Be("A4");
            result.Should().Be(Enums.GameResult.Fail);
        }

        [Test]
        public void PlayGame_PlayerPressesQ_ShouldEndGameAsQuit()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.Q);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string>(),
                StartPosition = new GridPosition('A', 1)
            });
            var moveRules = new IMoveRule[0];
            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            var result = gameService.PlayGame();

            result.Should().Be(Enums.GameResult.Quit);
        }

        [Test]
        public void PlayGame_MoveToMineOnHColumnZeroLives_ShouldEndGameAsFail()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string> { "B1", "C1", "H1" },
                StartPosition = new GridPosition('A', 1)
            });

            var moveRules = new IMoveRule[1];

            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.RightArrow))).Returns(true);
            mockMoveRule.SetupSequence(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('B', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('C', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('D', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('E', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('F', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('G', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('H', 1) });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            var result = gameService.PlayGame();

            gameService.Score.Should().Be(7);
            gameService.Lives.Should().Be(0);
            gameService.CurrentPosition!.ToString().Should().Be("H1");
            result.Should().Be(Enums.GameResult.Fail);
        }

        [Test]
        public void PlayGame_MoveToMineOnHColumnNonZeroLives_ShouldEndGameAsSuccess()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string> { "B1", "H1" },
                StartPosition = new GridPosition('A', 1)
            });

            var moveRules = new IMoveRule[1];

            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.RightArrow))).Returns(true);
            mockMoveRule.SetupSequence(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('B', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('C', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('D', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('E', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('F', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('G', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('H', 1) });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            var result = gameService.PlayGame();

            gameService.Score.Should().Be(7);
            gameService.Lives.Should().Be(1);
            gameService.CurrentPosition!.ToString().Should().Be("H1");
            result.Should().Be(Enums.GameResult.Success);
        }

        [Test]
        public void PlayGame_MoveToHColumnNonZeroLives_ShouldEndGameAsSuccess()
        {
            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow)
                .Returns(ConsoleKey.RightArrow);

            var gameSetupServiceMock = new Mock<IGameSetupService>();
            gameSetupServiceMock.Setup(g => g.GetBoardSetup()).Returns(new BoardSetup
            {
                Mines = new List<string> { "B2", "C2", "H2" },
                StartPosition = new GridPosition('A', 1)
            });

            var moveRules = new IMoveRule[1];

            var mockMoveRule = new Mock<IMoveRule>();
            mockMoveRule.Setup(m => m.IsMatch(It.Is<ConsoleKey>(k => k == ConsoleKey.RightArrow))).Returns(true);
            mockMoveRule.SetupSequence(m => m.MoveFrom(It.IsAny<GridPosition>()))
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('B', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('C', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('D', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('E', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('F', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('G', 1) })
                .Returns(new MoveResult { IsSuccessful = true, NewPosition = new GridPosition('H', 1) });
            moveRules[0] = mockMoveRule.Object;

            var gameService = new GameService(consoleServiceMock.Object, gameSetupServiceMock.Object, moveRules);

            gameService.InitialiseService();
            gameService.SetupNewGame();
            var result = gameService.PlayGame();

            gameService.Score.Should().Be(7);
            gameService.Lives.Should().Be(3);
            gameService.CurrentPosition!.ToString().Should().Be("H1");
            result.Should().Be(Enums.GameResult.Success);
        }
    }
}
