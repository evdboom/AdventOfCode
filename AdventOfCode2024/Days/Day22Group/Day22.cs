using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day22(IFileImporter importer) : Day(importer)
    {

        public override int DayNumber => 22;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Select(line => GetSecretNumber(long.Parse(line), 2000))
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var patterns = new Dictionary<(int, int, int, int), long>();
            foreach (var line in input)
            {
                var number = long.Parse(line);
                var line_patterns = new HashSet<(int, int, int, int)>();
                var pattern = new List<int>();

                for (int i = 0; i < 2000; i++)
                {
                    var last = number % 10;
                    number = GetSecretNumber(number, 1);
                    var next = number % 10;
                    pattern.Add((int)(last - next));

                    if (i > 2 && line_patterns.Add((pattern[i - 3], pattern[i - 2], pattern[i - 1], pattern[i])))
                    {
                        var current  = patterns.TryGetValue((pattern[i - 3], pattern[i - 2], pattern[i - 1], pattern[i]), out var c) 
                            ? c
                            : 0;
                        patterns[(pattern[i - 3], pattern[i - 2], pattern[i - 1], pattern[i])] = current + next;
                    }
                }
            }

            return patterns.Values.Max();
        }

        private long GetSecretNumber(long number, int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                number = ((number * 64) ^ number) % 16777216;
                number = ((number / 32) ^ number) % 16777216;
                number = ((number * 2048) ^ number) % 16777216;
            }

            return number;
        }
    }
}
