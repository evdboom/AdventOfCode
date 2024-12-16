using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day15(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 15;

        protected override long ProcessPartOne(string[] input)
        {
            var map = GetMap(input, false, out var instructions);
            var position = map.First(x => x.Value == '@').Key;
            foreach (var instruction in instructions)
            {
                position = MoveIfPossible(map, position, instruction, '@');
            }

            return map
                .Where(map => map.Value == 'O')
                .Sum(map => map.Key.Y * 100 + map.Key.X);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var map = GetMap(input, true, out var instructions);
            var position = map.First(x => x.Value == '@').Key;

            foreach (var instruction in instructions)
            {
                position = MoveIfPossible(map, position, instruction, '@');
            }

            return map
                .Where(map => map.Value == '[')
                .Sum(map => map.Key.Y * 100 + map.Key.X);
        }

        private Point MoveIfPossible(Dictionary<Point, char> map, Point position, char instruction, char movedObject)
        {
            if (!CanMove(map, position, instruction, out var newPosition) || map[position] != movedObject)
            {
                return position;
            }

            var value = map[newPosition];
            switch (value)
            {
                case 'O':
                    MoveIfPossible(map, newPosition, instruction, value);
                    break;
                case '[':
                    MoveIfPossible(map, newPosition, instruction, value);
                    MoveIfPossible(map, newPosition with { X = newPosition.X + 1 }, instruction, ']');
                    break;
                case ']':
                    MoveIfPossible(map, newPosition, instruction, value);
                    MoveIfPossible(map, newPosition with { X = newPosition.X - 1 }, instruction, '[');
                    break;
            }
            map[newPosition] = movedObject;
            map[position] = '.';

            return newPosition;
        }

        private bool CanMove(Dictionary<Point, char> map, Point position, char instruction, out Point newPosition)
        {
            newPosition = instruction switch
            {
                '^' => position with { Y = position.Y - 1 },
                'v' => position with { Y = position.Y + 1 },
                '<' => position with { X = position.X - 1 },
                '>' => position with { X = position.X + 1 },
                _ => throw new Exception($"Unknown instruction {instruction}")
            };

            var newPositionValue = map[newPosition];
            return instruction switch
            {
                '<' or '>' => newPositionValue switch
                {
                    '.' => true,
                    'O' or '[' or ']' => CanMove(map, newPosition, instruction, out _),
                    _ => false
                },
                _ => newPositionValue switch
                {
                    '.' => true,
                    'O' => CanMove(map, newPosition, instruction, out _),
                    '[' => CanMove(map, newPosition, instruction, out _) && CanMove(map, newPosition with { X = newPosition.X + 1 }, instruction, out _),
                    ']' => CanMove(map, newPosition, instruction, out _) && CanMove(map, newPosition with { X = newPosition.X - 1 }, instruction, out _),
                    _ => false
                }
            };
        }

        private Dictionary<Point, char> GetMap(string[] input, bool enlarged, out string instructions)
        {
            instructions = string.Empty;
            var instructionsStarted = false;
            var map = new Dictionary<Point, char>();

            foreach (var (line, row) in input.Select((l, i) => (l, i)))
            {
                if (instructionsStarted)
                {
                    instructions += line;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    instructionsStarted = true;
                    continue;
                }
                else
                {
                    foreach (var (point, column) in line.Select((c, i) => (c, i)))
                    {
                        if (!enlarged)
                        {
                            map[new Point(column, row)] = point;
                        }
                        else
                        {
                            switch (point)
                            {
                                case '#':
                                case '.':
                                    map[new Point(column * 2, row)] = point;
                                    map[new Point(column * 2 + 1, row)] = point;
                                    break;
                                case '@':
                                    map[new Point(column * 2, row)] = point;
                                    map[new Point(column * 2 + 1, row)] = '.';
                                    break;
                                case 'O':
                                    map[new Point(column * 2, row)] = '[';
                                    map[new Point(column * 2 + 1, row)] = ']';
                                    break;
                                default:
                                    throw new Exception($"Unknown character {point}");

                            }
                        }
                    }
                }
            }

            return map;
        }
    }
}
