using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerModels.Battleship
{
    public class GameBoard
    {
        public bool[][] Grid { get; set; }
        public int[][] ShipCoordinates { get; set; }
        public bool AllShipsPresent { get; set; }
        public bool AllShipsSunk { get; set; }
    }
}
