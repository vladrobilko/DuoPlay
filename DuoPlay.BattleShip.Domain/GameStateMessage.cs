namespace DuoPlay.BattleShip.Domain
{
    public static class GameStateMessage
    {
        public static string WhoShoot(string namePlayer) => $"{namePlayer} turn to shoot";

        public static string WhoHitAndShoot(string namePlayer) => $"{namePlayer} hit the target.";

        public static string WhoMissAndShoot(string namePlayerWhoMiss) => $"{namePlayerWhoMiss} miss the target.";

        public static string WhoKillAndShoot(string namePlayer) => $"{namePlayer} kill the ship.";

        public static string WhoShootSameCellAndWhoShoot(string namePlayer) => $"{namePlayer} already shot this area.";

        public static string WhoWinGame(string namePlayer) => $"{namePlayer} win the game.";
    }
}
