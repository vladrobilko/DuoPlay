using DuoPlay.BattleShip.Domain;

namespace DuoPlay.Application.Helpers
{
    public static class PlayAreaConverter
    {
        public static string ToStringForDb(this PlayArea playArea) => string.Join("", playArea.Select(p => p.State.ToStringWithAllCell()));
    }
}
