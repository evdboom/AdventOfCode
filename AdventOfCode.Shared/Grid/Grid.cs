using System.Drawing;

namespace AdventOfCode.Shared.Grid
{
    public class Grid<TValue>
    {
        private readonly TValue[,] _grid;
        public Grid(int width, int height)
        {
            _grid = new TValue[width, height];
        }
        public TValue this[int x, int y]
        {
            get => _grid[x, y];
            set => _grid[x, y] = value;
        }

        public TValue this[Point point]
        {
            get => _grid[point.X, point.Y];
            set => _grid[point.X, point.Y] = value;
        }

        public int Width => _grid.GetLength(0);
        public int Height => _grid.GetLength(1);
        public long Size => Width * Height;
        public int MaxX => Width - 1;
        public int MaxY => Height - 1;
    }
}
