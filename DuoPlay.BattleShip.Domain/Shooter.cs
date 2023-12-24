using DuoPlay.BattleShip.Domain.Enums;
namespace DuoPlay.BattleShip.Domain
{
    public class Shooter
    {
        public static ShootResultType Result(List<Ship> shipsTarget, Point pointShoot)
        {
            var ship = SearchShip(shipsTarget, pointShoot);
            var shootResultType = ship == null ? ShootResultType.Miss : ShootResultType.Hit;
            if (shootResultType == ShootResultType.Hit)
            {
                KillDeck(ship, pointShoot);
                shootResultType = IsAllShipsDead(shipsTarget) ? ShootResultType.GameOver : ShootResultType.Hit;
                if (IsShipDead(ship) && shootResultType != ShootResultType.GameOver)
                    shootResultType = ShootResultType.Kill;
            }
            return shootResultType;
        }

        private static Ship SearchShip(List<Ship> ships, Point point)
        {
            foreach (var ship in ships)
                foreach (var deck in ship.Decks)
                    if (deck != null && deck.Point.Y == point.Y && deck.Point.X == point.X)
                        return ship;
            return null;
        }

        private static Ship KillDeck(Ship ship, Point point)
        {
            for (var i = 0; i < ship.Decks.Count; i++)
            {
                if (ship.Decks[i] != null && ship.Decks[i].Point.Y == point.Y && ship.Decks[i].Point.X == point.X)
                    ship.Decks[i] = null;
            }
            return ship;
        }

        private static bool IsAllShipsDead(List<Ship> ships)
        {
            foreach (var ship in ships)
                if (!IsShipDead(ship))
                    return false;
            return true;
        }

        private static bool IsShipDead(Ship ship)
        {
            foreach (var deck in ship.Decks)
                if (deck != null)
                    return false;
            return true;
        }
    }
}
