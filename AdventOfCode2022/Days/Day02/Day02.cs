using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day02 : Day
    {
        private readonly Dictionary<string, int> _scores = new()
        {
            { "A", 1 },
            { "B", 2 },
            { "C", 3 }
        };

        private readonly Dictionary<string, string> _translations = new()
        {
            { "X", "A" },
            { "Y", "B" },
            { "Z", "C" }
        };

        private readonly Dictionary<string, string> _wins = new()
        {
            { "A", "B" },
            { "B", "C" },
            { "C", "A" }
        };

        public Day02(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 2;

        protected override long ProcessPartOne(string[] input)
        {
            long score = 0;
            foreach (var line in input)
            {
                var plays = line.Split(' ');

                var played = _translations[plays[1]];
                score += _scores[played];
                if (_wins[plays[0]] == played)
                {
                    score += 6;
                }
                else if (plays[0] == played)
                {
                    score += 3;
                }
            }

            return score;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            long score = 0;
            foreach (var line in input)
            {
                var plays = line.Split(' ');

                switch (plays[1])
                {
                    case "X":
                        var played = _wins.First(p => plays[0] == p.Value).Key;
                        score += _scores[played];
                        break;
                    case "Y":
                        score += _scores[plays[0]] + 3;
                        break;
                    case "Z":
                        score += _scores[_wins[plays[0]]] + 6;
                        break;
                }
            }

            return score;
        }
    }
}
