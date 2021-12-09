using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day01 : Day
    {
        public override int DayNumber => 1;
        public Day01(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] lines)
        {
            var increased = 0;
            int previous = int.MaxValue;
            foreach (var line in lines)
            {
                if (int.TryParse(line, out int next))
                {
                    if (next > previous)
                    {
                        increased++;
                    }

                    previous = next;
                }
            }

            return increased;
        }

        protected override long ProcessPartTwo(string[] lines)
        {
            var parsedlines = lines
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => int.Parse(line))
                .ToList();
            var increased = 0;
            int previous = int.MaxValue;
            for(int i = 0; i < parsedlines.Count; i ++)
            {
                if (i < parsedlines.Count - 2)
                {
                    var next = parsedlines[i] + parsedlines[i + 1] + parsedlines[i + 2];

                    if (next > previous)
                    {
                        increased++;
                    }

                    previous = next;

                }               
            }

            return increased;
        }
    }
}
