using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
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
                .Select(line =>
                {
                    var numberParts = line
                        .Split(": ")[1]
                        .Split(" | ");
                    var winning = GetNumbers(numberParts[0]);
                    var having = GetNumbers(numberParts[1]);

                    var match = having
                        .Intersect(winning)
                        .Count();

                    if (match == 0)
                    {
                        return 0L;
                    }

                    return (long)Math.Pow(2, match - 1);
                })
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Aggregate((Cards: new Dictionary<int, long>(), Card: 0), (value, line) =>
                {
                    value.Card++;
                    var cards = value.Cards.TryGetValue(value.Card, out long currentCards)
                        ? currentCards + 1
                        : 1;
                    value.Cards[value.Card] = cards;
                    
                    var numberParts = line
                        .Split(": ")[1]
                        .Split(" | ");
                    var winning = GetNumbers(numberParts[0]);
                    var having = GetNumbers(numberParts[1]);

                    var match = having
                        .Intersect(winning)
                        .Count();

                    for(int i = 1; i <= match; i++)
                    {
                        if (value.Cards.ContainsKey(value.Card + i))
                        {
                            value.Cards[value.Card + i] += cards;
                        }
                        else
                        {
                            value.Cards[value.Card + i] = cards;
                        }
                    }

                    return value;

                })
                    .Cards
                    .Values
                    .Sum();
        }

        private List<int> GetNumbers(string input)
        {
            return input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}
