namespace AdventOfCode2023.Days.Day19Group
{
    public record WorkFlow
    {
        public string Name { get; set; } = string.Empty;
        public List<WorkFlowRule> Rules { get; set; } = [];
        public string DefaultResult { get; set; } = string.Empty;
    }
}
