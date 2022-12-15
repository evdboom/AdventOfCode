using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day14 : Day
    {
        public Day14(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 14;
        protected override long ProcessPartOne(string[] input)
        {
            var grid = BuildGrid(input, out int minX);
            var count = 0;
            while(DropSand(grid, minX))
            {
                count++;
            }

            return count;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = BuildGrid(input, out int minX);
            var y = grid.GetLength(1) - 1;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                grid[i, y] = true;
            }

            var count = 0;
            while (!grid[500 - minX, 0])
            {
                DropSand(grid, minX);
                count++;
            }

            return count;
        }

        private bool DropSand(bool[,] grid, int minX)
        {
            var sandX = 500 - minX;
            var sandY = 0;
            while (true)
            {
                if (sandY == grid.GetLength(1) - 1)
                {
                    return false;                    
                }
                else if (!grid[sandX, sandY + 1])
                {
                    sandY++;
                }
                else if (sandX == 0)
                {
                    return false;
                }
                else if (!grid[sandX - 1, sandY + 1])
                {
                    sandX--;
                    sandY++;
                }
                else if (sandX == grid.GetLength(0) - 1)
                {
                    return false;
                }
                else if (!grid[sandX + 1, sandY + 1])
                {
                    sandX++;
                    sandY++;
                }
                else
                {
                    grid[sandX, sandY] = true;
                    return true;
                }
            }
        }

        private bool[,] BuildGrid(string[] input, out int minX)
        {
            var numbers = input
                .Select(line => line
                    .Split(" -> ")
                    .Select(part => part
                        .Split(',')
                        .Select(int.Parse)
                        .ToArray())
                    .ToList());

            minX = int.MaxValue;
            var maxX = 0;
            var maxY = 0;

            foreach (var pointGroup in numbers)
            {
                foreach (var point in pointGroup)
                {
                    if (point[0] < minX)
                    {
                        minX = point[0];
                    }
                    if (point[0] > maxX)
                    {
                        maxX = point[0];
                    }
                    if (point[1] > maxY)
                    {
                        maxY = point[1];
                    }
                }
            }

            minX = Math.Min(500 - maxY - 5, minX);
            maxX = Math.Max(500 + maxY + 5, maxX);

            var result = new bool[maxX - minX + 1, maxY + 3];

            foreach (var pointGroup in numbers)
            {
                for (int i = 1; i < pointGroup.Count; i++)
                {
                    var from = pointGroup[i - 1];
                    var to = pointGroup[i];
                    var difX = Math.Abs(from[0] - to[0]);
                    var difY = Math.Abs(from[1] - to[1]);
                    var start = difY == 0
                        ? Math.Min(from[0], to[0])
                        : Math.Min(from[1], to[1]);
                    if (difX == 0)
                    {
                        for (int y = start; y < start + difY + 1; y++)
                        {
                            result[from[0] - minX, y] = true;
                        }
                    }
                    if (difY == 0)
                    {
                        for (int x = start; x < start + difX + 1; x++)
                        {
                            result[x - minX, from[1]] = true;
                        }
                    }

                }
            }

            return result;
        }
    }
}
