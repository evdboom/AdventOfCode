using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day09 : Day
    {
        public Day09(IFileImporter importer) : base(importer)
        {
        }

        private const int MaximumBasinValue = 8;
        protected override int DayNumber => 9;

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
                foreach (var point in Adjacent(x, y, grid, MaximumBasinValue))
                {
                    GetPoints(point.X, point.Y, grid, ref points);
                }
            }
        }

        private IEnumerable<(int X, int Y)> Adjacent(int x, int y, int[,] grid, int compare)
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
        }

        private IEnumerable<(int X, int Y, int Value)> GetLowPoints(int[,] grid)
        {           
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (!Adjacent(i, j, grid, grid[i, j]).Any())
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
