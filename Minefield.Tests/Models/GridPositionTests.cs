using FluentAssertions;
using Minefield.Models;

namespace Minefield.Tests.Models
{
    [TestFixture]
    public class GridPositionTests
    {
        [Test]
        public void ToString_AfterColumnCharConstructor_Test(
           [Values('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H')] char columnId,
           [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var expectedResult = $"{columnId}{rowId}";
            
            var position = new GridPosition(columnId, rowId);

            position.ToString().Should().Be(expectedResult);
        }

        [Test]
        public void ToString_AfterColumnIntConstructor_Test(
            [Values(65, 66, 67, 68, 69, 70, 71, 72)] int columnId,
            [Values(1, 2, 3, 4, 5, 6, 7, 8)] int rowId)
        {
            var expectedResult = $"{(char)columnId}{rowId}";

            var position = new GridPosition(columnId, rowId);

            position.ToString().Should().Be(expectedResult);
        }
    }
}
