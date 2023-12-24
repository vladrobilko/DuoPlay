using DuoPlay.BattleShip.Domain.Enums;
using DuoPlay.BattleShip.Domain;

namespace DuoPlay.Application.Helpers
{
    public static class StringConverter
    {
        public static GameType ToGameType(this string value)
        {
            if (Enum.TryParse(value, out GameType gameType)) return gameType;
            throw new ArgumentException($"Invalid GameType: {value}");
        }

        public static PlayArea ToPlayArea(this string stringPlayArea)
        {
            var playArea = new PlayArea();
            for (int i = 0; i < stringPlayArea.Length; i++)
                playArea[i].State = stringPlayArea[i].ToCellState();
            return playArea;
        }

        private static CellState ToCellState(this char cell)
        {
            switch (cell)
            {
                case ' ':
                    return CellState.Empty;
                case '#':
                    return CellState.BusyDeck;
                case '-':
                    return CellState.BusyDeckNearby;
                case '*':
                    return CellState.HasMiss;
                case 'x':
                    return CellState.HasHit;
                default: return CellState.Empty;
            }
        }
    }
}
