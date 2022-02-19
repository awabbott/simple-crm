using CustomerModels.Battleship;
using NUnit.Framework;
using System.Collections.Generic;

namespace CustomerModels.Tests
{
    [TestFixture]
    class ShipClassShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(ShipName.Carrier, 5)]
        [TestCase(ShipName.Battleship, 4)]
        [TestCase(ShipName.Cruiser, 3)]
        [TestCase(ShipName.Submarine, 3)]
        [TestCase(ShipName.Destroyer, 2)]
        public void IntantiatingClassWithShipName_SetsCorrespondingShipLength(ShipName shipName, int expected)
        {
            Ship mockShip = new Ship(shipName);

            int actual = mockShip.Length;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(ShipName.Carrier, "images/Carrier.png")]
        [TestCase(ShipName.Battleship, "images/Battleship.png")]
        [TestCase(ShipName.Cruiser, "images/Cruiser.png")]
        [TestCase(ShipName.Submarine, "images/Submarine.png")]
        [TestCase(ShipName.Destroyer, "images/Destroyer.png")]
        public void IntantiatingClassWithShipName_SetsCorrespondingImageUrl(ShipName shipName, string expected)
        {
            Ship mockShip = new Ship(shipName);

            string actual = mockShip.ImageUrl;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(ShipName.Carrier, 5)]
        [TestCase(ShipName.Battleship, 4)]
        [TestCase(ShipName.Cruiser, 3)]
        [TestCase(ShipName.Submarine, 3)]
        [TestCase(ShipName.Destroyer, 2)]
        public void InstantiatingClassWithShipName_SetsCoordinateArrays(ShipName shipName, int length)
        {
            int expected = new List<Coordinate>(length).Count;

            Ship mockShip = new Ship(shipName);
            int actual = mockShip.Coordinates.Count;

            Assert.IsInstanceOf(typeof(List<Coordinate>), mockShip.Coordinates);
            Assert.AreEqual(expected, actual);
        }
    }
}