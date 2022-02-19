using CompanyCrmApi.Controllers;
using CompanyCrmApi.Data.Repositories;
using CustomerModels.Battleship;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCrmApi.Tests
{
    [TestFixture]
    public class BattleshipControllerShould
    {
        private Mock<IBattleshipRepository> _mockBattleshipRepository;

        [SetUp]
        public void Setup()
        {
            _mockBattleshipRepository = new Mock<IBattleshipRepository>();
        }

        [Test]
        public async Task GetShip_PassValidShipId_ReturnsActionResultWithShip()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.GetShipAsync(It.IsAny<int>()))
                                     .ReturnsAsync(MockData.mockShip);

            var mockBattleshipController = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await mockBattleshipController.GetShip(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<Ship>), result);
            Assert.AreEqual(MockData.mockShip.ShipId, result.Value.ShipId);
            Assert.AreEqual(MockData.mockShip.Name, result.Value.Name);
            Assert.AreEqual(MockData.mockShip.Length, result.Value.Length);
            Assert.AreEqual(MockData.mockShip.HitCount, result.Value.HitCount);
            Assert.AreEqual(MockData.mockShip.IsSunk, result.Value.IsSunk);
            Assert.AreEqual(MockData.mockShip.BelongsToUser, result.Value.BelongsToUser);
            Assert.AreEqual(MockData.mockShip.ImageUrl, result.Value.ImageUrl);
            Assert.AreEqual(MockData.mockShip.GameId, result.Value.GameId);
            Assert.AreEqual(MockData.mockShip.Coordinates, result.Value.Coordinates);
        }

        [Test]
        public async Task GetShip_PassInvalidShipId_ReturnsNotFound()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.GetShipAsync(It.IsAny<int>()))
                                  .Returns(Task.FromResult<Ship>(null));

            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.GetShip(It.IsAny<int>());

            // Assert
            _mockBattleshipRepository.Verify(x => x.GetShipAsync(It.IsAny<int>()), Times.Once);
            Assert.IsNull(result.Value);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task GetCoordinate_PassValidParameters_ReturnsTrue()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.GetCoordinateAsync(10, true, 1, 2))
                                  .ReturnsAsync(MockData.mockCoordinate);
            _mockBattleshipRepository.Setup(x => x.GetShipAsync(It.IsAny<int>())).ReturnsAsync(MockData.mockShip);
            _mockBattleshipRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.Get(10, true, 1, 2);

            // Assert
            Assert.IsTrue(result.Value);
        }

        [Test]
        public async Task GetCoordinate_PassInvalidParameters_ReturnsFalse()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.GetCoordinateAsync(10, true, 1, 2))
                                  .ReturnsAsync((Coordinate)null);

            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.Get(10, true, 1, 2);

            // Assert
            Assert.IsFalse(result.Value);
            _mockBattleshipRepository.Verify(x => x.GetCoordinateAsync(10, true, 1, 2), Times.Once);
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        }

        [Test]
        public async Task Post_ValidGame_CallsRepositoryAndReturnsCreated201Response()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.Add(It.IsAny<Game>()));
            _mockBattleshipRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.Post(MockData.mockGame);
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult<Game>), result);
            Assert.AreEqual(201, objectResult.StatusCode);
            _mockBattleshipRepository.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
            _mockBattleshipRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Post_InvalidGame_Returns400BadRequest()
        {
            // Arrange
            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.Post(MockData.mockGame);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        }

        [Test]
        public async Task IncrementShipHitCount_PassValidShipId_AddsHitCountAndReturnsShipFiredOn()
        {
            // Arrange
            _mockBattleshipRepository.Setup(x => x.GetShipAsync(It.IsAny<int>())).ReturnsAsync(MockData.mockShip2);
            _mockBattleshipRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.IncrementShipHitCount(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf(typeof(Ship), result);
            Assert.AreEqual(1, result.HitCount);
            Assert.AreEqual(MockData.mockShip2.ShipId, result.ShipId);
            Assert.AreEqual(MockData.mockShip2.Name, result.Name);
            Assert.AreEqual(MockData.mockShip2.Length, result.Length);
            Assert.AreEqual(MockData.mockShip2.IsSunk, result.IsSunk);
            Assert.AreEqual(MockData.mockShip2.BelongsToUser, result.BelongsToUser);
            Assert.AreEqual(MockData.mockShip2.ImageUrl, result.ImageUrl);
            Assert.AreEqual(MockData.mockShip2.GameId, result.GameId);
            Assert.AreEqual(MockData.mockShip2.Coordinates, result.Coordinates);
        }

        [Test]
        public async Task IncrementShipHitCount_PassInvalidShipId_ReturnsNull()
        {
            // Arrange
            var sut = new BattleshipController(_mockBattleshipRepository.Object);

            // Act
            var result = await sut.IncrementShipHitCount(It.IsAny<int>());

            // Assert
            _mockBattleshipRepository.Verify(x => x.GetShipAsync(It.IsAny<int>()), Times.Once);
            _mockBattleshipRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
            Assert.IsNull(result);
        }
    }
}