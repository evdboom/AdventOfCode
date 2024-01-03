using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day24Group;

namespace AdventOfCode2023.Days
{
    public class Day24 : Day
    {
        public long TestAreaStart = 200000000000000;
        public long TestAreaEnd = 400000000000000;

        public Day24(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 24;

        protected override long ProcessPartOne(string[] input)
        {
            var hailStones = input
                .Select((line, index) =>
                {
                    var parts = line.Split(" @ ");
                    return new HailStone
                    {
                        Id = index,
                        Position = parts[0]
                            .Split(",")
                            .Select(part => long.Parse(part.Trim()))
                            .ToArray(),
                        Velocity = parts[1]
                            .Split(",")
                            .Select(part => long.Parse(part.Trim()))
                            .ToArray(),
                    };
                })
                .ToList();

            var valid = 0;
            foreach (var hail in hailStones)
            {
                foreach(var other in hailStones.Where(other => other.Id > hail.Id)) 
                {
                    var intersection = GetIntersection(hail, other);

                    if (intersection.X.HasValue &&
                        intersection.Y.HasValue &&
                        intersection.X >= TestAreaStart &&
                        intersection.X <= TestAreaEnd &&
                        intersection.Y >= TestAreaStart &&
                        intersection.Y <= TestAreaEnd)
                    {
                        valid++;
                    }
                }
            }

            return valid;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var hailStones = input
                            .Select((line, index) =>
                            {
                                var parts = line.Split(" @ ");
                                return new HailStone
                                {
                                    Id = index,
                                    Position = parts[0]
                                        .Split(",")
                                        .Select(part => long.Parse(part.Trim()))
                                        .ToArray(),
                                    Velocity = parts[1]
                                        .Split(",")
                                        .Select(part => long.Parse(part.Trim()))
                                        .ToArray(),
                                };
                            })
                            .ToList();

            var matrix = new double[7, 6];
            FillRow(matrix, 0, hailStones[0], hailStones[1], 0, 1); // X, Y
            FillRow(matrix, 1, hailStones[0], hailStones[1], 0, 2); // X, Z
            FillRow(matrix, 2, hailStones[0], hailStones[1], 1, 2); // Y, Z
            FillRow(matrix, 3, hailStones[0], hailStones[2], 0, 1);
            FillRow(matrix, 4, hailStones[0], hailStones[2], 0, 2);
            FillRow(matrix, 5, hailStones[0], hailStones[2], 1, 2);

            var coefficients = matrix.GaussianElimination();

            return coefficients
                .Take(3)
                .Sum(v => (long)Math.Round(v));
        }

        private void FillRow(double[,] matrix, int row, HailStone one, HailStone two, int param1, int param2)
        {
            matrix[param1, row] = two.Velocity[param2] - one.Velocity[param2];
            matrix[param2, row] = one.Velocity[param1] - two.Velocity[param1];
            matrix[param1 + 3, row] = one.Position[param2] - two.Position[param2];
            matrix[param2 + 3, row] = two.Position[param1] - one.Position[param1];
            matrix[6, row] = two.Position[param1] * two.Velocity[param2] - two.Position[param2] * two.Velocity[param1] - one.Position[param1] * one.Velocity[param2] + one.Position[param2] * one.Velocity[param1];
        }

        private (double? X, double? Y) GetIntersection(HailStone hail, HailStone other)
        {            
            if (Math.Abs(hail.DY - other.DY) == 0)
            {
                return (null, null);
            }

            var x = (other.Y0 - hail.Y0) / (hail.DY - other.DY);
            var y = hail.DY * x + hail.Y0;

            if (IsInPast(hail, x, y) || IsInPast(other, x, y))
            {
                return (null, null);
            }

            return (x, y);
        }

        private bool IsInPast(HailStone hail, double x, double y) 
        {
            return
                (x > hail.Position[0] && hail.Velocity[0] < 0) ||
                (x < hail.Position[0] && hail.Velocity[0] > 0) ||
                (y > hail.Position[1] && hail.Velocity[1] < 0) ||
                (y < hail.Position[1] && hail.Velocity[1] > 0);

        }
    }
}
