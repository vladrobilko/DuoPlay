namespace DuoPlay.BattleShip.Domain
{
    public class Point
    {
        public int Y { get; set; }

        public int X { get; set; }

        public Point(int y, int x)
        {
            this.Y = y;
            this.X = x;
        }
    }
}
