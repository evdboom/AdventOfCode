using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day01 : Day
    {
        public Day01(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 1;

        private List<int> Aggragate(string[] input)
        {
            return input
                .Aggregate(new List<int>() { 0 }, (current, next) =>
                {
                    if (string.IsNullOrEmpty(next))
                    {
                        current.Insert(0, 0);
                    }
                    else
                    {
                        current[0] += int.Parse(next);
                    }
                    return current;
                });
        }

        protected override long ProcessPartOne(string[] input)
        {
            return Aggragate(input)
                .Max();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return Aggragate(input)
                .OrderByDescending(e => e)
                .Take(3)
                .Sum();
        }
    }
}
