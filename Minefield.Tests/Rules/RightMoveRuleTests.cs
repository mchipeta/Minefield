using FluentAssertions;
using Minefield.Models;
using Minefield.Rules;

namespace Minefield.Tests.Rules
{
    [TestFixture]
    public class RightMoveRuleTests
    {
        public void IsMatch_RightArrowKey_ShouldBeTrue()
        {
            var rightMoveRule = new RightMoveRule();

            var result = rightMoveRule.IsMatch(ConsoleKey.RightArrow);
            
            result.Should().BeTrue();
        }

        [TestCase(ConsoleKey.LeftArrow)]
        [TestCase(ConsoleKey.UpArrow)]
        [TestCase(ConsoleKey.DownArrow)]
        [TestCase(ConsoleKey.Q)]
        public void IsMatch_NonRightArrowKey_ShouldBeFalse(ConsoleKey key)
        {
            var rightMoveRule = new RightMoveRule();

            var result = rightMoveRule.IsMatch(key);

            result.Should().BeFalse();
        }

        [Test]
        public void MoveFrom_AnywhereExceptColumnH_ShouldSucceed(
            [Values('A', 'B', 'C', 'D', 'E', 'F', 'G')] char columnId,
            [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var rightMoveRule = new RightMoveRule();
            var expectedNewPosition = new GridPosition(columnId + 1, rowId);

            var result = rightMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeTrue();
            result.NewPosition!.ToString().Should().Be(expectedNewPosition.ToString());
        }

        [Test]
        public void MoveFrom_ColumnH_ShouldFail(
            [Values('H')] char columnId,
            [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var rightMoveRule = new RightMoveRule();

            var result = rightMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeFalse();
            result.NewPosition.Should().BeNull();
        }
    }
}
