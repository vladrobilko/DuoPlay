namespace DuoPlay.BattleShip.Domain
{
    public class ShootModel
    {
        public int ShootCoordinateY { get; set; }

        public int ShootCoordinateX { get; set; }

        public string? NamePlayer { get; set; }

        public string? NameSession { get; set; }

        public ShootModel(int shootCoordinateY, int shootCoordinateX, string? playerName, string? sessionName)
        {
            ShootCoordinateY = shootCoordinateY;
            ShootCoordinateX = shootCoordinateX;
            NamePlayer = playerName;
            NameSession = sessionName;
        }
    }
}
