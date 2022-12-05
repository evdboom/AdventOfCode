using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day04 : Day
    {
       
        public Day04(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 4;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Aggregate(0, (value, pair) =>
                {
                    var overlaps = IsOverlap(pair
                        .Split(',')
                        .Select(e => e
                            .Split('-')
                            .Select(b => int.Parse(b))
                            .ToArray()));
                        
                    if (overlaps)
                    {
                        value++;
                    }

                    return value;
                });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Aggregate(0, (value, pair) =>
                {
                    var overlaps = IsOverlap(pair
                        .Split(',')
                        .Select(e => e
                            .Split('-')
                            .Select(b => int.Parse(b))
                            .ToArray()), true);

                    if (overlaps)
                    {
                        value++;
                    }

                    return value;
                });
        }

        private bool IsOverlap(IEnumerable<int[]> pair, bool partial = false)
        {
            var first = pair.First();
            var second = pair.Last();

            if (partial)
            {
                return first[0] <= second[1] && first[1] >= second[0];
            }
            else
            {
                var one = first[0] - second[0];
                var two = second[1] - first[1];
                return (one <= 0 && two <= 0) 
                    || (one >= 0 && two >= 0);
            }               
        }
    }
}
