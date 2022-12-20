namespace AdventOfCode2022.Days.Day16Group
{
    public record Valve
    {
        public string Name { get; set; } = string.Empty;
        public int FlowRate { get; set; }
        public string Connections { get; set; } = string.Empty;
    }
}
