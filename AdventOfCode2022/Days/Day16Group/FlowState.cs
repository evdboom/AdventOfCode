namespace AdventOfCode2022.Days.Day16Group
{
    public record FlowState
    {
        public string[] ClosedValves { get; set; } = Array.Empty<string>();
        public Dictionary<int, FlowActor> Actors { get; set; } = new();
        public long Flow { get; set; }
        public int TimeRemaining { get; set; }
    }
}
