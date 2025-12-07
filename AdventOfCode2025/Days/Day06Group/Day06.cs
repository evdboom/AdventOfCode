using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days;

public class Day06(IFileImporter fileImporter) : Day(fileImporter)
{
    public override int DayNumber => 6;

    protected override long ProcessPartOne(string[] input)
    {
        var operators = input[input.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var values = input[..^1]
            .Select(value =>
                value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()
            )
            .ToList();

        return Enumerable
            .Range(0, operators.Length)
            .Sum(i => GetResult(operators[i], values.Select(v => v[i])));
    }

    protected override long ProcessPartTwo(string[] input)
    {
        var operators = input[input.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var values = ParseColumns(input[..^1]);

        return Enumerable.Range(0, operators.Length).Sum(i => GetResult(operators[i], values[i]));
    }

    private static long GetResult(string op, IEnumerable<long> currentValues)
    {
        return op switch
        {
            "+" => currentValues.Sum(),
            "*" => currentValues.Aggregate(1L, (acc, val) => acc * val),
            _ => throw new InvalidOperationException($"Unsupported operator: {op}"),
        };
    }

    private static List<List<long>> ParseColumns(string[] values)
    {
        var result = new List<List<long>>();
        var length = values.Max(v => v.Length);

        var current = new List<long>();
        for (int i = length - 1; i >= 0; i--)
        {
            if (values.All(value => char.IsWhiteSpace(value[i])))
            {
                result.Insert(0, current);
                current = [];
                continue;
            }

            current.Add(
                long.Parse(string.Join(string.Empty, values.Select(value => value[i])).Trim())
            );
        }
        result.Insert(0, current);

        return result;
    }
}
