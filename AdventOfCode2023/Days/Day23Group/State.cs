using System.Drawing;

namespace AdventOfCode2023.Days.Day23Group
{
    public class State
    {
        public int Distance { get; set; }
        public Intersection Location { get; set; } = null!;
        public List<Intersection> Visited { get; set; } = [];
    }
}
