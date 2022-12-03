using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day02 : Day
    {
        public Day02(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 2;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Select(line => line
                    .Split(' ')                
                    .Select((item, index) => item[0] - (index == 0 ? 'A' -1 : 'X' -1))
                    .ToArray())
                .Aggregate(0, (value, values) =>
            {
                var result = values[1] - values[0];
                result = result < 0
                    ? ((result - 4) % 3) + 1
                    : ((result + 4) % 3) - 1;

                return value + values[1] + (result + 1) * 3;
            });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Select(line => line
                    .Split(' ')
                    .Select((item, index) => item[0] - (index == 0 ? 'A' -1 : 'X' + 1))
                    .ToArray())
                .Aggregate(0, (value, values) =>
            {
                var result = (values[0] + values[1]) % 3;
                result = result == 0 ? 3 : result;
                return value + result + (values[1] + 1) * 3;

            });       
        }
    }
}
