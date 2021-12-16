using System.Drawing;

namespace AdventOfCode2021.Constructs.Day05
{
    public record Line
    {
        public Point Start { get; init; }
        public Point End { get; init; }
    }
}
