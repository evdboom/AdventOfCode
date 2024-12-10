using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day10(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 10;

        protected override long ProcessPartOne(string[] input)
        {
            var map = input.ToIntGrid();
            return map
                .Where(c => c.Value == 0)
                .Aggregate(0L, (acc, cell) =>
                {
                    return acc + GetScores(cell.Point, map);
                });
                
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var map = input.ToIntGrid();
            return map
                .Where(c => c.Value == 0)
                .Aggregate(0L, (acc, cell) =>
                {
                    return acc + GetScores(cell.Point, map, unique: false);
                });
        }

        private long GetScores(Point point, Grid<int> map, bool unique = true)
        {
            ICollection<Point> points = unique
                ? new HashSet<Point>()
                : new List<Point>();
            foreach (var next in GetNextPoints(point, map, 0))
            {
                points.Add(next);
            }
            return points.Count;
        }

        private IEnumerable<Point> GetNextPoints(Point source, Grid<int> map, int value)
        {
            if (value == 9)
            {
                yield return source;
                yield break;
            }

            foreach (var point in map
                .Adjacent(source, compare => compare.Target == compare.Origin + 1)
                .SelectMany(point => GetNextPoints(point, map, value + 1)))
            {
                yield return point;
            }
        }
    }
}
