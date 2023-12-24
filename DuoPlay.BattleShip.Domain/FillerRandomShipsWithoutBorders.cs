using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.BattleShip.Domain
{
    public class FillerRandomShipsWithoutBorders
    {
        private static readonly Random Random = new Random();

        public static Cell[,] FillShip(Cell[,] cells, Ship ship)
        {
            while (ship.Decks.Count != ship.Length)
            {
                var y = Random.Next(cells.GetLength(0) - 1);
                var x = Random.Next(cells.GetLength(1) - 1);
                if (CanFillShipUp(cells, ship.Length, y, x) && Random.Next(2) == 0)
                {
                    FillShipUp(cells, ship, y, x);
                }
                if (CanFillShipRight(cells, ship.Length, y, x) && Random.Next(2) == 1)
                {
                    FillShipRight(cells, ship, y, x);
                }
            }
            return cells;
        }

        private static void FillShipRight(Cell[,] cells, Ship ship, int y, int x)
        {
            FillAroundShipRight(cells, ship.Length, y, x);
            for (var i = 0; i < ship.Length; i++)
            {
                cells[y, x + i].State = CellState.BusyDeck;
                ship.PutDeck(y, x + i);
            }
        }

        private static bool CanFillShipRight(Cell[,] cells, int shipLength, int y, int x)
        {
            var areaSize = cells.GetLength(0) - 1;
            if (x + shipLength >= areaSize || IsCellInSideLine(cells, y, x) || !CanFillAroundShipRight(cells, shipLength, y, x))
                return false;
            for (var i = 0; i < shipLength; i++)
                if (cells[y, x + i].State != CellState.Empty) return false;
            return true;
        }

        private static void FillAroundShipRight(Cell[,] cells, int shipLength, int y, int x)
        {
            var upPosY = y - 1;
            var downPosY = y + 1;
            var posX = x - 1;
            cells[y, posX].State = CellState.BusyDeckNearby;
            cells[y, posX + shipLength + 1].State = CellState.BusyDeckNearby;
            for (var i = 0; i < shipLength + 2; i++)
            {
                cells[upPosY, posX].State = CellState.BusyDeckNearby;
                cells[downPosY, posX].State = CellState.BusyDeckNearby;
                posX++;
            }
        }

        private static bool CanFillAroundShipRight(Cell[,] cells, int shipLength, int y, int x)
        {
            if (IsCellLeftOfShipAndRightOfShipBusyDeck(cells, shipLength, y, x)) return false;
            x--;
            for (var i = 0; i < shipLength + 2; i++)
            {
                if (IsCellUpAndDownBusyDeck(cells, y, x + i)) return false;
            }
            return true;
        }

        private static bool IsCellUpAndDownBusyDeck(Cell[,] cells, int y, int x)
        {
            return cells[y - 1, x].State == CellState.BusyDeck ||
                cells[y + 1, x].State == CellState.BusyDeck;
        }

        private static bool IsCellLeftOfShipAndRightOfShipBusyDeck(Cell[,] cells, int shipLength, int y, int x)
        {
            return cells[y, x - 1].State == CellState.BusyDeck ||
                cells[y, x + shipLength].State == CellState.BusyDeck;
        }

        private static void FillShipUp(Cell[,] cells, Ship ship, int y, int x)
        {
            FillAroundShipUp(cells, ship.Length, y, x);
            for (var i = 0; i < ship.Length; i++)
            {
                cells[y - i, x].State = CellState.BusyDeck;
                ship.PutDeck(y - i, x);
            }
        }

        private static bool CanFillShipUp(Cell[,] cells, int shipLength, int y, int x)
        {
            if (y - shipLength <= 0 || IsCellInSideLine(cells, y, x) || !CanFillAroundShipUp(cells, shipLength, y, x))
                return false;
            for (var i = 0; i < shipLength; i++)
            {
                if (cells[y - i, x].State != CellState.Empty) return false;
            }
            return true;
        }

        private static void FillAroundShipUp(Cell[,] cells, int shipLength, int y, int x)
        {
            var rightPosX = x + 1;
            var leftPosX = x - 1;
            var posY = y + 1;
            cells[posY, x].State = CellState.BusyDeckNearby;
            cells[posY - (shipLength + 1), x].State = CellState.BusyDeckNearby;
            for (var i = 0; i < shipLength + 2; i++)
            {
                cells[posY, rightPosX].State = CellState.BusyDeckNearby;
                cells[posY, leftPosX].State = CellState.BusyDeckNearby;
                posY--;
            }
        }

        private static bool CanFillAroundShipUp(Cell[,] cells, int shipLength, int y, int x)
        {
            if (IsCellUpOfShipAndDownOfShipBusyDeck(cells, shipLength, y, x)) return false;
            y++;
            for (var i = 0; i > shipLength + 2; i++)
            {
                if (IsCellRightAndLeftBusyDeck(cells, y - i, x)) return false;
            }
            return true;
        }

        private static bool IsCellRightAndLeftBusyDeck(Cell[,] cells, int y, int x)
        {
            return cells[y, x - 1].State == CellState.BusyDeck ||
                cells[y, x + 1].State == CellState.BusyDeck;
        }

        private static bool IsCellUpOfShipAndDownOfShipBusyDeck(Cell[,] cells, int shipLength, int y, int x)
        {
            return cells[y + 1, x].State == CellState.BusyDeck ||
                cells[y - shipLength, x].State == CellState.BusyDeck;
        }

        private static bool IsCellInSideLine(Cell[,] cells, int y, int x)
        {
            return x == 0 || y == cells.GetLength(1) - 1 || y == 0 || x == cells.GetLength(0) - 1;
        }
    }
}
