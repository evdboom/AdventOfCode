using System.Drawing;
using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day07(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 7;

    protected override long ProcessPartOne(string[] input)
    {
        var manifold = input.ToGrid(c => c == '^', c => c == 'S', out var start);

        if (!start.HasValue)
        {
            throw new InvalidOperationException("Start point 'S' not found in the input.");
        }

        var visited = new HashSet<Point>();
        var splitters = new HashSet<Point>();

        var queue = new Queue<Point>([start.Value]);

        while (queue.TryDequeue(out var current))
        {
            var adjacent = manifold.AdjacentWithDirection(current, true);
            if (adjacent.FirstOrDefault(a => a.Direction == Directions.Down) is var next)
            {
                if (!next.Value && visited.Add(next.Point))
                {
                    queue.Enqueue(next.Point);
                }
                else if (next.Value)
                {
                    splitters.Add(current);
                    foreach (
                        var adj in adjacent.Where(a => (Directions.CaretDown & a.Direction) > 0)
                    )
                    {
                        if (visited.Add(adj.Point))
                        {
                            queue.Enqueue(adj.Point);
                        }
                    }
                }
            }
        }

        return splitters.Count;
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var manifold = input.ToGrid(c => c == '^', c => c == 'S', out var start);

        if (!start.HasValue)
        {
            throw new InvalidOperationException("Start point 'S' not found in the input.");
        }

        var calculatedTimeLines = new Dictionary<Point, long>();

        return CalculateTimeLines(manifold, start.Value, ref calculatedTimeLines);
    }

    private static long CalculateTimeLines(
        Grid<bool> manifold,
        Point current,
        ref Dictionary<Point, long> calculatedTimeLines
    )
    {
        if (calculatedTimeLines.ContainsKey(current))
        {
            return calculatedTimeLines[current];
        }

        var adjacent = manifold.AdjacentWithDirection(current, true);
        if (
            adjacent.FirstOrDefault(a => a.Direction == Directions.Down) is var next
            && next.Direction != Directions.Unknown
        )
        {
            if (!next.Value)
            {
                var timelines = CalculateTimeLines(manifold, next.Point, ref calculatedTimeLines);
                calculatedTimeLines[current] = timelines;
                return timelines;
            }
            else
            {
                var splitValues = adjacent.Where(a => (Directions.CaretDown & a.Direction) > 0);

                var result = 0L;
                foreach (var value in splitValues)
                {
                    result += CalculateTimeLines(manifold, value.Point, ref calculatedTimeLines);
                }
                calculatedTimeLines[current] = result;
                return result;
            }
        }
        else
        {
            calculatedTimeLines[current] = 1;
            return 1;
        }
    }
}
