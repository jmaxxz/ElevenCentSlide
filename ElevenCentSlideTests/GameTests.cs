using System;
using System.Linq;
using Xunit;

namespace ElevenCentSlide
{
    public class GameTests
    {
        [Fact]
        public void Constructor_DoneGame_SolvedIsTrue()
        {
            // Arrange
            var pattern = new[] { Coin.None, Coin.Dime, Coin.Penny };

            var board = Enumerable.Repeat(Coin.None, 10)
                        .Concat(Enumerable.Repeat(Coin.Dime, 10))
                        .Concat(Enumerable.Repeat(Coin.Penny, 10))
                        .Concat(pattern);

            // Act
            var instance = new Game(pattern, board);

            // Assert
            Assert.True(instance.Solved);
            Assert.Equal(board, instance.Board);
            Assert.Equal(pattern, instance.TargetPattern);
        }

        [Fact]
        public void Constructor_InCompleteGame_SolvedIsFalse()
        {
            // Arrange
            // Act
            var instance = Game.CommonGame;

            // Assert
            Assert.False(instance.Solved);
            Assert.Equal(new[] { Coin.Penny, Coin.Penny, Coin.Penny, Coin.Dime, Coin.Dime }, instance.TargetPattern);
        }

        [Fact]
        public void Move_WithLowStartingPosition_ReturnsGameWithNewBoard()
        {
            // Arrange
            var pattern = new[] { Coin.None, Coin.Dime, Coin.Penny };
            var board = new[] { Coin.Dime, Coin.Penny, Coin.None, Coin.None };
            var instance = new Game(pattern, board);

            // Act
            instance = instance.Move(0, 2);

            // Assert
            Assert.True(instance.Solved);
            Assert.Equal(new[] { Coin.None, Coin.None, Coin.Dime, Coin.Penny}, instance.Board);
            Assert.Equal(pattern, instance.TargetPattern);
        }

        [Fact]
        public void Move_WithHighStartingPosition_ReturnsGameWithNewBoard()
        {
            // Arrange
            var pattern = new[] { Coin.Dime, Coin.Penny, Coin.None };
            var board = new[] { Coin.None, Coin.None, Coin.Dime, Coin.Penny };
            var instance = new Game(pattern, board);

            // Act
            instance = instance.Move(2, 0);

            // Assert
            Assert.True(instance.Solved);
            Assert.Equal(new[] { Coin.Dime, Coin.Penny, Coin.None, Coin.None }, instance.Board);
            Assert.Equal(pattern, instance.TargetPattern);
        }

        [Theory] // Board size is 
        [InlineData(-1, 2, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny)] // off board
        [InlineData(3, 0, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny)] // off board
        [InlineData(3, 0, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny)] // off board
        [InlineData(0, 0, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny)] // null move
        [InlineData(4, 1, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny, Coin.Dime, Coin.Penny)] // space taken
        [InlineData(2, 1, false, Coin.None, Coin.None, Coin.Dime, Coin.Penny, Coin.Dime, Coin.Penny)] // 1 slot shift (invalid)
        [InlineData(3, 0, false, Coin.None, Coin.None, Coin.None, Coin.Penny, Coin.Penny, Coin.Dime)] // not moving 11 cents
        [InlineData(2, 0, true, Coin.None, Coin.None, Coin.Dime, Coin.Penny)] // valid
        public void IsValid_ReturnsFalseForInvalidMoves(int start, int end, bool expected, params Coin[] board)
        {
            // Arrange
            var pattern = new[] { Coin.Dime, Coin.Penny };
            var instance = new Game(pattern, board);

            // Act
            var result = instance.IsValidMove(start, end);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Move_InvalidMove_ThrowsException()
        {
            // Arrange
            var pattern = new[] { Coin.Dime, Coin.Penny };
            var instance = new Game(pattern, new[] { Coin.None, Coin.None, Coin.Dime, Coin.Penny });

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(()=> instance.Move(1, 3));
        }
    }
}
