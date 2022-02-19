using CustomerModels.Battleship;
using System.Collections.Generic;

namespace CompanyCrm.Models
{
    public class BattleshipViewModel_V2
    {
        public BattleshipViewModel_V2()
        {
            UserShips = new Dictionary<ShipName, List<Coordinate>>
            {
                { ShipName.Destroyer, new List<Coordinate>(2) },
                //{ ShipName.Destroyer, new List<Coordinate>
                //    {
                //        new Coordinate { Row = 0, Column = 0 },
                //        new Coordinate { Row = 0, Column = 0 }
                //    }
                //},
                { ShipName.Cruiser, new List<Coordinate>(3) },
                //{ ShipName.Cruiser, new List<Coordinate>
                //    {
                //        new Coordinate { Row = 0, Column = 0 },
                //        new Coordinate { Row = 0, Column = 0 },
                //        new Coordinate { Row = 0, Column = 0 }
                //    }
                //},
                //{ ShipName.Battleship, new List<Coordinate>(4);
                { ShipName.Battleship, new List<Coordinate>(4) }
                    //{
                    //    new Coordinate { Row = 0, Column = 0 },
                    //    new Coordinate { Row = 0, Column = 0 },
                    //    new Coordinate { Row = 0, Column = 0 },
                    //    new Coordinate { Row = 0, Column = 0 }
                    //}
                //};
            };
        }

        public Dictionary<ShipName, List<Coordinate>> UserShips { get; set; }
    }
}