using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day12 : Day
    {
        public Day12(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 12;

        protected override long ProcessPartOne(string[] input)
        {
            var result = input
                .Aggregate(0L, (arrangements, row) =>
                {
                    var parts = row.Split(" ");
                    var broken = parts[1]
                        .Split(",")
                        .Select(int.Parse)
                        .ToList();
                    var springs = parts[0];

                    return arrangements + GetPossibleArrangements(springs, broken);
                });

            return result;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var result = input
                .Aggregate(0L, (arrangements, row) =>
                {
                    var parts = row.Split(" ");
                    var broken = string.Join(',', Enumerable.Repeat(parts[1], 5))
                        .Split(",")
                        .Select(int.Parse)
                        .ToList();
                    var springs = string.Join('?', Enumerable.Repeat(parts[0], 5));

                    return arrangements + GetPossibleArrangements(springs, broken);
                });

            return result;
        }

        private long GetPossibleArrangements(string springs, List<int> brokenList)
        {
            return brokenList
                .Aggregate(new List<(int StartIndex, int Size, long Count)> { (0, -1, 1L) }, (arrangements, broken) =>
                {
                    return arrangements
                        .SelectMany(arrangement => GetValidIndexes(springs, arrangement.StartIndex + arrangement.Size + 1, broken)
                            .Select(index => (Index: index, arrangement.Count)))
                        .GroupBy(index => index.Index)
                        .Select(group => (group.Key, broken, group.Sum(index => index.Count)))
                        .ToList();
                })
                .Select(arrangement => (Remainder: springs[(arrangement.StartIndex + arrangement.Size)..], arrangement.Count))
                .Where(arrangement => !arrangement.Remainder.Contains('#'))
                .Sum(arrangement => arrangement.Count);
        }

        private IEnumerable<int> GetValidIndexes(string springs, int startIndex, int broken)
        {
            if (startIndex >= springs.Length)
            {
                yield break;
            }

            var highestStartIndex = springs.IndexOf('#', startIndex);
            var index = startIndex;
            while (index + broken <= springs.Length && (highestStartIndex == -1 || index <= highestStartIndex))
            {
                if (springs[index] != '.')
                {
                    if (springs[index..(index + broken)].All(spring => spring != '.'))
                    {
                        if (springs.Length == index + broken || springs[index + broken] != '#')
                        {
                            yield return index;
                        }
                    }
                }
                index++;
            }
        }
    }
}
