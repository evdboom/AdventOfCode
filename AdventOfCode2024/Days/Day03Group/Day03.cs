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
            var merged = string.Join('\n', input);
            return ProcessLine(merged);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var merged = string.Join('\n', input);
            var line = CleanLine(merged);
            return ProcessLine(line);
        }

        private string CleanLine(string line)
        {
            return string.Join(string.Empty, line
                .Split("do()")
                .Select(part => part
                    .Split("don't()")[0]));
        }

        private long ProcessLine(string line)
        {
            var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            var matches = Regex.Matches(line, pattern);
            return matches
                .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        }
    }
}
