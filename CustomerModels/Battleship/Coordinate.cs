namespace CustomerModels.Battleship
{
    public class Coordinate
    {
        public int CoordinateId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ShipId { get; set; }
    }
}