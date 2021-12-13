using AdventOfCode2021.Services;
using System.Drawing;

namespace AdventOfCode2021.Days
{
    public class Day13 : Day
    {
        private const char PointSplit = ',';
        private const char InstructionSplit = '=';
        private const string InstructionPrefix = "fold along ";
        private const string Horizontal = "x";
        private const string Vertical = "y";

        private readonly IScreenWriter _writer;

        public Day13(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 13;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = GetGrid(input);
            var instructions = GetInstructions(input);

            var result = ProcessInstruction(grid, instructions[0]);

            var sum = 0;
            for (int j = 0; j < result.GetLength(1); j++)
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    if (result[i, j])
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = GetGrid(input);
            var instructions = GetInstructions(input);

            var result = grid;
            foreach (var instruction in instructions)
            {
                result = ProcessInstruction(result, instruction);
            }

            PrintGrid(result);

            var sum = 0;
            for (int j = 0; j < result.GetLength(1); j++)
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    if (result[i, j])
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        private void PrintGrid(bool[,] result)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    if (result[i, j])
                    {
                        _writer.WriteBlock(ConsoleColor.White);
                    }
                    else
                    {
                        _writer.WriteBlock();
                    }
                }
                _writer.NewLine();
            }
        }

        private bool[,] ProcessInstruction(bool[,] grid, (string Direction, int Location) p)
        {
            if (p.Direction == Horizontal)
            {
                return ProcessHorizontal(grid, p.Location);
            }
            else if (p.Direction == Vertical)
            {
                return ProcessVertical(grid, p.Location);
            }
            else
            {
                throw new ArgumentException($"Unknown direction {p.Direction}");
            }
        }

        private bool[,] ProcessVertical(bool[,] grid, int location)
        {
            var result = new bool[grid.GetLength(0), location + 1];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (j < result.GetLength(1))
                    {
                        result[i, j] = result[i, j] || grid[i, j];
                    }
                    else
                    {
                        var dist = j - location;
                        var newJ = location - dist;

                        if (newJ >= 0)
                        {
                            result[i, newJ] = result[i, newJ] || grid[i, j];
                        }
                    }
                }
            }

            return result;

        }

        private bool[,] ProcessHorizontal(bool[,] grid, int location)
        {
            var result = new bool[location + 1, grid.GetLength(1)];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (i < result.GetLength(0))
                    {
                        result[i, j] = result[i, j] || grid[i, j];
                    }
                    else
                    {
                        var dist = i - location;
                        var newI = location - dist;

                        if (newI >= 0)
                        {
                            result[newI, j] = result[newI, j] || grid[i, j];
                        }
                    }
                }
            }

            return result;
        }

        private bool[,] GetGrid(string[] input)
        {
            var coordinates = input
                .Where(i => i.Contains(PointSplit))
                .Select(i => GetCoordinate(i))
                .ToList();

            var width = coordinates.Max(p => p.X) + 1;
            var height = coordinates.Max(p => p.Y) + 1;

            var result = new bool[width, height];

            foreach (var point in coordinates)
            {
                result[point.X, point.Y] = true;
            }

            return result;
        }

        private List<(string Direction, int Location)> GetInstructions(string[] input)
        {
            return input
                .Where(i => i.StartsWith(InstructionPrefix))
                .Select(i => GetInstruction(i))
                .ToList();
        }

        private (string Direction, int Location) GetInstruction(string instruction)
        {
            var parts = instruction
                .Substring(InstructionPrefix.Length)
                .Split(InstructionSplit);

            return (parts[0], int.Parse(parts[1]));
        }

        private Point GetCoordinate(string coordinate)
        {
            var values = coordinate
                .Split(PointSplit)
                .Select(c => int.Parse(c))
                .ToArray();

            return new Point(values[0], values[1]);
        }
    }
}
