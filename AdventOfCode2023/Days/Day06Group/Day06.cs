using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day06 : Day
    {
        public Day06(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 6;

        protected override long ProcessPartOne(string[] input)
        {
            var times = input[0]
                .Replace("Time:", string.Empty)
                .Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();
            var distances = input[1]
                .Replace("Distance:", string.Empty)
                .Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            return times
                .Select((time, index) => ValidChargeTimesToBeat(time, distances[index]))
                .Aggregate(1L, (previous, current) => previous * current);

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var time = long.Parse(input[0]
                .Replace("Time:", string.Empty)
                .Replace(" ", string.Empty));
            var distance = long.Parse(input[1]
                .Replace("Distance:", string.Empty)
                .Replace(" ", string.Empty));

            return ValidChargeTimesToBeat(time, distance);                
        }

        private long ValidChargeTimesToBeat(double raceTime, double distanceToBeat)
        {
            var d = Math.Sqrt(raceTime * raceTime - 4 * distanceToBeat);
            var lowest = (raceTime - d) / 2;
            var highest = (raceTime + d) / 2;
            var lowestWhole = Math.Floor(lowest + 1); // floor +1 because has to be more then to beat (instead of ceiling lowest)
            var highestWhole = Math.Ceiling(highest - 1); // ceiling -1 because has to be more then to beat (instead of floor highest)
            return (long)(highestWhole - lowestWhole + 1); // lowest is inclusive
        }
    }
}
