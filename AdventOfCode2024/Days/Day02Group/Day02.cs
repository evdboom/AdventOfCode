using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day02 : Day
    {
        public Day02(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 2;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Where(line => ValidLine(line
                    .Split(' ')
                    .Select(int.Parse)))
                .Count();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var reports = input
                .Select(line => line
                    .Split(' ')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();

            var skip = -1;
            var safe = 0L;

            while (reports.Count > 0)
            {
                var skipped = reports
                    .Where(line => !ValidLine(line
                        .Where((_, index) => index != skip)))
                    .ToList();

                safe += reports.Count - skipped.Count;

                skip++;
                reports = skipped
                    .Where(line => line.Length > skip)
                    .ToList();
            }

            return safe;
        }

        private bool ValidLine(IEnumerable<int> line)
        {
            var levels = new Queue<int>(line);

            bool? descending = null;
            var last = levels.Dequeue();
            while (levels.TryDequeue(out var level))
            {
                if (!descending.HasValue)
                {
                    descending = last > level;
                }

                var newDescending = last > level;
                if (descending != newDescending)
                {
                    return false;
                }

                var diff = Math.Abs(last - level);
                if (diff < 1 || diff > 3)
                {
                    return false;
                }
                last = level;
            }

            return true;
        }
    }
}
