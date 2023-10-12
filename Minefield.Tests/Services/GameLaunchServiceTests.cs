using Minefield.Services;
using Moq;

namespace Minefield.Tests.Services
{
    [TestFixture]
    public class GameLaunchServiceTests
    {
        [Test]
        public void LaunchGame_PlayerQuitsAfter1Game_ShouldPlayGameOnce()
        {
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock.Setup(g => g.PlayGame()).Returns(Enums.GameResult.Quit);

            var consoleServiceMock = new Mock<IConsoleService>();
            var gameLaunchService = new GameLaunchService(gameServiceMock.Object, consoleServiceMock.Object);

            gameLaunchService.LaunchGame();

            gameServiceMock.Verify(g => g.InitialiseService(), Times.Once());
            gameServiceMock.Verify(g => g.SetupNewGame(), Times.Once());
            gameServiceMock.Verify(g => g.PlayGame(), Times.Once());
        }

        [Test]
        public void LaunchGame_PlayerAgreesToPlay1MoreGame_ShouldPlayGameTwice()
        {
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock.Setup(g => g.PlayGame()).Returns(Enums.GameResult.Success);

            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.Y) // yes to 1 more game
                .Returns(ConsoleKey.N); // no to 2 more games

            var gameLaunchService = new GameLaunchService(gameServiceMock.Object, consoleServiceMock.Object);

            gameLaunchService.LaunchGame();

            gameServiceMock.Verify(g => g.InitialiseService(), Times.Once());
            gameServiceMock.Verify(g => g.SetupNewGame(), Times.Exactly(2));
            gameServiceMock.Verify(g => g.PlayGame(), Times.Exactly(2));
        }

        [Test]
        public void LaunchGame_PlayerAgreesToPlay2MoreGames_ShouldPlayGameThrice()
        {
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock.Setup(g => g.PlayGame()).Returns(Enums.GameResult.Success);

            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.Y) // yes to 1 more game
                .Returns(ConsoleKey.Y) // yes to 2 more games
                .Returns(ConsoleKey.N); // no to 3 more games

            var gameLaunchService = new GameLaunchService(gameServiceMock.Object, consoleServiceMock.Object);

            gameLaunchService.LaunchGame();

            gameServiceMock.Verify(g => g.InitialiseService(), Times.Once());
            gameServiceMock.Verify(g => g.SetupNewGame(), Times.Exactly(3));
            gameServiceMock.Verify(g => g.PlayGame(), Times.Exactly(3));
        }

        [Test]
        public void LaunchGame_PlayerEntersInvalidKeyAFewTimesBeforeAgreeingToPlay1MoreGame_ShouldPlayGameTwice()
        {
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock.Setup(g => g.PlayGame()).Returns(Enums.GameResult.Success);

            var consoleServiceMock = new Mock<IConsoleService>();
            consoleServiceMock.SetupSequence(c => c.ReadKey())
                .Returns(ConsoleKey.A) // invalid key
                .Returns(ConsoleKey.B) // invalid key
                .Returns(ConsoleKey.C) // invalid key
                .Returns(ConsoleKey.Y) // yes to 1 more game
                .Returns(ConsoleKey.N); // no to 2 more games

            var gameLaunchService = new GameLaunchService(gameServiceMock.Object, consoleServiceMock.Object);

            gameLaunchService.LaunchGame();

            gameServiceMock.Verify(g => g.InitialiseService(), Times.Once());
            gameServiceMock.Verify(g => g.SetupNewGame(), Times.Exactly(2));
            gameServiceMock.Verify(g => g.PlayGame(), Times.Exactly(2));
        }
    }
}
