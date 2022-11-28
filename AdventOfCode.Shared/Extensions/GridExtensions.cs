using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class GridExtensions
    {
        public static int[,] ToGrid(this string[] input)
        {
            var grid = new int[input[0].Length, input.Length];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    grid[i, j] = int.Parse($"{input[j][i]}");
                }
            }

            return grid;
        }

        public static IEnumerable<Point> Adjacent(this int[,] grid, int x, int y, bool allowDiagonal = false)
        {
            return Adjacent(grid, x, y, int.MaxValue, allowDiagonal);
        }

        public static IEnumerable<Point> Adjacent(this int[,] grid, int x, int y, int compare, bool allowDiagonal = false)
        {
            if (y > 0 && grid[x, y - 1] <= compare)
            {
                yield return new Point(x, y - 1);
            }
            if (y < grid.GetLength(1) - 1 && grid[x, y + 1] <= compare)
            {
                yield return new Point(x, y + 1);
            }
            if (x > 0 && grid[x - 1, y] <= compare)
            {
                yield return new Point(x - 1, y);
            }
            if (x < grid.GetLength(0) - 1 && grid[x + 1, y] <= compare)
            {
                yield return new Point(x + 1, y);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0 && grid[x - 1, y - 1] <= compare)
                {
                    yield return new Point(x - 1, y - 1);
                }
                if (x > 0 && y < grid.GetLength(1) - 1 && grid[x - 1, y + 1] <= compare)
                {
                    yield return new Point(x - 1, y + 1);
                }
                if (x < grid.GetLength(0) - 1 && y > 0 && grid[x + 1, y - 1] <= compare)
                {
                    yield return new Point(x + 1, y - 1);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && grid[x + 1, y + 1] <= compare)
                {
                    yield return new Point(x + 1, y + 1);
                }
            }
        }
    }
}
