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
            return input.Aggregate(0, (value, line) =>
            {
                var values = line
                    .Split(' ')
                    .Select((item, index) => 1 + item[0] - (index == 0 ? 'A' : 'X'))
                    .ToArray();

                values[1] = values[0] == 1 ? values[1] % 3 : values[1];                
                values[0] = values[1] == 1 ? values[0] % 3 : values[0];

                return value + values[1] + (values[1] - values[0] + 1) * 3;
            });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input.Aggregate(0, (value, line) =>
            {
                var values = line
                    .Split(' ')
                    .Select((item, index) => item[0] - (index == 0
                        ? 'A' - 1
                        : 'X' + 1))
                    .ToArray();
                var result = (values[0] + values[1]) % 3;
                result = result == 0 ? 3 : result;
                return value + result + (values[1] + 1) * 3;

            });       
        }
    }
}
