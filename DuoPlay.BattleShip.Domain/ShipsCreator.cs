namespace DuoPlay.BattleShip.Domain
{
    public class ShipsCreator
    {
        public static List<Ship> CreateShips(List<ShipConfigure> configureShips)
        {
            var ships = new List<Ship>();

            for (var i = 0; i < configureShips.Count; i++)
                for (var j = 0; j < configureShips[i].CountShip; j++)
                    ships.Add(new Ship(configureShips[i].LengthShip));
            return ships;
        }

        public static List<Ship> CreateShipsForDefaultGame() => CreateShips(GetConfigureShips());

        private static List<ShipConfigure> GetConfigureShips()
        {
            return new List<ShipConfigure>() {
                new ShipConfigure(1, 4),
                new ShipConfigure(2, 3),
                new ShipConfigure(3, 2),
                new ShipConfigure(4, 1)
            };
        }
    }
}
