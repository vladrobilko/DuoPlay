namespace DuoPlay.BattleShip.Domain
{
    public class ShipConfigure
    {
        public int CountShip { get; set; }

        public int LengthShip { get; set; }

        public ShipConfigure(int count, int length)
        {
            CountShip = count;
            LengthShip = length;
        }
    }
}
