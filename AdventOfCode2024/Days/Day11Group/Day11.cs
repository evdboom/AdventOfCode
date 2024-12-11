using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day11(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 11;

        private Dictionary<long, Dictionary<int, long>> _cache = [];

        protected override long ProcessPartOne(string[] input)
        {
            return input[0]
                .Split(' ')
                .Select(long.Parse)
                .Aggregate(0L, (acc, stone) => acc + PerformSteps(stone, 25));

        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input[0]
                .Split(' ')
                .Select(long.Parse)
                .Aggregate(0L, (acc, stone) => acc + PerformSteps(stone, 75));
        }

        private long PerformSteps(long stone, int stepCount)
        {
            if (stepCount == 0)
            {
                return 1;
            }
            else if (_cache.TryGetValue(stone, out var cache) && cache.TryGetValue(stepCount, out var cachedResult))
            {
                return cachedResult;
            }

            var result = Blink(stone)
                .Select(next => PerformSteps(next, stepCount - 1))
                .Sum();
            if (!_cache.TryGetValue(stone, out var innerCache))
            {
                innerCache = new Dictionary<int, long>();
                _cache[stone] = innerCache;
            }

            innerCache[stepCount] = result;
            return result;
        }

        private IEnumerable<long> Blink(long stone)
        {
            if (stone == 0)
            {
                yield return 1;
            }
            else if ($"{stone}" is string stringed && stringed.Length % 2 == 0)
            {                
                yield return long.Parse(stringed[..(stringed.Length / 2)]);
                yield return long.Parse(stringed[(stringed.Length / 2)..]);
            }
            else
            {
                yield return stone * 2024;
            }
        }
    }
}
