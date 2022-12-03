using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day03 : Day
    {
       
        public Day03(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 3;

        protected override long ProcessPartOne(string[] input)
        {

            return input
                .Aggregate(0, (value, line) =>
                {                    
                    var middle = line.Length / 2;
                    return value + line[..middle]
                        .Intersect(line[middle..])
                        .Select(CharValue)
                        .Single();                    
                });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Select((line, index) => new { Index = index / 3, Line = line as IEnumerable<char> })
                .GroupBy(value => value.Index)
                .Select(g => g
                    .Select(v => v.Line)
                    .Aggregate((current, next) => current.Intersect(next))
                    .Select(CharValue)
                    .Single())
                .Sum();
        }

        private int CharValue(char c)
        {
            return (c % 32) + (char.IsUpper(c) ? 26 : 0);                
        }
    }
}
