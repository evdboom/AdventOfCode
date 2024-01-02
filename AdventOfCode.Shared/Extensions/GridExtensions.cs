using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class GridExtensions
    {
        public static int[,] ToIntGrid(this string[] input)
        {
            return ToGrid(input, (c) => int.Parse($"{c}"));
        }

        public static T[,] ToGrid<T>(this string[] input, Func<char, T> parse)
        {
            var grid = new T[input[0].Length, input.Length];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    grid[i, j] = parse(input[j][i]);
                }
            }

            return grid;
        }
        
        public static IEnumerable<Point> Adjacent(this Point point, bool allowDiagonal = false)
        {
            yield return point with { X = point.X + 1 };
            yield return point with { X = point.X - 1 };
            yield return point with { Y = point.Y + 1 };
            yield return point with { Y = point.Y - 1 };

            if (allowDiagonal)
            {
                yield return point with { X = point.X + 1, Y = point.Y + 1 };
                yield return point with { X = point.X - 1, Y = point.Y + 1 };
                yield return point with { X = point.X + 1, Y = point.Y - 1 };
                yield return point with { X = point.X - 1, Y = point.Y - 1 };
            }
        }

        public static IEnumerable<Point> Adjacent<T>(this T[,] grid, Point point, bool allowDiagonal = false)
        {
            return Adjacent(grid, point.X, point.Y, allowDiagonal);
        }

        public static IEnumerable<Point> Adjacent<T>(this T[,] grid, int x, int y, bool allowDiagonal = false)
        {
            if (y > 0)
            {
                yield return new Point(x, y - 1);
            }
            if (y < grid.GetLength(1) - 1)
            {
                yield return new Point(x, y + 1);
            }
            if (x > 0)
            {
                yield return new Point(x - 1, y);
            }
            if (x < grid.GetLength(0) - 1)
            {
                yield return new Point(x + 1, y);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0)
                {
                    yield return new Point(x - 1, y - 1);
                }
                if (x > 0 && y < grid.GetLength(1) - 1)
                {
                    yield return new Point(x - 1, y + 1);
                }
                if (x < grid.GetLength(0) - 1 && y > 0)
                {
                    yield return new Point(x + 1, y - 1);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1)
                {
                    yield return new Point(x + 1, y + 1);
                }
            }
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
