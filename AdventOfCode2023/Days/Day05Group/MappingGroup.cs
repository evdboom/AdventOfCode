
namespace AdventOfCode2023.Days.Day05Group
{
    public class MappingGroup(string typeFrom, string typeTo)
    {
        private readonly List<Mapping> _mappings = [];

        public string TypeFrom { get; } = typeFrom;
        public string TypeTo { get; } = typeTo;

        public void AddMapping(long sourceStart, long destinationStart, long rangeLength)
        {
            _mappings.Add(new(sourceStart, destinationStart, rangeLength));
        }

        public long GetMapping(long source)
        {
            foreach (var mapping in _mappings)
            {
                var result = mapping.GetMapping(source);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            return source;
        }

        public IEnumerable<(long Start, long Length)> GetMappingRanges((long Start, long Length) source)
        {
            var toMap = new List<(long Start, long Length, bool Mapped)>
            {
                (source.Start, source.Length, false)
            };

            var withMatch = _mappings
                .Where(map => HasMatch(map, source))                    
                .OrderBy(map => map.SourceStart)
                .ToList();

            foreach (var mapping in withMatch)
            {
                toMap = toMap
                    .Where(map => map.Mapped)
                    .Concat(toMap
                        .Where(map => !map.Mapped)
                        .SelectMany(mapping.GetMapping))
                    .ToList();
            }
            return toMap
                .Select(map => (map.Start, map.Length));
        }

        private bool HasMatch(Mapping map, (long Start, long Length) source)
        {
            return
                source.Start <= map.SourceEnd &&
                source.Start + source.Length > map.SourceStart;

        }
    }
}
