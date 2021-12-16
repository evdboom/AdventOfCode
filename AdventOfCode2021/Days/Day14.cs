using AdventOfCode2021.Constructs;
using AdventOfCode2021.Constructs.Day14;
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
            return Process(10, input);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return Process(40, input);
        }

        private long Process(int steps, string[] input)
        {
            var rules = GetRules(input);
            var template = FillInitialTemplate(input[0], rules);

            var startChar = input[0].First();
            var endChar = input[0].Last();

            for (int i = 0; i < steps; i++)
            {
                template = template
                    .SelectMany(pair => pair.Value.ProcessStep(rules))
                    .GroupBy(pair => pair.Code)
                    .ToDictionary(g => g.Key, g => new Pair(g.Key, rules[g.Key]) { Count = g.Sum(pair => pair.Count) });
            }

            var values = template
                .SelectMany(t => t.Value.GetCharacterCount())
                .GroupBy(c => c.Key)
                .ToDictionary(g => g.Key, g => g.Sum(c => c.Value));

            // outer characters are only counted once.
            values[startChar]++;
            values[endChar]++;            

            // All characters are counted twice
            var max = values.Max(c => c.Value) / 2;
            var min = values.Min(c => c.Value) / 2;

            return max - min;
        }

        private Dictionary<string, string> GetRules(string[] input)
        {
            return input
                .Where(i => i.Contains(InsertionSplit))
                .Select(i => i.Split(InsertionSplit))
                .ToDictionary(i => i[0], i => i[1]);
        }

        private Dictionary<string, Pair> FillInitialTemplate(string line, Dictionary<string, string> rules)
        {
            var result = new Dictionary<string, Pair>();
            for(int i = 0; i < line.Length -1; i++)
            {
                var code = line.Substring(i, 2);
                if (!result.ContainsKey(code))
                {
                    result[code] = new Pair(code, rules[code]);
                }
                else
                {
                    result[code].Count++;
                }
            }

            return result;
        }
    }
}
