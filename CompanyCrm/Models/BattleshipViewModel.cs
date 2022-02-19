using CustomerModels.Battleship;
using System.Collections.Generic;

namespace CompanyCrm.Models
{
    public class BattleshipViewModel
    {
        public List<Ship> UserFleet { get; set; } = new List<Ship>
        {
            new Ship(ShipName.Destroyer) { BelongsToUser = true },
            new Ship(ShipName.Submarine) { BelongsToUser = true },
            new Ship(ShipName.Cruiser) { BelongsToUser = true },
            new Ship(ShipName.Battleship) { BelongsToUser = true },
            new Ship(ShipName.Carrier) { BelongsToUser = true }
        };

        public List<List<Coordinate>> CoordinateLists { get; set; } = new List<List<Coordinate>>
        {
            new List<Coordinate>
            {
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 }
            },
            new List<Coordinate>
            {
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 }
            },
            new List<Coordinate>
            {
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 }
            },
            new List<Coordinate>
            {
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 }
            },
            new List<Coordinate>
            {
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 },
                new Coordinate { Row = 0, Column = 0 }
            }
        };
    }
}