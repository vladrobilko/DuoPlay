using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.BattleShip.Domain
{
    public interface IPlayer
    {
        string NamePlayer { get; }

        List<Ship> Ships { get; }

        Point GetNextValidShootTarget();

        ShootResultType OnShoot(Point target);

        PlayArea GetPlayArea();
    }
}