using System.Collections;

namespace DuoPlay.BattleShip.Domain
{
    public class PlayArea : IEnumerable<Cell>
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public Cell[,] Cells { get; set; }

        public Cell this[int index]
        {
            get => Cells[index / Width, index % Width];
        }

        public PlayArea(int height = 10, int width = 10)
        {
            this.Height = height;
            this.Width = width;
            this.Cells = new Cell[height, width];
            FillArrayWithCell(height, width);
        }

        public PlayArea(PlayArea playArea)
        {
            Height = playArea.Height;
            Width = playArea.Width;
            Cells = playArea.Cells;
        }

        private void FillArrayWithCell(int height, int width)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    Cells[i, j] = new Cell(i, j);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    yield return Cells[i, j];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
