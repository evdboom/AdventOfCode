namespace AdventOfCode2022.Days.Day20Group
{
    public record MixPart
    {
        public long Value { get; set; }
        public int TimesMoved { get; set; }
        public int OriginalIndex { get; set; }
    }
}
