using CompanyCrm.Models;
using CustomerModels.Battleship;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyCrm.Tests
{
    public class MockBattleshipData
    {
        public static List<Coordinate> inBoundCoordinateList = new List<Coordinate>
        {
            new Coordinate { Row = 1, Column = 2 },
            new Coordinate { Row = 1, Column = 3 },
            new Coordinate { Row = 4, Column = 5 },
            new Coordinate { Row = 5, Column = 5 },
            new Coordinate { Row = 6, Column = 5 },
            new Coordinate { Row = 0, Column = 9 },
            new Coordinate { Row = 1, Column = 9 },
            new Coordinate { Row = 2, Column = 9 },
            new Coordinate { Row = 3, Column = 9 }
        };

        public static List<Coordinate> outOfBoundCoordinateList = new List<Coordinate>
        {
            new Coordinate { Row = 1, Column = 2 },
            new Coordinate { Row = 1, Column = 3 },
            new Coordinate { Row = 4, Column = 5 },
            new Coordinate { Row = 5, Column = 5 },
            new Coordinate { Row = 6, Column = 5 },
            new Coordinate { Row = 0, Column = 9 },
            new Coordinate { Row = 0, Column = 10 },
            new Coordinate { Row = 0, Column = 11 },
            new Coordinate { Row = 0, Column = 12 }
        };

        public static List<Coordinate> overlappingCoordinateList = new List<Coordinate>
        {
            new Coordinate { Row = 1, Column = 2 },
            new Coordinate { Row = 1, Column = 3 },
            new Coordinate { Row = 4, Column = 5 },
            new Coordinate { Row = 5, Column = 5 },
            new Coordinate { Row = 6, Column = 5 },
            new Coordinate { Row = 1, Column = 3 },
            new Coordinate { Row = 2, Column = 3 },
            new Coordinate { Row = 3, Column = 3 },
            new Coordinate { Row = 4, Column = 3 }
        };

        public static BattleshipViewModel mockBattleshipViewModel = new BattleshipViewModel
        {
            UserFleet = new List<Ship>
            {
                new Ship(ShipName.Destroyer)
                {
                    Coordinates = new List<Coordinate>
                    {
                        new Coordinate { Row = 1, Column = 1 },
                        new Coordinate { Row = 1, Column = 2 }
                    }
                },
                new Ship(ShipName.Cruiser)
                {
                    Coordinates = new List<Coordinate>
                    {
                        new Coordinate { Row = 2, Column = 2 },
                        new Coordinate { Row = 3, Column = 2 },
                        new Coordinate { Row = 4, Column = 2 }
                    }
                },
                new Ship(ShipName.Battleship)
                {
                    Coordinates = new List<Coordinate>
                    {
                        new Coordinate { Row = 8, Column = 5 },
                        new Coordinate { Row = 8, Column = 6 },
                        new Coordinate { Row = 8, Column = 7 },
                        new Coordinate { Row = 8, Column = 8 }
                    }
                }
            }
        };
    }
}
