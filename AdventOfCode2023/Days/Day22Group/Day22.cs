using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day22Group;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023.Days
{
    public class Day22 : Day
    {
        public Day22(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 22;

        protected override long ProcessPartOne(string[] input)
        {
            var bricks = GetBricks(input);
            SettleBricks(bricks);            

            return bricks
                .Where(brick => !brick.Supports
                    .Where(support => support.SupportedBy.Count == 1)
                    .Any())
                .Count();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var bricks = GetBricks(input);
            SettleBricks(bricks);

            var count = 0;
            foreach(var brick in bricks.Where(b => b.Supports.Any()))
            {
                List<int> fallen = [brick.Id];
                Disintigrate(fallen, bricks);
                count += fallen.Count - 1;
            }
            return count;            
        }

        private void Disintigrate(List<int> fallen, List<Brick> bricks)
        {
            var newFallen = bricks
                .Where(other =>
                    !fallen.Contains(other.Id) &&
                    !other.WouldFallIfRemoved.Except(fallen).Any())
                .Select(other => other.Id)
                .ToList();

            if (newFallen.Any())
            {
                fallen.AddRange(newFallen);
                Disintigrate(fallen, bricks);
            }

        }

        private void SettleBricks(List<Brick> bricks)
        {
            foreach (var brick in bricks)
            {
                List<Brick>? supportBricks;
                while (CanFall(brick, bricks, out supportBricks))
                {
                    brick.Fall();
                }

                brick.SupportedBy = supportBricks;
                foreach (var support in brick.SupportedBy)
                {
                    support.Supports.Add(brick);
                }
            }
        }

        private List<Brick> GetBricks(string[] input)
        {
            var bricks = input
                .Select((line, index) =>
                {
                    var parts = line.Split('~');
                    return new Brick
                    {
                        Id = index + 1,
                        Start = parts[0]
                            .Split(',')
                            .Select(int.Parse)
                            .ToArray(),
                        End = parts[1]
                            .Split(',')
                            .Select(int.Parse)
                            .ToArray()
                    };
                })
                .OrderBy(brick => brick.Bottom)
                .ToList();

            return bricks;
        }

        private bool CanFall(Brick brick, List<Brick> bricks, out List<Brick> supportBricks)
        {
            if (brick.Bottom == 1)
            {
                supportBricks = [];
                return false;
            }

            supportBricks = bricks
                .Where(support =>
                    support.Id != brick.Id &&
                    support.Top == brick.Bottom - 1)
                .Where(support =>
                    brick.Left <= support.Right &&
                    brick.Right >= support.Left &&
                    brick.North <= support.South &&
                    brick.South >= support.North)
                .ToList();

            return !supportBricks.Any();
        }
    }
}