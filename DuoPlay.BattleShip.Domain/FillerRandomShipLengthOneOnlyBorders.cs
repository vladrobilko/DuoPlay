using DuoPlay.BattleShip.Domain.Enums;

namespace DuoPlay.BattleShip.Domain
{
    public class FillerRandomShipLengthOneOnlyBorders
    {
        private static readonly Random Random = new Random();

        public static Cell[,] FillShips(Cell[,] cells, Ship ship)
        {
            var firstCoordinate = 1;

            while (firstCoordinate < 9)
            {
                if (CanFillUpHorizontalLine(cells, firstCoordinate) && Random.Next(4) == 0)
                {
                    FillUpHorizontalLine(cells, ship, firstCoordinate); break;
                }
                if (CanFillDownHorizontalLine(cells, firstCoordinate) && Random.Next(4) == 1)
                {
                    FillDownHorizontalLine(cells, ship, firstCoordinate); break;
                }
                if (CanFillLeftVerticalLine(cells, firstCoordinate) && Random.Next(4) == 2)
                {
                    FillLeftVerticalLine(cells, ship, firstCoordinate); break;
                }
                if (CanFillRightVerticalLine(cells, firstCoordinate) && Random.Next(4) == 3)
                {
                    FillRightVerticalLine(cells, ship, firstCoordinate); break;
                }
                firstCoordinate++;
            }
            if (firstCoordinate == 9)
            {
                FillShips(cells, ship);
            }
            return cells;
        }

        private static void FillUpHorizontalLine(Cell[,] cells, Ship ship, int x)
        {
            cells[0, x + 1].State = CellState.BusyDeckNearby;
            cells[0, x - 1].State = CellState.BusyDeckNearby;
            cells[0, x].State = CellState.BusyDeck;
            ship.PutDeck(0, x);
        }

        private static bool CanFillUpHorizontalLine(Cell[,] cells, int x)
        {
            return (cells[0, x].State == CellState.Empty) &&
                   (cells[0, x - 1].State == CellState.Empty) &&
                   (cells[1, x - 1].State != CellState.BusyDeck) &&
                   (cells[1, x].State != CellState.BusyDeck) &&
                   (cells[1, x + 1].State != CellState.BusyDeck);
        }

        private static void FillDownHorizontalLine(Cell[,] cells, Ship ship, int x)
        {
            cells[cells.GetLength(0) - 1, x - 1].State = CellState.BusyDeckNearby;
            cells[cells.GetLength(0) - 1, x + 1].State = CellState.BusyDeckNearby;
            cells[cells.GetLength(0) - 1, x].State = CellState.BusyDeck;
            ship.PutDeck(cells.GetLength(0) - 1, x);
        }

        private static bool CanFillDownHorizontalLine(Cell[,] cells, int x)
        {
            return (cells[cells.GetLength(0) - 1, x].State == CellState.Empty) &&
                   (cells[cells.GetLength(0) - 2, x - 1].State != CellState.BusyDeck) &&
                   (cells[cells.GetLength(0) - 2, x].State != CellState.BusyDeck) &&
                   (cells[cells.GetLength(0) - 2, x + 1].State != CellState.BusyDeck);
        }

        private static void FillLeftVerticalLine(Cell[,] cells, Ship ship, int y)
        {
            cells[y - 1, 0].State = CellState.BusyDeckNearby;
            cells[y + 1, 0].State = CellState.BusyDeckNearby;
            cells[y, 0].State = CellState.BusyDeck;
            ship.PutDeck(y, 0);
        }

        private static bool CanFillLeftVerticalLine(Cell[,] cells, int y)
        {
            return (cells[y, 0].State == CellState.Empty) &&
                (cells[y - 1, 1].State != CellState.BusyDeck) &&
                (cells[y + 1, 1].State != CellState.BusyDeck) &&
                (cells[y, 1].State != CellState.BusyDeck);
        }

        private static void FillRightVerticalLine(Cell[,] cells, Ship ship, int y)
        {
            cells[y - 1, cells.GetLength(0) - 1].State = CellState.BusyDeckNearby;
            cells[y + 1, cells.GetLength(0) - 1].State = CellState.BusyDeckNearby;
            cells[y, cells.GetLength(0) - 1].State = CellState.BusyDeck;
            ship.PutDeck(y, cells.GetLength(0) - 1);
        }

        private static bool CanFillRightVerticalLine(Cell[,] cells, int y)
        {
            return (cells[y, cells.GetLength(0) - 1].State == CellState.Empty) &&
                (cells[y - 1, cells.GetLength(0) - 2].State != CellState.BusyDeck) &&
                (cells[y + 1, cells.GetLength(0) - 2].State != CellState.BusyDeck) &&
                (cells[y, cells.GetLength(0) - 2].State != CellState.BusyDeck);
        }
    }
}
