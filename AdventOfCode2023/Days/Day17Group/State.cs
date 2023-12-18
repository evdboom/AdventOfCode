using System.Drawing;

namespace AdventOfCode2023.Days.Day17Group
{
    public record State
    {
        public int Distance { get; set; }
        public int HeatLoss { get; set; }
        public char Direction { get; set; }
        public int DirectionLength { get; set; }
    }
}
