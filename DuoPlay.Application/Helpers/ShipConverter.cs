using Newtonsoft.Json;

namespace DuoPlay.Application.Helpers
{
    public static class ShipConverter
    {
        public static string ToJson(this List<BattleShip.Domain.Cell?> cells) =>
             JsonConvert.SerializeObject(cells
                .Where(c => c != null)
                .Select(c => new Cell()
                {
                    IsDead = false,
                    Y = c!.Point.Y,
                    X = c.Point.X
                })
                .ToList());
    }
}
