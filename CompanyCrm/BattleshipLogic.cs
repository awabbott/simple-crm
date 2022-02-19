using CustomerModels.Battleship;
using System;
using System.Collections.Generic;

namespace CompanyCrm
{
    public static class BattleshipLogic
    {
        public static List<Ship> PlaceShips()
        {
            var ships = new List<Ship>
            {
                new Ship(ShipName.Destroyer),
                new Ship(ShipName.Submarine),
                new Ship(ShipName.Cruiser),
                new Ship(ShipName.Battleship),
                new Ship(ShipName.Carrier)
            };

            Random rnd = new Random();
            bool validPlacement = false;
            List<Coordinate> fleetCoordinates = new List<Coordinate>();

            while (!validPlacement)
            {
                foreach (var ship in ships)
                {
                    ship.Coordinates.Clear();

                    int startingRow = rnd.Next(9);
                    int startingColumn = rnd.Next(9);
                    bool orientation = rnd.Next(2) == 0;

                    ship.Coordinates.Add(new Coordinate
                    {
                        Row = startingRow,
                        Column = startingColumn
                    });

                    ship.Coordinates = AddRemainingCoordinates(ship, orientation);
                    fleetCoordinates.AddRange(ship.Coordinates);
                }

                if (AllCoordinatesAreInBounds(fleetCoordinates) && ShipsDoNotOverlap(fleetCoordinates))
                {
                    validPlacement = true;
                }
                else
                {
                    fleetCoordinates.Clear();
                }
            }

            return ships;
        }


        public static bool AllCoordinatesAreInBounds(List<Coordinate> coordinateList)
        {
            foreach (var coordinate in coordinateList)
            {
                if (coordinate.Row < 0 || coordinate.Row > 9
                    || coordinate.Column < 0 || coordinate.Column > 9)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ShipsDoNotOverlap(List<Coordinate> coordinateList)
        {
            var distinctCoordinates = new HashSet<int>();

            foreach (var coordinate in coordinateList)
            {
                int rowAndColumn = int.Parse(coordinate.Row.ToString() + coordinate.Column.ToString());
                distinctCoordinates.Add(rowAndColumn);
            }
            return distinctCoordinates.Count == coordinateList.Count;
        }

        public static List<Coordinate> AddRemainingCoordinates(Ship ship, bool shipIsVertical)
        {
            if (shipIsVertical)
            {
                for (int i = 1; i < ship.Length; i++)
                {
                    ship.Coordinates.Add(new Coordinate
                    {
                        Row = ship.Coordinates[0].Row + i,
                        Column = ship.Coordinates[0].Column
                    });
                }
            }
            else
            {
                for (int i = 1; i < ship.Length; i++)
                {
                    ship.Coordinates.Add(new Coordinate
                    {
                        Row = ship.Coordinates[0].Row,
                        Column = ship.Coordinates[0].Column + i
                    });
                }
            }

            return ship.Coordinates;
        }
    }
}