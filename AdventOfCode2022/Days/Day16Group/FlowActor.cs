namespace AdventOfCode2022.Days.Day16Group
{
    public record FlowActor
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public int ETA { get; set; }
    }
}
