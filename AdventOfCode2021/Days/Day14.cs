using AdventOfCode2021.Constructs;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day14 : Day
    {
        private const string InsertionSplit = " -> ";        

        public Day14(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 14;

        protected override long ProcessPartOne(string[] input)
        {
            var rules = GetRules(input);
            var template = FillInitialTemplate(input[0], rules);

            for (int i = 0; i < 10; i++)
            {
                var newPairs = new List<Day14Pair>();
                foreach(var pair in template)
                {
                    foreach(var step in pair.ProcessStep(rules))
                    {
                        newPairs.Add(step);
                    }
                }

                template = newPairs;
            }

            var values = template
                .SelectMany(t => t.GetCharacterCount())
                .GroupBy(c => c.Key)
                .Select(g => g.Sum(c => c.Value))
                .OrderByDescending(c => c)
                .ToList();

            var max = (long)(Math.Ceiling(values.Max() / 2d));
            var min = (long)(Math.Ceiling(values.Min() / 2d));

            return max - min;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> GetRules(string[] input)
        {
            return input
                .Where(i => i.Contains(InsertionSplit))
                .Select(i => i.Split(InsertionSplit))
                .ToDictionary(i => i[0], i => i[1]);
        }

        private List<Day14Pair> FillInitialTemplate(string line, Dictionary<string, string> rules)
        {
            var result = new List<Day14Pair>();
            for(int i = 0; i < line.Length -1; i++)
            {
                var code = $"{line[i]}{line[i + 1]}";
                result.Add(new Day14Pair(code, rules[code]));
            }

            return result;
        }
    }
}
