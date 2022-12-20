namespace AdventOfCode2022.Days.Day16Group
{
    public class Valve
    {
        public string Name { get; set; } = string.Empty;
        public int FlowRate { get; set; }
        public List<Valve> Connections { get; } = new();
        public Dictionary<string, bool> Visited { get; } = new();
        public Dictionary<string, List<Valve>> Paths { get; } = new();
        public Dictionary<string, int> Distances { get; } = new();
    }
}
