using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day01 : Day
    {
        public Day01(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 1;

        protected override long ProcessPartOne(string[] input)
        {
            PriorityQueue<int, int> one = new();
            PriorityQueue<int, int> two = new();

            foreach (var item in input)
            {
                var parts = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var first = int.Parse(parts[0]);
                var second = int.Parse(parts[1]);
                one.Enqueue(first, first);
                two.Enqueue(second, second);
            }

            long result = 0;
            while(one.TryDequeue(out int first, out _))
            {
                var second = two.Dequeue();
                result += Math.Abs(first - second);
            }

            return result;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            List<int> one = [];
            List<int> two = [];

            foreach (var item in input)
            {
                var parts = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                one.Add(int.Parse(parts[0]));
                two.Add(int.Parse(parts[1]));                
            }

            var groupedOne = one
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var groupedTwo = two
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            var result = groupedOne.Aggregate(0L, (acc, x) =>
            {
                var count = groupedTwo.GetValueOrDefault(x.Key, 0);
                return acc + x.Key * x.Value * count;
            });

            return result;
        }
    }
}
