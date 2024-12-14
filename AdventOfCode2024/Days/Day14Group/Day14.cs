using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2024.Days.Day14Group;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day14(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 14;
        public int GridWidth { get; set; } = 101;
        public int GridHeight { get; set; } = 103;

        protected override long ProcessPartOne(string[] input)
        {
            var robots = GetRobots(input);
            foreach (var robot in robots)
            {
                robot.Move(100, GridWidth, GridHeight);
            }

            var result = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var x_start = i * GridWidth / 2 + i;
                    var y_start = j * GridHeight / 2 + j;
                    var x_end = (i + 1) * GridWidth / 2;
                    var y_end = (j + 1) * GridHeight / 2;

                    var count = robots.Count(r => r.X >= x_start && r.X < x_end && r.Y >= y_start && r.Y < y_end);
                    result *= count;
                }
            }

            return result;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            // No solution for provided testdata, testdata sets the with to 11
            if (GridWidth != 101)
            {
                return -1;
            }

            var robots = GetRobots(input);

            var total = robots.Count;

            var found = false;
            var steps = 0;

            while (!found)
            {
                foreach (var robot in robots)
                {
                    robot.Move(1, GridWidth, GridHeight);
                }
                steps++;

                HashSet<int> counts = [];
                HashSet<Point> positions = [];
                foreach (var robot in robots)
                {
                    var count = 0;
                    if (!positions.Add(robot.Location))
                    {
                        continue;
                    }

                    var queue = new Queue<Point>();
                    queue.Enqueue(robot.Location);

                    while (queue.TryDequeue(out var location))
                    {
                        foreach (var adjacent in location.Adjacent())
                        {
                            if (!positions.Add(adjacent))
                            {
                                continue;
                            }

                            var add = robots.Count(r => r.Location == adjacent);
                            if (add > 0)
                            {
                                count += add;
                                queue.Enqueue(adjacent);
                            }                            
                        }
                    }

                    counts.Add(count);
                }

                // picture = image + frame, so 2 groups
                // 'most' of the robots are in the picture (assumption: more than half)
                found = counts
                    .OrderDescending()
                    .Take(2)
                    .Sum() >= total / 2;
            }

            return steps;

        }

        private void PrintGrid(List<Robot> robots)
        {
            for (int i = 0; i < GridHeight; i++)
            {
                for (int j = 0; j < GridWidth; j++)
                {
                    if (robots.Any(r => r.X == j && r.Y == i))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }                    
                }
                Console.WriteLine();
            }
        }

        private List<Robot> GetRobots(string[] input)
        {
            return input
                .Select(line =>
            {
                var parts = line.Split(' ');
                var position = parts[0].Split(',');
                var x = int.Parse(position[0][2..]);
                var y = int.Parse(position[1]);
                var velocity = parts[1].Split(',');
                var dx = int.Parse(velocity[0][2..]);
                var dy = int.Parse(velocity[1]);
                return new Robot(x, y, dx, dy);
            }).ToList();
        }
    }
}
