using System.Drawing;
using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day09(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 9;

    protected override long ProcessPartOne(string[] input)
    {
        var redTiles = ParseInput(input);
        var maxSurface = 0L;
        for (int i = 0; i < redTiles.Count - 1; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                var first = redTiles[i];
                var second = redTiles[j];
                var surface =
                    (Math.Abs(first.X - second.X) + 1L) * (Math.Abs(first.Y - second.Y) + 1L);
                maxSurface = Math.Max(maxSurface, surface);
            }
        }
        return maxSurface;
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var redTiles = ParseInput(input);
        var surfaces = new Dictionary<(Point First, Point Second), long>();
        for (int i = 0; i < redTiles.Count - 1; i++)
        {
            for (int j = i + 1; j < redTiles.Count; j++)
            {
                var first = redTiles[i];
                var second = redTiles[j];
                surfaces[(first, second)] =
                    (Math.Abs(first.X - second.X) + 1L) * (Math.Abs(first.Y - second.Y) + 1L);
            }
        }

        HashSet<Point> inside = [];
        HashSet<Point> outside = [];

        var maxSurface = surfaces
            .OrderByDescending(kv => kv.Value)
            .First(kv =>
                IsValidForSurface(kv.Key.First, kv.Key.Second, redTiles, ref inside, ref outside)
            )
            .Value;

        return maxSurface;
    }

    private static bool IsValidForSurface(
        Point first,
        Point second,
        List<Point> redTiles,
        ref HashSet<Point> inside,
        ref HashSet<Point> outside
    )
    {
        // Check all points in the rectangle formed by first and second
        var minX = Math.Min(first.X, second.X);
        var maxX = Math.Max(first.X, second.X);
        var minY = Math.Min(first.Y, second.Y);
        var maxY = Math.Max(first.Y, second.Y);

        var redTilesSet = redTiles.ToHashSet();

        for (int x = minX; x <= maxX; x++)
        {
            var upperPoint = new Point(x, minY);
            if (inside.Contains(upperPoint))
            {
                continue;
            }
            if (
                outside.Contains(upperPoint)
                || (
                    !redTilesSet.Contains(upperPoint)
                    && !IsPointOnBoundary(upperPoint, redTiles)
                    && !IsPointInsideShape(upperPoint, redTiles)
                )
            )
            {
                outside.Add(upperPoint);
                return false;
            }
            else
            {
                inside.Add(upperPoint);
            }
            var lowerPoint = new Point(x, maxY);
            if (inside.Contains(lowerPoint))
            {
                continue;
            }
            if (
                outside.Contains(lowerPoint)
                || (
                    !redTilesSet.Contains(lowerPoint)
                    && !IsPointOnBoundary(lowerPoint, redTiles)
                    && !IsPointInsideShape(lowerPoint, redTiles)
                )
            )
            {
                outside.Add(lowerPoint);
                return false;
            }
            else
            {
                inside.Add(lowerPoint);
            }
        }
        for (int y = minY; y <= maxY; y++)
        {
            var leftPoint = new Point(minX, y);
            if (inside.Contains(leftPoint))
            {
                continue;
            }
            if (
                outside.Contains(leftPoint)
                || (
                    !redTilesSet.Contains(leftPoint)
                    && !IsPointOnBoundary(leftPoint, redTiles)
                    && !IsPointInsideShape(leftPoint, redTiles)
                )
            )
            {
                outside.Add(leftPoint);
                return false;
            }
            else
            {
                inside.Add(leftPoint);
            }
            var rightPoint = new Point(maxX, y);
            if (inside.Contains(rightPoint))
            {
                continue;
            }
            if (
                outside.Contains(rightPoint)
                || (
                    !redTilesSet.Contains(rightPoint)
                    && !IsPointOnBoundary(rightPoint, redTiles)
                    && !IsPointInsideShape(rightPoint, redTiles)
                )
            )
            {
                outside.Add(rightPoint);
                return false;
            }
            else
            {
                inside.Add(rightPoint);
            }
        }
        return true;
    }

    private static bool IsPointOnBoundary(Point point, List<Point> redTiles)
    {
        for (int i = 0; i < redTiles.Count; i++)
        {
            var tile = redTiles[i];
            var nextTile = redTiles[(i + 1) % redTiles.Count];

            if (
                point.X == tile.X
                && point.X == nextTile.X
                && point.Y >= Math.Min(tile.Y, nextTile.Y)
                && point.Y <= Math.Max(tile.Y, nextTile.Y)
            )
            {
                return true;
            }
            else if (
                point.Y == tile.Y
                && point.Y == nextTile.Y
                && point.X >= Math.Min(tile.X, nextTile.X)
                && point.X <= Math.Max(tile.X, nextTile.X)
            )
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPointInsideShape(Point point, List<Point> redTiles)
    {
        // Ray casting algorithm: count how many times a ray from the point crosses the polygon boundary
        // If odd, point is inside; if even, point is outside
        var crossings = 0;

        for (int i = 0; i < redTiles.Count; i++)
        {
            var current = redTiles[i];
            var next = redTiles[(i + 1) % redTiles.Count];

            // Check if ray from point going right crosses the edge between current and next
            if ((current.Y > point.Y) != (next.Y > point.Y))
            {
                // Calculate x-coordinate of intersection
                var intersectionX =
                    current.X
                    + (double)(point.Y - current.Y) / (next.Y - current.Y) * (next.X - current.X);

                if (point.X < intersectionX)
                {
                    crossings++;
                }
            }
        }

        return crossings % 2 == 1;
    }

    private static List<Point> ParseInput(string[] input)
    {
        return
        [
            .. input.Select(line =>
            {
                var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
            }),
        ];
    }
}
