using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.BattleShip.Domain
{
    public class PlayerStateModel : IPlayer
    {
        public string? NamePlayer { get; set; }

        public PlayArea? PlayArea { get; set; }

        public List<Ship>? Ships { get; set; }

        public PlayArea? EnemyPlayArea { get; set; }

        public ShootModel LastShoot { get; set; }

        public PlayerStateModel(string? playerName)
        {
            PlayArea = new PlayArea();
            EnemyPlayArea = new PlayArea();
            Ships = ShipsCreator.CreateShipsForDefaultGame();
            NamePlayer = playerName;
        }

        public void FillShips()
        {
            var randomFiller = new RandomFiller();
            randomFiller.FillShips(PlayArea?.Cells, Ships);
        }

        public PlayArea GetPlayArea() => new PlayArea(PlayArea);

        public Point GetNextValidShootTarget()
        {
            if (EnemyPlayArea?.Cells[LastShoot.ShootCoordinateY, LastShoot.ShootCoordinateX].State == CellState.HasShot)
                throw new NotFiniteNumberException();
            EnemyPlayArea!.Cells[LastShoot.ShootCoordinateY, LastShoot.ShootCoordinateX].State = CellState.HasShot;
            return new Point(LastShoot.ShootCoordinateY, LastShoot.ShootCoordinateX);
        }

        public ShootResultType OnShoot(Point target)
        {
            var shootResultType = Shooter.Result(Ships, target);
            PlayArea!.Cells[target.Y, target.X].State = CellState.HasShot;
            if (shootResultType == ShootResultType.Miss)
                PlayArea.Cells[target.Y, target.X].State = CellState.HasMiss;
            else if (shootResultType == ShootResultType.Hit || shootResultType == ShootResultType.Kill)
                PlayArea.Cells[target.Y, target.X].State = CellState.HasHit;
            return shootResultType;
        }
    }
}
