using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerModels.Battleship
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool ShotReceived { get; set; }
        public Coordinate Coordinate { get; set; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
