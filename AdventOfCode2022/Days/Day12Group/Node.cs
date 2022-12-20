namespace AdventOfCode2022.Days.Day12Group
{
    public class Node
    {
        public Node()
        {
            Connections = new List<Node>();
            Distance = int.MaxValue;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public int Distance { get; set; }
        public int Value { get; set; }
        public List<Node> Connections { get; set; }
        public bool Visited { get; set; }
    }
}
