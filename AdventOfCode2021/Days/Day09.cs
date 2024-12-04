using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2021.Days
{
    public class Day09 : Day
    {
        public Day09(IFileImporter importer) : base(importer)
        {
        }

        private const int MaximumBasinValue = 8;
        public override int DayNumber => 9;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToIntGrid();

            return GetLowPoints(grid).Sum(p => p.Value + 1);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToIntGrid();
            var basins = GetLowPoints(grid)
                .Select(point => GetBasinSize(point.X, point.Y, grid))
                .OrderByDescending(b => b)
                .Take(3)
                .Aggregate((a, b) => a * b);

            return basins;
        }

        private int GetBasinSize(int x, int y, Grid<int> grid)
        {
            var points = new List<(int X, int Y)>();
            GetPoints(x, y, grid, ref points);

            return points.Count;
        }

        private void GetPoints(int x, int y, Grid<int> grid, ref List<(int X, int Y)> points)
        {
            if (!points.Any(p => p.X == x && p.Y == y))
            {
                points.Add((x, y));
                foreach (var point in grid.Adjacent(x, y, MaximumBasinValue))
                {
                    GetPoints(point.X, point.Y, grid, ref points);
                }
            }
        }

        private IEnumerable<(int X, int Y, int Value)> GetLowPoints(Grid<int> grid)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    if (!grid.Adjacent(i, j, grid[i, j]).Any())
                    {
                        yield return (i, j, grid[i, j]);
                    }
                }
            }
        }
    }
}
