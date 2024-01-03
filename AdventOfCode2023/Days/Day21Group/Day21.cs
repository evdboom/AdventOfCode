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
            return Walk(start, grid, [Steps]).First();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var steps = 26501365;
            var (grid, start) = GetGrid(input);
            var width = grid.GetLength(0);
            var rem = 26501365 % width;
            var values = Walk(start, grid, [rem, rem + width, rem + width * 2]).ToList();

            var matrix = new double[4, 3];
            FillRow(0, 0, 1, values[0], 0, matrix);
            FillRow(1, 1, 1, values[1], 1, matrix);
            FillRow(4, 2, 1, values[2], 2, matrix);

            var result = matrix.GaussianElimination();

            var x = steps / width;
            var a = (long)Math.Round(result[0]);
            var b = (long)Math.Round(result[1]);
            var c = (long)Math.Round(result[2]);

            return a * x * x + b * x + c;
        }

        private void FillRow(int a, int b, int c, long value, int row, double[,] matrix)
        {
            matrix[0, row] = a;
            matrix[1, row] = b;
            matrix[2, row] = c;
            matrix[3, row] = value;
        }

        private IEnumerable<long> Walk(Point start, bool[,] grid, List<int> steps)
        {           
            HashSet<Point> options = [start];
            if (steps.Contains(0))
            {
                yield return options.Count;
            }
            var max = steps.Max();
            for (int i = 1; i <= max; i++)
            {
                options = options
                    .SelectMany(option => GetOptions(option, grid))
                    .ToHashSet();

                if (steps.Contains(i))
                {
                    yield return options.Count;
                }
            }
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