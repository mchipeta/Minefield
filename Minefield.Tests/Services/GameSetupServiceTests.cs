using FluentAssertions;
using Minefield.Factories;
using Minefield.Services;
using Moq;

namespace Minefield.Tests.Services
{
    [TestFixture]
    public class GameSetupServiceTests
    {
        private const int _numberOfMines = 16;
        private readonly Random _random = new Random(1);

        [Test]
        public void GetBoardSetup_InitialisesStartPositionAndMines()
        {
            var randomFactoryMock = new Mock<IRandomFactory>();
            randomFactoryMock.Setup(r => r.Create()).Returns(_random);
            var gameSetupService = new GameSetupService(randomFactoryMock.Object);
            
            var boardSetup = gameSetupService.GetBoardSetup();

            boardSetup.StartPosition.Should().NotBeNull();
            boardSetup.Mines.Should().NotBeNull();
        }

        [Test]
        public void GetBoardSetup_InitialisesStartPositionInColumnA()
        {
            var randomFactoryMock = new Mock<IRandomFactory>();
            randomFactoryMock.Setup(r => r.Create()).Returns(_random);
            var gameSetupService = new GameSetupService(randomFactoryMock.Object);

            var boardSetup = gameSetupService.GetBoardSetup();

            boardSetup.StartPosition!.ToString().Should().StartWith("A");
        }

        [Test]
        public void GetBoardSetup_ReturnsNoMineInStartPosition()
        {
            var randomFactoryMock = new Mock<IRandomFactory>();
            randomFactoryMock.Setup(r => r.Create()).Returns(_random);
            var gameSetupService = new GameSetupService(randomFactoryMock.Object);

            var boardSetup = gameSetupService.GetBoardSetup();

            boardSetup.Mines.Contains(boardSetup.StartPosition!.ToString()).Should().BeFalse();
        }

        [Test]
        public void GetBoardSetup_ReturnsCorrectNumberOfMines()
        {
            var randomFactoryMock = new Mock<IRandomFactory>();
            randomFactoryMock.Setup(r => r.Create()).Returns(_random);
            var gameSetupService = new GameSetupService(randomFactoryMock.Object);

            var boardSetup = gameSetupService.GetBoardSetup();

            boardSetup.Mines.Count.Should().Be(_numberOfMines);
        }

        [Test]
        public void GetBoardSetup_ReturnsNoDuplicateMines()
        {
            var randomFactoryMock = new Mock<IRandomFactory>();
            randomFactoryMock.Setup(r => r.Create()).Returns(_random);
            var gameSetupService = new GameSetupService(randomFactoryMock.Object);

            var boardSetup = gameSetupService.GetBoardSetup();

            boardSetup.Mines.Distinct().Count().Should().Be(_numberOfMines);
        }
    }
}
