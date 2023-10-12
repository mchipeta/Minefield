using FluentAssertions;
using Minefield.Models;
using Minefield.Rules;

namespace Minefield.Tests.Rules
{
    [TestFixture]
    public class DownMoveRuleTests
    {
        [Test]
        public void IsMatch_DownArrowKey_ShouldBeTrue()
        {
            var downMoveRule = new DownMoveRule();

            var result = downMoveRule.IsMatch(ConsoleKey.DownArrow);

            result.Should().BeTrue();
        }

        [TestCase(ConsoleKey.RightArrow)]
        [TestCase(ConsoleKey.UpArrow)]
        [TestCase(ConsoleKey.LeftArrow)]
        [TestCase(ConsoleKey.Q)]
        public void IsMatch_NonDownArrowKey_ShouldBeFalse(ConsoleKey key)
        {
            var downMoveRule = new DownMoveRule();

            var result = downMoveRule.IsMatch(key);

            result.Should().BeFalse();
        }

        [Test]
        public void MoveFrom_AnywhereExceptRowId1_ShouldSucceed(
            [Values('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
            [Values(2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var downMoveRule = new DownMoveRule();
            var expectedNewPosition = new GridPosition(columnId, rowId - 1);

            var result = downMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeTrue();
            result.NewPosition!.ToString().Should().Be(expectedNewPosition.ToString());
        }

        [Test]
        public void MoveFrom_RowId1_ShouldFail(
            [Values('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
            [Values(1)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var downMoveRule = new DownMoveRule();

            var result = downMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeFalse();
            result.NewPosition.Should().BeNull();
        }
    }
}
