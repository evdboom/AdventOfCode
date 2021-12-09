using AdventOfCode2021.Constructs;
using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day06 : Day
    {
        private const char SplitChar = ',';

        public override int DayNumber => 6;

        public Day06(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            return Process(input, 80);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return Process(input, 256);
        }

        private long Process(string[] input, int counter)
        {
            var schools = input[0]
                .Split(SplitChar)
                .Select(i => int.Parse(i))
                .GroupBy(i => i)
                .Select(g => new Day06FishSchool(g.Key, g.Count()))
                .ToList();

            for (int i = 1; i <= counter; i++)
            {
                Day06FishSchool? newSchool = null;
                foreach (var school in schools)
                {
                    school.PassDay(out bool reproduces);
                    if (reproduces)
                    {
                        newSchool = new Day06FishSchool(school.Size);
                    }
                }

                if (newSchool is not null)
                {
                    schools.Add(newSchool);
                }                

                schools = schools
                    .GroupBy(s => s.Counter)
                    .Select(g => new Day06FishSchool(g.Key, g.Sum(s => s.Size)))
                    .ToList();
            }

            return schools
                .Sum(s => s.Size);
        }
    }
}
