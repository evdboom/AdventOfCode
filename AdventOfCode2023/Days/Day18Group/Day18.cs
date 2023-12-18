using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day18Group;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2023.Days
{
    public class Day18 : Day
    {
        public Day18(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 18;

        protected override long ProcessPartOne(string[] input)
        {
            var instructions = GetInstructions(input);            
            var points = GetPoints(instructions);
            return CalculateArea(points);

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var instructions = GetInstructions(input)
                .Select(TransformInstruction);
            var points = GetPoints(instructions);
            return CalculateArea(points);
        }

        private long CalculateArea(List<Point> points) 
        {
            var boundaryLength = 0M;
            var area = 0M;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var one = points[i];
                var two = points[i + 1];
                boundaryLength += Math.Abs(two.X - one.X) + Math.Abs(two.Y - one.Y);
                area += ((decimal)one.X * two.Y) - ((decimal)one.Y * two.X);             
            }

            area = Math.Abs(area / 2);
           
            return (long)(area + boundaryLength / 2 + 1);
        }

        private List<Point> GetPoints(IEnumerable<Instruction> instructions)
        {
            var point = new Point(0, 0);
            List<Point> points = [point];
            foreach (var instruction in instructions)
            {
                var newPoint = ToPoint(point, instruction);
                points.Add(newPoint);
                point = newPoint;
            }

            return points;
        }

        private Instruction TransformInstruction(Instruction instruction)
        {
            return new Instruction
            {
                Length = Convert.ToInt32(instruction.ColorCode[2..7], 16),
                Direction = instruction.ColorCode[7] switch
                {
                    '0' => 'R',
                    '1' => 'D',
                    '2' => 'L',
                    '3' => 'U',
                    _ => throw new NotSupportedException($"Invalid direction {instruction.ColorCode[7]}")
                }
            };
        }

        private IEnumerable<Instruction> GetInstructions(string[] input)
        {
            return input
                .Select(line =>
                {
                    var parts = line.Split(' ');
                    return new Instruction
                    {
                        Direction = parts[0][0],
                        Length = int.Parse(parts[1]),
                        ColorCode = parts[2]
                    };
                });
        }

        private Point ToPoint(Point point, Instruction instruction)
        {
            return instruction.Direction switch
            {
                'R' => new Point(point.X + instruction.Length, point.Y),
                'L' => new Point(point.X - instruction.Length, point.Y),
                'D' => new Point(point.X, point.Y + instruction.Length),
                'U' => new Point(point.X, point.Y - instruction.Length),
                _ => throw new NotSupportedException($"Unknown direction {instruction.Direction}")
            };
        }
    }
}