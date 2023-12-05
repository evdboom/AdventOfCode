using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day05Group;

namespace AdventOfCode2023.Days
{
    public class Day05 : Day
    {
        public Day05(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 5;

        protected override long ProcessPartOne(string[] input)
        {
            var mapped = GetSeeds(input[0]);
            var mappings = GetMappings(input);

            var typeFrom = "seed";
            MappingGroup mapping;
            do
            {
                mapping = mappings.First(map => map.TypeFrom == typeFrom);
                typeFrom = mapping.TypeTo;

                mapped = mapped
                    .Select(map => mapping.GetMapping(map))
                    .ToList();
            }
            while (mapping.TypeTo != "location");

            return mapped.Min();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var mapped = GetSeedRanges(input[0]).ToList();
            var mappings = GetMappings(input);

            var typeFrom = "seed";
            MappingGroup mapping;
            do
            {
                mapping = mappings.First(map => map.TypeFrom == typeFrom);
                typeFrom = mapping.TypeTo;

                mapped = mapped
                    .SelectMany(map => mapping.GetMappingRanges(map))
                    .GroupBy(map => map.Start)
                    .Select(group => (group.Key, group.Max(map => map.Length)))
                    .ToList();

            }
            while (mapping.TypeTo != "location");

            return mapped
                .Select(map => map.Start)
                .Min();
        }

        private List<long> GetSeeds(string line)
        {
            return line
                .Replace("seeds: ", string.Empty)
                .Split(" ")
                .Select(long.Parse)
                .ToList();
        }

        private IEnumerable<(long Start, long Length)> GetSeedRanges(string line)
        {
            var values = GetSeeds(line);

            for (int i = 0; i < values.Count; i += 2)
            {
                yield return (values[i], values[i + 1]);                
            }
        }

        private List<MappingGroup> GetMappings(string[] input)
        {
            var lines = input
                .Where(line => !line.StartsWith("seeds:") && !string.IsNullOrEmpty(line));

            var result = new List<MappingGroup>();
            MappingGroup? current = null;
            foreach (var line in lines)
            {
                if (line.EndsWith(" map:"))
                {
                    var types = line
                        .Replace(" map:", string.Empty)
                        .Split("-to-");
                    current = new MappingGroup(types[0], types[1]);
                    result.Add(current);
                }
                else
                {
                    var values = line
                        .Split(" ")
                        .Select(long.Parse)
                        .ToList();

                    current!.AddMapping(values[1], values[0], values[2]);
                }
            }

            return result;
        }
    }
}
