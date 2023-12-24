using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.Application.Helpers
{
    public static class CellStateConverter
    {
        public static string ToStringWithAllCell(this CellState cell)
        {
            switch (cell)
            {
                case CellState.Empty:
                    return " ";
                case CellState.BusyDeck:
                    return "#";
                case CellState.BusyDeckNearby:
                    return "-";
                case CellState.HasMiss:
                    return "*";
                case CellState.HasHit:
                    return "x";
                default: return " ";
            }
        }
    }
}
