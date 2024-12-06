using System.Collections;
using System.Drawing;
using System.Text;

namespace AdventOfCode.Shared.Grid
{
    public class Grid<TValue> : IEnumerable<GridCell<TValue>>
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

        public GridCell<TValue> GetCell(Point point) => new(point, this[point]);

        public IEnumerator<GridCell<TValue>> GetEnumerator()
        {
            return new GridEnumerator<TValue>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var stringbuilder = new StringBuilder();
            var row = 0;
            foreach (var cell in this)
            {
                if (cell.Point.Y != row)
                {
                    stringbuilder.AppendLine();
                    row = cell.Point.Y;
                }

                var value = cell.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    stringbuilder.Append(value[0]);
                }
                else
                {
                    stringbuilder.Append(' ');
                }                    
            }
            return stringbuilder.ToString();
        }
    }
}
