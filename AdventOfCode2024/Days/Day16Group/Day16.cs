using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day16(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 16;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid();
            var start = grid.First(x => x.Value == 'S').Point;
            var end = grid.First(x => x.Value == 'E').Point;

            return GetBestPaths(grid, start, end, false, out _);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid();
            var start = grid.First(x => x.Value == 'S').Point;
            var end = grid.First(x => x.Value == 'E').Point;

            GetBestPaths(grid, start, end, true, out var visited);

            return visited.Count;
        }

        private long GetBestPaths(Grid<char> grid, Point start, Point end, bool getAll, out HashSet<Point> visited)
        {
            visited = [start];
            var position = (Point: start, Direction: Directions.Right, VisistedOnRoute: new HashSet<Point>(visited));
            var queue = new PriorityQueue<(Point Point, Directions Direction, HashSet<Point> VisistedOnRoute), long>();
            var cache = new Dictionary<(Point Point, Directions Direction), long>();
            queue.Enqueue(position, 0);
            var best = long.MaxValue;
            while (queue.TryDequeue(out position, out var cost))
            {
                if (cache.TryGetValue((position.Point, position.Direction), out var existingCost) && existingCost < cost)
                {
                    continue;
                }
                else
                {
                    cache[(position.Point, position.Direction)] = cost;
                }

                if (position.Point == end && cost <= best)
                {
                    best = cost;
                    foreach (var point in position.VisistedOnRoute)
                    {
                        visited.Add(point);
                    }
                    if (!getAll)
                    {
                        break;
                    }
                }
                foreach (var (adjacent, direction, _) in grid.AdjacentWithDirection(position.Point, compare => compare.Target != '#').Where(x => MaxNinetyDegrees(position.Direction, x.Direction)))
                {
                    if (direction != position.Direction)
                    {
                        queue.Enqueue((position.Point, direction, position.VisistedOnRoute), cost + 1000);
                    }
                    else
                    {
                        var newVisited = new HashSet<Point>(position.VisistedOnRoute);
                        if (newVisited.Add(adjacent))
                        {
                            queue.Enqueue((adjacent, direction, newVisited), cost + 1);
                        }
                    }
                }
            }

            return best;
        }

        private bool MaxNinetyDegrees(Directions direction, Directions newDirection)
        {
            return direction switch
            {
                Directions.Up => newDirection == Directions.Up || newDirection == Directions.Left || newDirection == Directions.Right,
                Directions.Down => newDirection == Directions.Down || newDirection == Directions.Left || newDirection == Directions.Right,
                Directions.Left => newDirection == Directions.Left || newDirection == Directions.Up || newDirection == Directions.Down,
                Directions.Right => newDirection == Directions.Right || newDirection == Directions.Up || newDirection == Directions.Down,
                _ => false
            };
        }
    }
}
