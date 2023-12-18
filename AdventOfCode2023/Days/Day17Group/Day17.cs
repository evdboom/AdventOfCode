using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day17Group;
using System.Drawing;

namespace AdventOfCode2023.Days
{
    public class Day17 : Day
    {
        public Day17(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 17;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid(c => int.Parse($"{c}"));
            return FindLowestHeatLoss(grid, 1, 3);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid(c => int.Parse($"{c}"));
            return FindLowestHeatLoss(grid, 4, 10);
        }

        private long FindLowestHeatLoss(int[,] grid, int minimumDirectionLength, int maximumDirectionLength)
        {
            var startPoint = new Point(0, 0);
            var endPoint = new Point(grid.GetLength(0) - 1, grid.GetLength(1) - 1);
            var startState = new State
            {
                Direction = 'X',
                DirectionLength = minimumDirectionLength,
                HeatLoss = 0,
                Distance = GetDistance(startPoint, endPoint)
            };

            var lowestHeatLoss = long.MaxValue;
            var storeR = new Dictionary<Point, (long Heatloss, int DirectionLength)>
            {
                [startPoint] = (startState.HeatLoss, startState.DirectionLength)
            };
            var storeL = new Dictionary<Point, (long Heatloss, int DirectionLength)>
            {
                [startPoint] = (startState.HeatLoss, startState.DirectionLength)
            };
            var storeU = new Dictionary<Point, (long Heatloss, int DirectionLength)>
            {
                [startPoint] = (startState.HeatLoss, startState.DirectionLength)
            };
            var storeD = new Dictionary<Point, (long Heatloss, int DirectionLength)>
            {
                [startPoint] = (startState.HeatLoss, startState.DirectionLength)
            };

            var queue = new PriorityQueue<Point, State>(new StateComparer());
            queue.Enqueue(startPoint, startState);
            while (queue.TryDequeue(out Point point, out State? state))
            {
                if (point == endPoint)
                {
                    if (state.HeatLoss < lowestHeatLoss && state.DirectionLength >= minimumDirectionLength)
                    {
                        lowestHeatLoss = state.HeatLoss;
                    }
                    continue;
                }

                if (state.HeatLoss + state.Distance >= lowestHeatLoss)
                {
                    continue;
                }

                foreach (var (newPoint, newState) in GetNewPoints(point, state, grid, endPoint, minimumDirectionLength, maximumDirectionLength))
                {
                    if (newState.HeatLoss + newState.Distance >= lowestHeatLoss)
                    {
                        continue;
                    }

                    switch (newState.Direction)
                    {
                        case 'R':
                            if (ShouldEnqueue(storeR, newPoint, newState, minimumDirectionLength))
                            {
                                queue.Enqueue(newPoint, newState);
                            }
                            break;
                        case 'L':
                            if (ShouldEnqueue(storeL, newPoint, newState, minimumDirectionLength))
                            {
                                queue.Enqueue(newPoint, newState);
                            }
                            break;
                        case 'U':
                            if (ShouldEnqueue(storeU, newPoint, newState, minimumDirectionLength))
                            {
                                queue.Enqueue(newPoint, newState);
                            }
                            break;
                        case 'D':
                            if (ShouldEnqueue(storeD, newPoint, newState, minimumDirectionLength))
                            {
                                queue.Enqueue(newPoint, newState);
                            }
                            break;
                    }
                }
            }
            return lowestHeatLoss;

        }

        private bool ShouldEnqueue(Dictionary<Point, (long HeatLoss, int DirectionLength)> store, Point point, State state, int minimumDirectionLength)
        {
            if (!store.TryGetValue(point, out var known))
            {
                store[point] = (state.HeatLoss, state.DirectionLength);
                return true;
            }
            else
            {


                if (state.HeatLoss < known.HeatLoss)
                {
                    store[point] = (Math.Min(known.HeatLoss, state.HeatLoss), Math.Min(known.DirectionLength, state.DirectionLength));
                    return true;
                }
                else if (state.DirectionLength < known.DirectionLength)
                {
                    store[point] = (Math.Min(known.HeatLoss, state.HeatLoss), Math.Min(known.DirectionLength, state.DirectionLength));
                    return true;
                }
                else if (known.DirectionLength < minimumDirectionLength && state.DirectionLength > known.DirectionLength)
                {
                    store[point] = (Math.Min(known.HeatLoss, state.HeatLoss), Math.Min(known.DirectionLength, state.DirectionLength));
                    return true;
                }                
            }

            return false;
        }

        private int GetDistance(Point startPoint, Point endPoint)
        {
            return Math.Abs(startPoint.X - endPoint.X) + Math.Abs(startPoint.Y - endPoint.Y);
        }

        private IEnumerable<(Point Point, State State)> GetNewPoints(Point point, State state, int[,] grid, Point endPoint, int minimumDirectionLength, int maximumDirectionLength)
        {
            if (point.X > 0 &&
                state.Direction != 'R' &&
                (
                    (state.Direction != 'L' && state.DirectionLength >= minimumDirectionLength) ||
                    (state.Direction == 'L' && state.DirectionLength < maximumDirectionLength)))
            {
                var newPoint = new Point(point.X - 1, point.Y);

                var newState = new State
                {
                    Direction = 'L',
                    DirectionLength = state.Direction == 'L'
                        ? state.DirectionLength + 1
                        : 1,
                    Distance = GetDistance(newPoint, endPoint),
                    HeatLoss = state.HeatLoss + grid[newPoint.X, newPoint.Y]
                };
                yield return (newPoint, newState);

            }
            if (point.Y > 0 &&
                state.Direction != 'D' &&
                (
                    (state.Direction != 'U' && state.DirectionLength >= minimumDirectionLength) ||
                    (state.Direction == 'U' && state.DirectionLength < maximumDirectionLength)))
            {
                var newPoint = new Point(point.X, point.Y - 1);

                var newState = new State
                {
                    Direction = 'U',
                    DirectionLength = state.Direction == 'U'
                    ? state.DirectionLength + 1
                    : 1,
                    Distance = GetDistance(newPoint, endPoint),
                    HeatLoss = state.HeatLoss + grid[newPoint.X, newPoint.Y],
                };
                yield return (newPoint, newState);

            }
            if (point.X < grid.GetLength(0) - 1 &&
                state.Direction != 'L' &&
                (
                    (state.Direction != 'R' && state.DirectionLength >= minimumDirectionLength) ||
                    (state.Direction == 'R' && state.DirectionLength < maximumDirectionLength)))
            {
                var newPoint = new Point(point.X + 1, point.Y);

                var newState = new State
                {
                    Direction = 'R',
                    DirectionLength = state.Direction == 'R'
                    ? state.DirectionLength + 1
                    : 1,
                    Distance = GetDistance(newPoint, endPoint),
                    HeatLoss = state.HeatLoss + grid[newPoint.X, newPoint.Y]
                };
                yield return (newPoint, newState);

            }
            if (point.Y < grid.GetLength(1) - 1 &&
                state.Direction != 'U' &&
                (
                    (state.Direction != 'D' && state.DirectionLength >= minimumDirectionLength) ||
                    (state.Direction == 'D' && state.DirectionLength < maximumDirectionLength)))
            {
                var newPoint = new Point(point.X, point.Y + 1);

                var newState = new State
                {
                    Direction = 'D',
                    DirectionLength = state.Direction == 'D'
                    ? state.DirectionLength + 1
                    : 1,
                    Distance = GetDistance(newPoint, endPoint),
                    HeatLoss = state.HeatLoss + grid[newPoint.X, newPoint.Y]
                };
                yield return (newPoint, newState);

            }
        }
    }
}