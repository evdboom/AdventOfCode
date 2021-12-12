﻿namespace AdventOfCode2021.Extensions
{
    public static class GridExtensions
    {
        public static IEnumerable<(int X, int Y)> Adjacent(this int[,] grid, int x, int y, bool allowDiagonal = false)
        {
            return Adjacent(grid, x, y, int.MaxValue, allowDiagonal);
        }

        public static IEnumerable<(int X, int Y)> Adjacent(this int[,] grid, int x, int y, int compare, bool allowDiagonal = false)
        {
            if (y > 0 && grid[x, y - 1] <= compare)
            {
                yield return (x, y - 1);
            }
            if (y < grid.GetLength(1) - 1 && grid[x, y + 1] <= compare)
            {
                yield return (x, y + 1);
            }
            if (x > 0 && grid[x - 1, y] <= compare)
            {
                yield return (x - 1, y);
            }
            if (x < grid.GetLength(0) - 1 && grid[x + 1, y] <= compare)
            {
                yield return (x + 1, y);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0 && grid[x - 1, y - 1] <= compare)
                {
                    yield return (x - 1, y - 1);
                }
                if (x > 0 && y < grid.GetLength(1) - 1 && grid[x - 1, y + 1] <= compare)
                {
                    yield return (x - 1, y + 1);
                }
                if (x < grid.GetLength(0) - 1 && y > 0 && grid[x + 1, y - 1] <= compare)
                {
                    yield return (x + 1, y - 1);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && grid[x + 1, y + 1] <= compare)
                {
                    yield return (x + 1, y + 1);
                }
            }
        }
    }
}
