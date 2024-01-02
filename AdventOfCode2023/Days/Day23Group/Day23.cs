using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day23Group;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2023.Days
{
    public class Day23 : Day
    {
        public Day23(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 23;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid(c => c);
            var start = new Intersection
            {
                Location = new Point(1, 0)
            };
            var end = new Intersection
            {
                Location = new Point(grid.GetLength(0) - 2, grid.GetLength(1) - 1)
            };
            var intersections = new Dictionary<Point, Intersection>
            {
                [start.Location] = start,
                [end.Location] = end
            };
            MapIntersection(start, start.Location, intersections, grid, slippySlopes: true);

            return GetMaxSteps(start, end);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c => c);
            var start = new Intersection
            {
                Location = new Point(1, 0)
            };
            var end = new Intersection
            {
                Location = new Point(grid.GetLength(0) - 2, grid.GetLength(1) - 1)
            };
            var intersections = new Dictionary<Point, Intersection>
            {
                [start.Location] = start,
                [end.Location] = end
            };
            MapIntersection(start, start.Location, intersections, grid, slippySlopes: false);

            return GetMaxSteps(start, end);
        }


        private void MapIntersection(Intersection start, Point mapFrom, Dictionary<Point, Intersection> intersections, char[,] grid, bool slippySlopes)
        {
            var directions = grid
                .Adjacent(start.Location)
                .Where(direction =>
                    direction != mapFrom &&
                    grid[direction.X, direction.Y] != '#');

            if (slippySlopes)
            {
                directions = directions
                    .Where(direction =>
                        (direction.X != start.Location.X - 1 || grid[direction.X, direction.Y] != '>') &&
                        (direction.X != start.Location.X + 1 || grid[direction.X, direction.Y] != '<') &&
                        (direction.Y != start.Location.Y - 1 || grid[direction.X, direction.Y] != 'v') &&
                        (direction.Y != start.Location.Y + 1 || grid[direction.X, direction.Y] != '^'));
            }

            foreach (var direction in directions)
            {
                var from = start.Location;
                var fromFrom = start.Location;
                var steps = 0;

                List<Point> next = [direction];
                do
                {
                    var newFrom = next[0];
                    next = grid
                        .Adjacent(next[0])
                        .Where(next =>
                            grid[next.X, next.Y] != '#' &&
                            next != from)
                        .ToList();
                    steps++;
                    fromFrom = from;
                    from = newFrom;


                }
                while (next.Count == 1);



                Intersection intersection;
                if (intersections.TryGetValue(from, out var known))
                {
                    intersection = known;
                }
                else
                {
                    intersection = new Intersection
                    {
                        Location = from
                    };
                    intersections[from] = intersection;
                    MapIntersection(intersection, fromFrom, intersections, grid, slippySlopes);
                }

                AddConnection(start, intersection, direction, steps);

                if (!slippySlopes)
                {
                    AddConnection(intersection, start, fromFrom, steps);
                }

            }
        }

        private void AddConnection(Intersection from, Intersection to, Point direction, int steps)
        {
            if (direction.X > from.Location.X)
            {
                from.East = to;
                from.EastDistance = steps;
            }
            else if (direction.X < from.Location.X)
            {
                from.West = to;
                from.WestDistance = steps;
            }
            else if (direction.Y > from.Location.Y)
            {
                from.South = to;
                from.SouthDistance = steps;
            }
            else if (direction.Y < from.Location.Y)
            {
                from.North = to;
                from.NorthDistance = steps;
            }
        }

        private long GetMaxSteps(Intersection start, Intersection end)
        {
            var initialState = new State
            {
                Distance = 0,
                Location = start,
                Visited = [start]
            };

            var queue = new PriorityQueue<State, int>(new PriorityInverter());
            queue.Enqueue(initialState, initialState.Distance);

            var maxSteps = 0;
            while (queue.TryDequeue(out var state, out int priority))
            {
                if (state.Location == end)
                {
                    if (priority > maxSteps)
                    {
                        maxSteps = priority;
                    }

                    continue;
                }

                foreach(var connection in state.Location.Connections())
                {
                    if (!state.Visited.Contains(connection.Intersection))
                    {
                        var newState = new State
                        {
                            Distance = state.Distance + connection.Distance,
                            Location = connection.Intersection,
                            Visited = state.Visited.ToList()
                        };
                        newState.Visited.Add(connection.Intersection);
                        queue.Enqueue(newState, newState.Distance);
                    }
                }
            }

            return maxSteps;
        }
    }
}
