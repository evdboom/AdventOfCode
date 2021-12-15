namespace AdventOfCode2021.Constructs
{
    public class Day15Node
    {
        public Day15Node()
        {
            Connections = new List<Day15Node>();
            Distance = int.MaxValue;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public int Distance { get; set; }
        public int Value { get; set; }
        public List<Day15Node> Connections { get; set; }
        public bool Visited { get; set; }
    }
}
