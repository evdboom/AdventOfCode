using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day01 : Day
    {
        private readonly Dictionary<string, int> _numbers;

        public Day01(IFileImporter importer) : base(importer)
        {
            _numbers = new()
            {
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3,
                ["four"] = 4,
                ["five"] = 5,
                ["six"] = 6,
                ["seven"] = 7,
                ["eight"] = 8,
                ["nine"] = 9,
            };

        }

        public override int DayNumber => 1;

      
        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Aggregate(0, (previous, next) =>
                {
                    var first = next.First(c => char.IsNumber(c));
                    var last = next.Last(c => char.IsNumber(c));
                    var number = int.Parse($"{first}{last}");
                    return previous + number;
                });            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Aggregate(0, (previous, next) =>
                {
                    foreach(var digit in _numbers)
                    {
                        next = next.Replace(digit.Key, $"{digit.Key}{digit.Value}{digit.Key}");
                    }

                    var first = next.First(c => char.IsNumber(c));
                    var last = next.Last(c => char.IsNumber(c));
                    var number = int.Parse($"{first}{last}");
                    return previous + number;
                });
        }
    }
}
