using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day02(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 2;

    protected override long ProcessPartOne(string[] input)
    {
        return ParseInput(input[0]).Sum(range => GetInvalidsInRange(range.Start, range.End));
    }

    protected override long ProcessPartTwo(string[] input)
    {
        return ParseInput(input[0]).Sum(range => GetAllInvalidsInRange(range.Start, range.End));
    }

    private static long GetInvalidsInRange(long start, long end)
    {
        return GetEnumerable(start, end).Sum(GetInvalidValue);
    }

    private static long GetAllInvalidsInRange(long start, long end)
    {
        return GetEnumerable(start, end).Sum(GetAllInvalidValue);
    }

    private static long GetInvalidValue(long id)
    {
        return CalculateCount($"{id}", 2) ? id : 0;
    }

    private static long GetAllInvalidValue(long id)
    {
        var value = $"{id}";

        for (int i = 2; i <= value.Length; i++)
        {
            if (CalculateCount(value, i))
            {
                return id;
            }
        }

        return 0;
    }

    private static bool CalculateCount(string value, int step)
    {
        if (step <= 0)
        {
            return false;
        }

        var divisor = value.Length / step;
        if (divisor * step != value.Length || divisor == 0)
        {
            return false;
        }

        var count = value
            .Select((c, index) => (Char: c, Index: index))
            .GroupBy(value => value.Index / divisor)
            .Select(g => new string([.. g.Select(v => v.Char)]))
            .Distinct();

        return count.Count() == 1;
    }

    private static IEnumerable<long> GetEnumerable(long start, long end)
    {
        for (long i = start; i <= end; i++)
        {
            yield return i;
        }
    }

    private static IEnumerable<(long Start, long End)> ParseInput(string input)
    {
        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(range =>
            {
                var parts = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
                return (Start: long.Parse(parts[0]), End: long.Parse(parts[1]));
            });
    }
}
