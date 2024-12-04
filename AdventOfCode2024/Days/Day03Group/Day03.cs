using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    public class Day03 : Day
    {
        public Day03(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 3;

        protected override long ProcessPartOne(string[] input)
        {
            var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            return Regex.Matches(string.Join('\n', input), pattern)            
                .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var pattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";            
            var enabled = true;
            return Regex.Matches(string.Join('\n', input), pattern)
                .Aggregate(0L, (acc, match) =>
                {
                    if (match.Value == "do()")
                    {
                        enabled = true;
                    }
                    else if (match.Value == "don't()")
                    {
                        enabled = false;
                    }
                    else if (enabled)
                    {
                        acc += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
                    }
                    return acc;
                });
        }
    }
}
