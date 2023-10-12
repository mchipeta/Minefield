using FluentAssertions;
using Minefield.Models;
using Minefield.Rules;

namespace Minefield.Tests.Rules
{
    [TestFixture]
    public class LeftMoveRuleTests
    {
        public void IsMatch_LeftArrowKey_ShouldBeTrue()
        {
            var leftMoveRule = new LeftMoveRule();

            var result = leftMoveRule.IsMatch(ConsoleKey.LeftArrow);
            
            result.Should().BeTrue();
        }

        [TestCase(ConsoleKey.RightArrow)]
        [TestCase(ConsoleKey.UpArrow)]
        [TestCase(ConsoleKey.DownArrow)]
        [TestCase(ConsoleKey.Q)]
        public void IsMatch_NonLeftArrowKey_ShouldBeFalse(ConsoleKey key)
        {
            var leftMoveRule = new LeftMoveRule();

            var result = leftMoveRule.IsMatch(key);

            result.Should().BeFalse();
        }

        [Test]
        public void MoveFrom_AnywhereExceptColumnA_ShouldSucceed(
            [Values('B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
            [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var leftMoveRule = new LeftMoveRule();
            var expectedNewPosition = new GridPosition(columnId - 1, rowId);

            var result = leftMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeTrue();
            result.NewPosition!.ToString().Should().Be(expectedNewPosition.ToString());
        }

        [Test]
        public void MoveFrom_ColumnA_ShouldFail(
            [Values('A')] char columnId,
            [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var leftMoveRule = new LeftMoveRule();

            var result = leftMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeFalse();
            result.NewPosition.Should().BeNull();
        }
    }
}
