using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day11 : Day
    {
        public Day11(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 11;

        protected override long ProcessPartOne(string[] input)
        {
            var galaxies = GetExpandedGalaxies(input.ToGrid(item => item == '#'), 2);

            return galaxies
                .Select((galaxy, index) => (galaxy.X, galaxy.Y, Index: index))
                .Aggregate(0L, (totalDistance, galaxy) =>
                {
                    var distances = galaxies
                        .Where((other, index) => index > galaxy.Index)
                        .Select(other => Math.Abs(other.X - galaxy.X) + Math.Abs(other.Y - galaxy.Y))
                        .Sum();
                    return totalDistance + distances;
                });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var galaxies = GetExpandedGalaxies(input.ToGrid(item => item == '#'), 1000000);

            return galaxies
                .Select((galaxy, index) => (galaxy.X, galaxy.Y, Index: index))
                .Aggregate(0L, (totalDistance, galaxy) =>
                {
                    var distances = galaxies
                        .Where((other, index) => index > galaxy.Index)
                        .Select(other => Math.Abs(other.X - galaxy.X) + Math.Abs(other.Y - galaxy.Y))
                        .Sum();
                    return totalDistance + distances;
                });
        }

        private List<(long X, long Y)> GetExpandedGalaxies(Grid<bool> grid, long expandFactor)
        {
            var galaxyFreeRows = new List<int>();
            for (var j = 0; j < grid.Height; j++) 
            {
                var hasGalaxy = false;
                for (var i = 0; i < grid.Width; i++)
                {
                    if (grid[i, j])
                    {
                        hasGalaxy = true;
                        break;
                    }
                }
                if (!hasGalaxy)
                {
                    galaxyFreeRows.Add(j);
                }
            }
            var galaxyFreeColumns = new List<int>();
            for (var i = 0; i < grid.Width; i++)
            {
                var hasGalaxy = false;
                for (var j = 0; j < grid.Height; j++)
                {
                    if (grid[i, j])
                    {
                        hasGalaxy = true;
                        break;
                    }
                }
                if (!hasGalaxy)
                {
                    galaxyFreeColumns.Add(i);
                }
            }

            var galaxies = new List<(long X, long Y)>();
            for (var j = 0; j < grid.Height; j++)
            {
                for (var i = 0; i < grid.Width; i++)
                {
                    if (grid[i, j])
                    {
                        var k = i + (expandFactor - 1) * galaxyFreeColumns.Where(column => column < i).Count();
                        var l = j + (expandFactor - 1) * galaxyFreeRows.Where(row => row < j).Count();
                        galaxies.Add((k, l));
                    }
                }
            }

            return galaxies;
        }
    }
}
