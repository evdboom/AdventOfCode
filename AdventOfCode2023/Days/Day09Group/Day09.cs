using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days
{
    public class Day09 : Day
    {
        public Day09(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 9;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Select(line => GetNextInSequency(line, false))
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Select(line => GetNextInSequency(line, true))
                .Sum();
        }

        private long GetNextInSequency(string line, bool backwards)
        {
            var values = line
                .Split(' ')
                .Select(long.Parse)
                .ToList();

            var result = backwards
                ? values.First()
                : values.Last();
            var even = true;
            while (!values.All(value => value == 0))
            {
                var newValues = new List<long>();
                for (int i = 0; i < values.Count -1; i ++)
                {
                    newValues.Add(values[i+1] - values[i]);
                }
                if (backwards && even)
                {
                    result -= newValues.First();
                }
                else if (backwards && !even)
                {
                    result += newValues.First();
                }
                else
                {
                    result += newValues.Last();
                }
                even = !even;
                values = newValues;
            }

            return result;
        }
    }
}
