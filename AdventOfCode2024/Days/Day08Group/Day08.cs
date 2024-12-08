using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day08(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 8;

        protected override long ProcessPartOne(string[] input)
        {
            var map = input.ToGrid();
            var antennaTypes = map
                .GroupBy(c => c.Value)
                .Where(c => c.Key != '.')
                .ToList();

            return antennaTypes.Aggregate(new HashSet<Point>(), (acc, type) =>
            {
                var antennas = type
                    .Select(c => c.Point)
                    .ToList();
                foreach (var antinode in GetNodes(antennas, map.MaxX, map.MaxY))
                {
                    acc.Add(antinode);
                }
                return acc;
            }).Count;

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var map = input.ToGrid();
            var antennaTypes = map
                .GroupBy(c => c.Value)
                .Where(c => c.Key != '.')
                .ToList();

            return antennaTypes.Aggregate(new HashSet<Point>(), (acc, type) =>
            {
                var antennas = type
                    .Select(c => c.Point)
                    .ToList();
                foreach (var antinode in GetNodes(antennas, map.MaxX, map.MaxY, true))
                {
                    acc.Add(antinode);
                }
                return acc;
            }).Count;
        }

        private bool PointOnMap(Point point, int mapMaxX, int mapMaxY)
        {
            return point.X >= 0 && point.X <= mapMaxX && point.Y >= 0 && point.Y <= mapMaxY;
        }

        private IEnumerable<Point> GetNodes(List<Point> antennnas, int mapMaxX, int mapMaxY, bool all = false)
        {
            for (int i = 0; i < antennnas.Count - 1; i++)
            {
                var current = antennnas[i];
                if (all)
                {
                    yield return current;
                }

                for (int j = i + 1; j < antennnas.Count; j++)
                {
                    var next = antennnas[j];
                    var distanceX = current.X - next.X;
                    var distanceY = current.Y - next.Y;

                    var pointOne = new Point(current.X, current.Y);
                    var pointTwo = new Point(next.X, next.Y);
                    do
                    {
                        pointOne.X += distanceX;
                        pointOne.Y += distanceY;
                        pointTwo.X -= distanceX;
                        pointTwo.Y -= distanceY;

                        if (PointOnMap(pointOne, mapMaxX, mapMaxY))
                        {
                            yield return pointOne;
                        }
                        if (PointOnMap(pointTwo, mapMaxX, mapMaxY))
                        {
                            yield return pointTwo;
                        }
                    }
                    while (all && (PointOnMap(pointOne, mapMaxX, mapMaxY) || PointOnMap(pointTwo, mapMaxX, mapMaxY)));
                }
            }

            if (all)
            {
                yield return antennnas[antennnas.Count - 1];
            }
        }
    }
}
