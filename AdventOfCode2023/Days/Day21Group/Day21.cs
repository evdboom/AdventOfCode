using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2023.Days
{
    public class Day21 : Day
    {
        public Day21(IFileImporter importer) : base(importer)
        {
        }

        public int Steps { get; set; } = 64;
        public override int DayNumber => 21;

        protected override long ProcessPartOne(string[] input)
        {
            var (grid, start) = GetGrid(input);

            HashSet<Point> options = [start];
            var cache = new Dictionary<Point, IEnumerable<Point>>();
            options = Walk(options, grid, Steps, cache);

            return options.Count;
        }

        // wrong 1971404079 (too low)
        protected override long ProcessPartTwo(string[] input)
        {
            var (grid, start) = GetGrid(input);
            var cache = new Dictionary<Point, IEnumerable<Point>>();
            var width = grid.GetLength(0);
            var half = width / 2;
            var toEdge = Walk([start], grid, half, cache);
            var nextEdge = Walk([start], grid, half + width, cache);
            var thirdEdge = Walk([start], grid, half + width + width, cache);

            var steps = (26501365 - half) / width;

            var c = toEdge.Count;
            var b = (4 * nextEdge.Count - thirdEdge.Count - 3 * c) / 2;
            var a = nextEdge.Count - c - b;

            return a * steps * steps + b * steps + c;
        }

        private HashSet<Point> Walk(HashSet<Point> options, bool[,] grid, int steps, Dictionary<Point, IEnumerable<Point>> cache)
        {
            for (int i = 0; i < steps; i++)
            {
                options = options
                    .SelectMany(option => cache.TryGetValue(option, out var fromCache)
                        ? fromCache
                        : GetOptions(option, grid))
                    .ToHashSet();
            }

            return options;
        }

        private IEnumerable<Point> GetOptions(Point option, bool[,] grid)
        {
            return option
                .Adjacent()
                .Where(adjacent =>
                {
                    while (adjacent.X < 0)
                    {
                        adjacent.X += grid.GetLength(0);
                    }
                    while (adjacent.Y < 0)
                    {
                        adjacent.Y += grid.GetLength(1);
                    }
                    return grid[adjacent.X % grid.GetLength(0), adjacent.Y % grid.GetLength(1)];
                });
        }

        private (bool[,] Grid, Point Start) GetGrid(string[] input)
        {
            var grid = new bool[input[0].Length, input.Length];
            Point? start = null;
            for (int j = 0; j < input.Length; j++)
            {
                for (int i = 0; i < input[j].Length; i++)
                {
                    if (input[j][i] == 'S')
                    {
                        start = new Point(i, j);
                        grid[i, j] = true;
                    }
                    else
                    {
                        grid[i, j] = input[j][i] == '.';
                    }
                }
            }

            if (start is null)
            {
                throw new InvalidOperationException("Start not found");
            }

            return (grid, start.Value);
        }
    }
}