namespace AdventOfCode2021.Constructs.Day04
{
    public record Cell
    {
        public int Row { get; init; }
        public int Column { get; init; }
        public int Value { get; init; }
        public bool Marked { get; set; }
    }
}
