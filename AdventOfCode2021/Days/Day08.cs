using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day08 : Day
    {
        private const string PartSplit = " | ";
        private const char DigitSplit = ' ';
        public override int DayNumber => 8;

        private const int OneLength = 2;
        private const int TwoLength = 5;
        private const int FourLength = 4;
        private const int SevenLength = 3;
        private const int EightLength = 7;
        private const int NineLength = 6;

        public Day08(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            var differentLengths = new[]
            {
                OneLength,
                FourLength,
                SevenLength,
                EightLength
            };

            return input.Sum(part => part
                .Split(PartSplit)[1]
                .Split(DigitSplit)
                .Count(o => differentLengths.Contains(o.Length)));
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input.Sum(part => GetValue(part));
        }

        private int GetValue(string part)
        {
            var parts = part
                .Split(PartSplit);
            var inputPart = parts[0]
                .Split(DigitSplit)
                .Select(p => new string(p.OrderBy(s => s).ToArray()))
                .ToArray();
            var outputPart = parts[1]
                .Split(DigitSplit)
                .Select(p => new string(p.OrderBy(s => s).ToArray()))
                .ToArray();

            var sections = DetermineSections(inputPart);

            return int.Parse(string.Join("", outputPart.Select(part => sections.Single(s => s.Value == part).Key)));
        }

        private Dictionary<int, string> DetermineSections(string[] inputPart)
        {
            var parts = inputPart.ToList();

            // Determine numbers with unique segment count
            var results = new Dictionary<int, string>
            {
                [1] = parts.Single(i => i.Length == OneLength),
                [4] = parts.Single(i => i.Length == FourLength),
                [7] = parts.Single(i => i.Length == SevenLength),
                [8] = parts.Single(i => i.Length == EightLength)
            };

            // remove found parts from the list
            parts.Remove(results[1]);
            parts.Remove(results[4]);
            parts.Remove(results[7]);
            parts.Remove(results[8]);

            // 9 can be determined when you subtract 4 and 7 only 1 segment should remain and has a length of 6 (otherwise it's 3 or 5).
            results[9] = parts.Single(part =>
                part.Except(results[4]).Except(results[7]).Count() == 1 &&
                part.Length == NineLength);
            parts.Remove(results[9]);

            // To determine 3, you subtract 4 and 7 and only 1 segment remains (possibly 5 or 3 as 9 is already removed) 
            // Then when you subtract it from 1, 3 will have no segments left (and 5 still 1).
            results[3] = parts.Single(part =>
                part.Except(results[4]).Except(results[7]).Count() == 1 &&
                !results[1].Except(part).Any());
            parts.Remove(results[3]);

            // As 3 is now removed, subtracting 4 and 7 will yield 5
            results[5] = parts.Single(part => part.Except(results[4]).Except(results[7]).Count() == 1);
            parts.Remove(results[5]);

            // 6 is 5 with one extra segment (as 9 already is removed);
            results[6] = parts.Single(part => part.Except(results[5]).Count() == 1);
            parts.Remove(results[6]);

            // 2 and 0 are left 2 has length 5, while zero has length 6.
            results[2] = parts.Single(part => part.Length == TwoLength);
            parts.Remove(results[2]);

            // just 1 (0) left.
            results[0] = parts.Single();
            parts.Remove(results[0]);

            return results;
        }
    }
}
