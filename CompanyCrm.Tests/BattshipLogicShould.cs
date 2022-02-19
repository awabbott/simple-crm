using CustomerModels.Battleship;
using System;
using System.Collections.Generic;
using Xunit;

namespace CompanyCrm.Tests
{
    public class BattshipLogicShould
    {
        [Theory]
        [Repeat(25)]
        public void PlaceShips_CreatesListOfCoordinates_WithinRange(int iterationNumber)
        {
            // Arrange

            // Act
            var result = BattleshipLogic.PlaceShips();

            // Assert
            Assert.NotEmpty(result);
            Assert.IsType<int>(iterationNumber);

            foreach (var ship in result)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    Assert.InRange(coordinate.Row, 0, 9);
                    Assert.InRange(coordinate.Column, 0, 9);
                }
            }
        }

        [Fact]
        public void PlaceShips_ReturnsExactly17Coordinates()
        {
            // Arrange
            int coordinateCount = 0;

            // Act
            var result = BattleshipLogic.PlaceShips();
            foreach (var ship in result)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    coordinateCount++;
                }
            }

            // Assert
            Assert.Equal(17, coordinateCount);
        }

        [Fact]
        public void AllCoordinatesAreInbounds_ReturnsTrue_OnValidListOfCoordinates()
        {
            // Arrange

            // Act
            var result = BattleshipLogic.AllCoordinatesAreInBounds(MockBattleshipData.inBoundCoordinateList);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AllCoordinatesAreInbounds_ReturnsFalse_OnInvalidListOfCoordinates()
        {
            // Arrange

            // Act
            var result = BattleshipLogic.AllCoordinatesAreInBounds(MockBattleshipData.outOfBoundCoordinateList);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShipsDoNotOverlap_ReturnsTrue_OnInvalidListOfCoordinates()
        {
            // Arrange

            // Act
            var result = BattleshipLogic.ShipsDoNotOverlap(MockBattleshipData.inBoundCoordinateList);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShipsDoNotOverlap_ReturnsFalse_OnOverlappingListOfCoordinates()
        {
            // Arrange

            // Act
            var result = BattleshipLogic.ShipsDoNotOverlap(MockBattleshipData.overlappingCoordinateList);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddRemainingCoordinates_ReturnsSequentialListOfCoordinates()
        {
            // Arrange
            Ship mockCarrier = new Ship(ShipName.Carrier)
            {
                Coordinates = new List<Coordinate> { new Coordinate { Row = 1, Column = 2 } }
            };

            bool shipIsVertical = false;

            List<Coordinate> expected = new List<Coordinate>
            {
                new Coordinate { Row = 1, Column = 2 },
                new Coordinate { Row = 1, Column = 3 },
                new Coordinate { Row = 1, Column = 4 },
                new Coordinate { Row = 1, Column = 5 },
                new Coordinate { Row = 1, Column = 6 }
            };

            // Act
            List<Coordinate> actual = BattleshipLogic.AddRemainingCoordinates(mockCarrier, shipIsVertical);

            // Assert
            Assert.Equal(5, actual.Count);

            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(expected[i].Row, actual[i].Row);
                Assert.Equal(expected[i].Column, actual[i].Column);
            }
        }
    }
}
