using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Constructs.Day12;

namespace AdventOfCode2021.Days
{
    public class Day12 : Day
    {
        private const char CaveConnection = '-';
        private const string StartCave = "start";
        private const string EndCave = "end";

        public Day12(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 12;

        protected override long ProcessPartOne(string[] input)
        {
            MapCaves(input, out Cave start);
            var run = new List<string>();
            long paths = 0;
            GetPaths(start, ref paths, run, false);

            return paths;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            MapCaves(input, out Cave start);
            var run = new List<string>();
            long paths = 0;
            GetPaths(start, ref paths, run, true);

            return paths;
        }

        private static void GetPaths(Cave cave, ref long paths, List<string> current, bool allowDoubleSmall)
        {
            current.Add(cave.Code);

            if (cave.Code.Equals(EndCave))
            {
                paths++;
                return;
            }

            allowDoubleSmall = allowDoubleSmall && (cave.LargeCave || current.Count(c => c.Equals(cave.Code)) == 1);
            var connections = cave.Connections.Values
                .Where(c =>
                    c.LargeCave ||
                    (!c.Code.Equals(StartCave) && allowDoubleSmall) ||
                    !current.Contains(c.Code))
                .ToList();
            foreach (var connection in connections)
            {
                GetPaths(connection, ref paths, current.ToList(), allowDoubleSmall);
            }
        }

        private void MapCaves(string[] input, out Cave start)
        {
            Cave? startCave = null;
            var caves = new Dictionary<string, Cave>();
            foreach (var line in input)
            {
                var caveSet = line
                    .Split(CaveConnection);

                if (!caves.TryGetValue(caveSet[0], out Cave? caveOne))
                {
                    caveOne = new Cave(caveSet[0]);
                    caves[caveSet[0]] = caveOne;
                }
                if (!caves.TryGetValue(caveSet[1], out Cave? caveTwo))
                {
                    caveTwo = new Cave(caveSet[1]);
                    caves[caveSet[1]] = caveTwo;
                }

                if (!caveOne.Connections.ContainsKey(caveTwo.Code))
                {
                    caveOne.Connections[caveTwo.Code] = caveTwo;
                }
                if (!caveTwo.Connections.ContainsKey(caveOne.Code))
                {
                    caveTwo.Connections[caveOne.Code] = caveOne;
                }

                if (caveOne.Code.Equals(StartCave))
                {
                    startCave = caveOne;
                }
                else if (caveTwo.Code.Equals(StartCave))
                {
                    startCave = caveTwo;
                }
            }

            if (startCave == null)
            {
                throw new InvalidOperationException("Could not find start cave");
            }

            start = startCave;
        }
    }
}
