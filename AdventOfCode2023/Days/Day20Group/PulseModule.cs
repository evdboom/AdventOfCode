namespace AdventOfCode2023.Days.Day20Group
{
    public abstract class PulseModule
    {        
        public string Name { get; set; } = string.Empty;
        public string[] Targets { get; set; } = [];
        public long LowPulsesSent { get; set; }
        public long HighPulsesSent { get; set; }
        public abstract IEnumerable<(string Target, bool HighPulse, string Source)> ProcessPulse(string source, bool highPulse);
    }
}
