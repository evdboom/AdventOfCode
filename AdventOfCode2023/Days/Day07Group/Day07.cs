using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day07Group;

namespace AdventOfCode2023.Days
{
    public class Day07 : Day
    {
        public Day07(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 7;

        protected override long ProcessPartOne(string[] input)
        {
            var x = input
                .Select(line =>
                {
                    var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    return new Hand
                    {
                        Cards = parts[0],
                        Bid = long.Parse(parts[1])
                    };
                })
                .OrderDescending();

            return input
                .Select(line =>
                {
                    var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    return new Hand
                    {
                        Cards = parts[0],
                        Bid = long.Parse(parts[1])
                    };
                })
                .OrderDescending()
                .Select((hand, index) => hand.Bid * (index + 1))
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Select(line =>
                 {
                     var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                     return new Hand
                     {
                         Cards = parts[0],
                         Bid = long.Parse(parts[1]),
                         JokerGame = true
                     };
                 })
                .OrderDescending()
                .Select((hand, index) => hand.Bid * (index + 1))
                .Sum();
        }
    }
}
