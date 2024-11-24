using System;
using Xunit;

namespace ConsoleTests
{
    public class ProgramTests
    {
        [Theory]
        [InlineData(2, 1, false)]
        [InlineData(51, 50, false)]
        [InlineData(1, 2, false)]
        [InlineData(2, 50, true)]
        [InlineData(50, 50, true)]
        public void IsValidInput_TestCases_ReturnsExpectedResult(int m, int n, bool expected)
        {
            // Arrange
            bool result = Program.IsValidInput(m, n);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(4, 6, 4)]
        [InlineData(2, 2, 2)]
        public void CountTilings_ValidInput_ReturnsExpectedResult(int m, int n, int expected)
        {
            // Arrange
            long result = Program.CountTilings(m, n);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CountTilings_ExampleInput1_ReturnsCorrectResult()
        {
            // Arrange
            long result = Program.CountTilings(2, 3);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void IsValidInput_ValidInput_ReturnsTrue()
        {
            // Arrange
            bool result = Program.IsValidInput(2, 2);

            // Assert
            Assert.True(result);
        }
    }
}