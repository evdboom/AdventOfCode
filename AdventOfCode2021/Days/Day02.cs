using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day02 : Day
    {
        private const string Forward = "forward";
        private const string Down = "down";
        private const string Up = "up";
        private const char SplitChar = ' ';

        public override int DayNumber => 2;

        public Day02(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] lines)
        {         
            var horizontal = 0;
            var depth = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var parts = line.Split(SplitChar);
                switch (parts[0])
                {
                    case Forward:
                        horizontal += int.Parse(parts[1]);
                        break;
                    case Down:
                        depth += int.Parse(parts[1]);
                        break;
                    case Up:
                        depth -= int.Parse(parts[1]);
                        break;
                }
            }

            return horizontal * depth;
        }

        protected override long ProcessPartTwo(string[] lines)
        {
            var horizontal = 0;
            var depth = 0;
            var aim = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var parts = line.Split(' ');
                switch (parts[0])
                {
                    case Forward:
                        horizontal += int.Parse(parts[1]);
                        depth += aim * int.Parse(parts[1]);
                        break;
                    case Down:
                        aim += int.Parse(parts[1]);
                        break;
                    case Up:
                        aim -= int.Parse(parts[1]);
                        break;
                }
            }

            return horizontal * depth;
        }
    }
}
