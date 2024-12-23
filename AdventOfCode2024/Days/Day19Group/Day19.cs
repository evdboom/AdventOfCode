using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day19(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 19;

        protected override long ProcessPartOne(string[] input)
        {
            var (towels, patterns) = GetTowelsAndPatterns(input);
            var cache = new Dictionary<string, long>();
            return patterns
                .Aggregate(0L, (acc, pattern) => acc + CanMakePattern(pattern, towels, cache, false));

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (towels, patterns) = GetTowelsAndPatterns(input);
            var cache = new Dictionary<string, long>();
            return patterns
                .Aggregate(0L, (acc, pattern) => acc + CanMakePattern(pattern, towels, cache, true));
        }

        private long CanMakePattern(string pattern, List<string> towels, Dictionary<string, long> cache, bool getAll)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return 1;
            }
            else if (cache.TryGetValue(pattern, out var value))
            {
                return value;
            }

            var result = 0L;
            foreach (var towel in towels)
            {
                if (pattern.StartsWith(towel))
                {
                    result += CanMakePattern(pattern[towel.Length..], towels, cache, getAll);
                    if (result > 0 && !getAll)
                    {
                        break;
                    }
                }
            }
            cache[pattern] = result;
            return result;
        }

        private (List<string> Towels, List<string> Patterns) GetTowelsAndPatterns(string[] input)
        {
            var towels = input[0].Split(", ").ToList();
            var patterns = new List<string>();

            for (int i = 2; i < input.Length; i++)
            {
                patterns.Add(input[i]);
            }

            return (towels, patterns);
        }
    }
}
