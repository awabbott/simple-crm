using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerModels.Battleship
{
    public class Ship
    {
        public Ship(ShipName name)
        {
            Name = name;
            ImageUrl = "images/" + Name + ".png";
            Length = name switch
            {
                ShipName.Carrier => 5,
                ShipName.Battleship => 4,
                ShipName.Cruiser => 3,
                ShipName.Submarine => 3,
                ShipName.Destroyer => 2,
                _ => 0,
            };
            Coordinates = new List<Coordinate>(Length);
        }
        public int ShipId { get; set; }
        public ShipName Name { get; private set; }
        public int Length { get; private set; }
        public int HitCount { get; set; }
        public bool IsSunk { get; set; }
        public bool BelongsToUser { get; set; }
        public string ImageUrl { get; private set; }
        public int GameId { get; set; }
        public List<Coordinate> Coordinates { get; set; }
    }
}