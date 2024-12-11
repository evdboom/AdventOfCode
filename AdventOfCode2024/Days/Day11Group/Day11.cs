using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day11(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 11;

        protected override long ProcessPartOne(string[] input)
        {
            return input[0]
                .Split(' ')
                .Select(stone => PerformSteps(long.Parse(stone), 25, []))
                .Sum();

        }

        protected override long ProcessPartTwo(string[] input)
        {            
            return input[0]
                .Split(' ')
                .Select(stone => PerformSteps(long.Parse(stone), 75, []))
                .Sum();                
        }

        private long PerformSteps(long stone, int stepCount, Dictionary<(long Stone, int Step), long> cache)
        {
            if (stepCount == 0)
            {
                return 1;
            }
            else if (cache.TryGetValue((stone, stepCount), out var cachedResult))
            {
                return cachedResult;
            }

            var result = Blink(stone)
                .Select(next => PerformSteps(next, stepCount - 1, cache))
                .Sum();

            cache[(stone, stepCount)] = result;
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
