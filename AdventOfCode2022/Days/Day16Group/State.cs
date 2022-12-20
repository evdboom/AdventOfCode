namespace AdventOfCode2022.Days.Day16Group
{
    public record State
    {
        public long Flow { get; set; }
        public long FlowRate { get; set; }
        public int TimeRemaining { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Previous { get; set; } = string.Empty;
        public string Elephant { get; set; } = string.Empty;
        public string PreviousElephant { get; set; } = string.Empty;
        public string OpenValves = string.Empty;

    }
}
