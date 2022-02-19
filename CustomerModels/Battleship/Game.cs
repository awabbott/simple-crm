using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerModels.Battleship
{
    public class Game
    {
        public int GameId { get; set; }
        public ICollection<Ship> Ships { get; set; } = new List<Ship>();
    }
}
