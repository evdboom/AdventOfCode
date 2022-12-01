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

        protected override long ProcessPartOne(string[] input)
        {
            var max = 0;
            var current = 0;
            foreach (var item in input)
            {
                if (string.IsNullOrEmpty(item))
                {
                    if (current > max)
                    {
                        max = current;                        
                    }
                    current = 0;
                }
                else
                {                    
                    current += int.Parse(item);
                }                
            }
            if (current > max) 
            {
                max = current;
            }

            return max;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var elves = new List<int>();
            var current = 0;
            foreach (var item in input)
            {
                if (string.IsNullOrEmpty(item))
                {
                    elves.Add(current);
                    current = 0;
                }
                else
                {
                    current += int.Parse(item);
                }
            }
            elves.Add(current);

            return elves
                .OrderByDescending(e => e)
                .Take(3)
                .Sum();
        }
    }
}
