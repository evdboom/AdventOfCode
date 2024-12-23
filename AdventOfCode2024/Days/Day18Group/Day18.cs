using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.ComponentModel;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day18(IFileImporter importer, IScreenWriter writer) : Day(importer)
    {
        private readonly IScreenWriter _writer = writer;

        private string? _resultTwo;

        public int GridSize { get; set; } = 71;
        public int BytesToRead { get; set; } = 1024;

        public override int DayNumber => 18;
        public string? PartTwoResult() => _resultTwo;

        protected override long ProcessPartOne(string[] input)
        {
            var memory = GetMemorySpace(input, GridSize, BytesToRead);
            var start = new Point(0, 0);
            var end = new Point(GridSize - 1, GridSize - 1);            
            return GetPath(start, end, memory).Steps;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var memory = GetMemorySpace(input, GridSize, BytesToRead);
            var start = new Point(0, 0);
            var end = new Point(GridSize - 1, GridSize - 1);
            HashSet<Point>? visited = null;
            for (int i = BytesToRead + 1; i < input.Length; i++)
            {
                var parts = input[i].Split(',');
                var byteToAdd = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
                memory[byteToAdd] = true;

                if (visited == null || visited.Contains(byteToAdd))
                {
                    var newPath = GetPath(start, end, memory);
                    if (newPath.Steps > 0)
                    {
                        visited = newPath.Visited;
                    }
                    else
                    {
                        _resultTwo = $"{byteToAdd.X},{byteToAdd.Y}";
                        _writer.WriteLine(_resultTwo);
                        break;
                    }
                }
            }

            return -1;
        }

        private (long Steps, HashSet<Point> Visited) GetPath(Point start, Point end, Grid<bool> memory)
        {
            var queue = new PriorityQueue<(Point Point, HashSet<Point> Visited), long>();
            var cache = new Dictionary<Point, long>();

            queue.Enqueue((start, new HashSet<Point>()), 0);
            while (queue.TryDequeue(out (Point Point, HashSet<Point> Visited) position, out var steps))
            {
                if (position.Point == end)
                {
                    return (steps, position.Visited);
                }
                var known = cache.TryGetValue(position.Point, out var knownSteps);
                if (known && knownSteps <= steps)
                {
                    continue;
                }
                else
                {
                    cache[position.Point] = steps;
                }

                foreach (var adjacent in memory.Adjacent(position.Point, compare => !compare.Target))
                {
                    if (position.Visited.Contains(adjacent))
                    {
                        continue;
                    }
                    var visited = new HashSet<Point>(position.Visited)
                    {
                        adjacent
                    };
                    queue.Enqueue((adjacent, visited), steps + 1);
                }
            }

            return (-1, []);
        }

        private Grid<bool> GetMemorySpace(string[] input, int gridSize, int bytesToRead)
        {
            var max = Math.Min(input.Length, bytesToRead);

            var grid = new Grid<bool>(gridSize, gridSize);
            for (int i = 0; i < max; i++)
            {
                var parts = input[i].Split(',');
                grid[int.Parse(parts[0]), int.Parse(parts[1])] = true;
            }

            return grid;
        }
    }
}
