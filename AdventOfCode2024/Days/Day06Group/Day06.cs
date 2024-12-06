using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day06(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 6;

        protected override long ProcessPartOne(string[] input)
        {
            var map = GetMap(input, out var start);
            var points = RunPatrol(map, start, Directions.Up, out _);
            return points.Count;            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var map = GetMap(input, out var start);
            var points = RunPatrol(map, start, Directions.Up, out _);
            var options = points
                .Where(c => c.Key != start)
                .Select(c => (Point: c.Key, Direction: c.Value))
                .ToList();
            var validOptions = 0L;
            foreach(var option in options)
            {
                map[option.Point] = true;
                start = GetFrom(option.Point, option.Direction);
                var startdirection = TurnRight(option.Direction);
                RunPatrol(map, start, startdirection, out var loop);
                if (loop)
                {
                    validOptions++;
                }

                map[option.Point] = false;
            }

            return validOptions;
        }

        private Point GetFrom(Point position, Directions direction)
        {
            return direction switch
            {
                Directions.Up => new Point(position.X, position.Y + 1),
                Directions.Right => new Point(position.X - 1, position.Y),
                Directions.Down => new Point(position.X, position.Y - 1),
                Directions.Left => new Point(position.X + 1, position.Y),
                _ => throw new System.Exception("Invalid direction")
            };
        }

        private Grid<bool> GetMap(string[] input, out Point? start)
        {
            return input.ToGrid(c =>
            {
                return c switch
                {                    
                    '#' => true,
                    _ => false
                };
            }, c => c == '^', out start);
        }

        private Dictionary<Point, Directions> RunPatrol(Grid<bool> map, Point? start, Directions startDirection, out bool loop)
        {
            var known = new Dictionary<Point, Directions>
            {
                [start!.Value] = startDirection
            };            
            var position = start;
            var direction = startDirection;
            loop = false;
            while (position.HasValue)
            {
                Point? newPosition;
                if (TryMoveDirection(position.Value, direction, map, out newPosition))
                { 
                    if (known.TryGetValue(newPosition.Value, out var knownDirection))
                    {
                        if (knownDirection == direction)
                        {
                            loop = true;
                            return known;
                        }
                    }
                    else
                    {
                        known[newPosition.Value] = direction;
                    }
                }
                else
                {
                    direction = TurnRight(direction);
                }                
                position = newPosition;                
            }

            return known;
        }

        private bool TryMoveDirection(Point position, Directions direction, Grid<bool> map, [NotNullWhen(true)] out Point? newPosition)
        {
            if (!map.TryGetPointInDirection(position, direction, out newPosition))
            {
                return false;
            }
            else if (map[newPosition.Value])
            {
                newPosition = position;
                return false;
            }

            return true;
        }

        private Directions TurnRight(Directions direction)
        {
            return direction switch
            {
                Directions.Up => Directions.Right,
                Directions.Right => Directions.Down,
                Directions.Down => Directions.Left,
                Directions.Left => Directions.Up,
                _ => throw new System.Exception("Invalid direction")
            };
        }
    }
}
