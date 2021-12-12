using AdventOfCode2021.Extensions;
using AdventOfCode2021.Importers;

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
            var grid = GenerateGrid(input);

            return GetLowPoints(grid).Sum(p => p.Value + 1);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = GenerateGrid(input);
            var basins = GetLowPoints(grid)
                .Select(point => GetBasinSize(point.X, point.Y, grid))
                .OrderByDescending(b => b)
                .Take(3)
                .Aggregate((a, b) => a * b);

            return basins;
        }

        private int GetBasinSize(int x, int y, int[,] grid)
        {
            var points = new List<(int X, int Y)>();
            GetPoints(x, y, grid, ref points);

            return points.Count;
        }

        private void GetPoints(int x, int y, int[,] grid, ref List<(int X, int Y)> points)
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

        private IEnumerable<(int X, int Y, int Value)> GetLowPoints(int[,] grid)
        {           
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (!grid.Adjacent(i, j, grid[i, j]).Any())
                    {
                        yield return (i, j, grid[i, j]);
                    }
                }
            }
        }

        private int[,] GenerateGrid(string[] input)
        {
            var width = input[0].Length;
            var height = input.Length;

            var result = new int[width, height];

            for (var j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result[i, j] = int.Parse($"{input[j][i]}");
                }
            }

            return result;
        }
    }
}
