using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023.Days.Day05Group
{
    public record Mapping
    {
        public Mapping(long sourceStart, long destinationStart, long rangeLength)
        {
            SourceStart = sourceStart;
            DestinationStart = destinationStart;
            RangeLength = rangeLength;
        }

        public long SourceStart { get; }                
        public long DestinationStart { get; }        
        public long RangeLength { get; }
        public long SourceEnd => SourceStart + RangeLength;

        public long? GetMapping(long source)
        {
            if (source < SourceStart || source > SourceEnd)
            {
                return null;
            }
            
            return DestinationStart - SourceStart + source;
        }

        public IEnumerable<(long Start, long Length, bool Mapped)> GetMapping((long Start, long Length, bool Mapped) source)
        {
            if (source.Mapped)
            {
                yield break;
            }

            if (source.Start < SourceStart) {
                var length = Math.Min(source.Length, SourceStart - source.Start);
                yield return (source.Start, length, false);

            }

            var start = Math.Max(source.Start, SourceStart);
            var delta = start - SourceStart;
            var end = Math.Min(source.Start + source.Length, SourceEnd);
            if (end > start)
            {
                yield return (DestinationStart + delta, end - start, true);
            }
            
            if (source.Start + source.Length > SourceEnd) 
            {
                var afterStart = Math.Max(source.Start, SourceEnd);
                var length = source.Start + source.Length - afterStart;
                yield return (afterStart, length, false);
            }
        }
    }
}
