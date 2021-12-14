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
                var newPairs = new Dictionary<string, Day14Pair>();
                foreach (var pair in template)
                {
                    foreach (var step in pair.Value.ProcessStep(rules))
                    {
                        if (!newPairs.ContainsKey(step.Code))
                        {
                            newPairs[step.Code] = step;
                        }
                        else
                        {
                            newPairs[step.Code].Count += step.Count;
                        }
                    }
                }

                template = newPairs;
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

        private Dictionary<string, Day14Pair> FillInitialTemplate(string line, Dictionary<string, string> rules)
        {
            var result = new Dictionary<string, Day14Pair>();
            for(int i = 0; i < line.Length -1; i++)
            {
                var code = $"{line[i]}{line[i + 1]}";
                if (!result.ContainsKey(code))
                {
                    result.Add(code, new Day14Pair(code, rules[code]));
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
