using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day05(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 5;

    protected override long ProcessPartOne(string[] input)
    {
        var (ranges, ingredients) = ParseRanges(input);
        return ingredients.Count(i => ranges.Any(r => IsInRange(i, r.Start, r.End)));
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var (ranges, _) = ParseRanges(input);
        var queue = new PriorityQueue<(long Start, long End), long>(
            ranges.Select(range => (range, range.Start))
        );

        var first = queue.Dequeue();
        var mergedRanges = new List<(long Start, long End)>([first]);

        while (queue.TryDequeue(out var currentRange, out _))
        {
            var (lastStart, lastEnd) = mergedRanges[^1];
            if (IsInRange(currentRange.Start, lastStart, lastEnd))
            {
                mergedRanges[^1] = (lastStart, Math.Max(lastEnd, currentRange.End));
            }
            else
            {
                mergedRanges.Add(currentRange);
            }
        }

        return mergedRanges.Aggregate(0L, (total, range) => total + range.End - range.Start + 1);
    }

    private static (List<(long Start, long End)> Ranges, List<long> Ingredients) ParseRanges(
        string[] input
    )
    {
        List<(long Start, long End)> ranges = [];
        List<long> ingredients = [];
        bool ingredientSection = false;
        foreach (string range in input)
        {
            if (string.IsNullOrWhiteSpace(range))
            {
                ingredientSection = true;
                continue;
            }
            if (ingredientSection)
            {
                if (long.TryParse(range, out long ingredient))
                {
                    ingredients.Add(ingredient);
                }
            }
            else
            {
                var parts = range.Split('-');
                if (
                    parts.Length == 2
                    && long.TryParse(parts[0], out long start)
                    && long.TryParse(parts[1], out long end)
                )
                {
                    ranges.Add((start, end));
                }
            }
        }

        return (ranges, ingredients);
    }

    private static bool IsInRange(long number, long rangeStart, long rangeEnd)
    {
        return number >= rangeStart && number <= rangeEnd;
    }
}
