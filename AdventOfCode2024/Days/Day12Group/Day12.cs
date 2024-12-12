using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day12(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 12;

        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid();
            return GetRegions(grid)
                .Aggregate(0L, (acc, region) => acc + (region.Count * GetPerimeter(region, grid)));                
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid();
            return GetRegions(grid)
                .Aggregate(0L, (acc, region) => acc + (region.Count * GetSides(region, grid)));
        }

        private long GetSides(List<Point> region, Grid<char> grid)
        {
            return region
                .SelectMany(point => GetDirectionOptions().Select(directions =>
                {
                    var primaryExists = grid.TryGetPointInDirection(point, directions.Primary, compare => compare.Origin == compare.Target, out _);
                    var secondaryExists = grid.TryGetPointInDirection(point, directions.Secondary, compare => compare.Origin == compare.Target, out _);
                    var diagonalExists = grid.TryGetPointInDirection(point, directions.Diagonal, compare => compare.Origin == compare.Target, out _);

                    return (!primaryExists && !secondaryExists) || (primaryExists && secondaryExists && !diagonalExists);
                }))
                .Where(x => x)
                .Count();         
        }

        private IEnumerable<(Directions Primary, Directions Secondary, Directions Diagonal)> GetDirectionOptions()
        {
            yield return (Directions.Up, Directions.Right, Directions.UpRight);
            yield return (Directions.Right, Directions.Down, Directions.DownRight);
            yield return (Directions.Down, Directions.Left, Directions.DownLeft);
            yield return (Directions.Left, Directions.Up, Directions.UpLeft);
        }

        private long GetPerimeter(List<Point> region, Grid<char> grid)
        {
            return region
                .Select(point => 4 - grid.Adjacent(point, compare => compare.Origin == compare.Target).Count())
                .Sum();
        }

        private List<List<Point>> GetRegions(Grid<char> grid)
        {
            var regions = new List<List<Point>>();
            var visited = new HashSet<Point>();
            foreach (var cell in grid)
            {
                if (!visited.Add(cell.Point))
                {
                    continue;
                }
                var region = new List<Point>
                {
                    cell.Point
                };
                var queue = new Queue<Point>();
                queue.Enqueue(cell.Point);
                while (queue.TryDequeue(out var current))
                {
                    foreach (var adjacent in grid.Adjacent(current, compare => compare.Origin == compare.Target))
                    {
                        if (!visited.Add(adjacent))
                        {
                            continue;
                        }
                        queue.Enqueue(adjacent);
                        region.Add(adjacent);
                    }
                }
                regions.Add(region);
            }
            return regions;
        }
    }
}
