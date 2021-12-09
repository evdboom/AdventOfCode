using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day07 : Day
    {
        private const char ValueSplit = ',';

        public override int DayNumber => 7;

        public Day07(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            return Process(input, (reference, position) => Math.Abs(reference - position));
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return Process(input, (reference, position) => GetFuelBurn(reference, position));
        }

        private long Process(string[] input, Func<int, int, long> sumFunction)
        {
            var positions = input[0]
                .Split(ValueSplit)
                .Select(v => int.Parse(v))
                .ToArray();

            var min = positions.Min();
            var max = positions.Max();
            var distinct = Enumerable.Range(min, max - min + 1)
                .ToArray();

            var result = distinct
                .Min(d => positions.Sum(p => sumFunction(d, p)));

            return result;
        }

        private int GetFuelBurn(int reference, int position)
        {
            var distance = Math.Abs(reference - position);
            return distance * (distance + 1) / 2;
        }
    }
}
