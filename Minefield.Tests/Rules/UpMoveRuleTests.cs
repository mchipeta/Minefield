using FluentAssertions;
using Minefield.Models;
using Minefield.Rules;

namespace Minefield.Tests.Rules
{
    [TestFixture]
    public class UpMoveRuleTests
    {
        public void IsMatch_UpArrowKey_ShouldBeTrue()
        {
            var upMoveRule = new UpMoveRule();

            var result = upMoveRule.IsMatch(ConsoleKey.UpArrow);
            
            result.Should().BeTrue();
        }

        [TestCase(ConsoleKey.RightArrow)]
        [TestCase(ConsoleKey.LeftArrow)]
        [TestCase(ConsoleKey.DownArrow)]
        [TestCase(ConsoleKey.Q)]
        public void IsMatch_NonUpArrowKey_ShouldBeFalse(ConsoleKey key)
        {
            var upMoveRule = new UpMoveRule();

            var result = upMoveRule.IsMatch(key);

            result.Should().BeFalse();
        }

        [Test]
        public void MoveFrom_AnywhereExceptRowId8_ShouldSucceed(
            [Values('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
            [Values(1, 2, 3, 4, 5, 6, 7)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var upMoveRule = new UpMoveRule();
            var expectedNewPosition = new GridPosition(columnId, rowId + 1);

            var result = upMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeTrue();
            result.NewPosition!.ToString().Should().Be(expectedNewPosition.ToString());
        }

        [Test]
        public void MoveFrom_RowId8_ShouldFail(
            [Values('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
            [Values(8)] int rowId)
        {
            var currentPosition = new GridPosition(columnId, rowId);
            var upMoveRule = new UpMoveRule();

            var result = upMoveRule.MoveFrom(currentPosition);

            result.IsSuccessful.Should().BeFalse();
            result.NewPosition.Should().BeNull();
        }
    }
}
