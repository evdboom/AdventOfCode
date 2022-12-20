namespace AdventOfCode2022.Days.Day19Group
{
    public record State
    {
        public int TimeRemaining { get; set; }
        public int OreBots { get; set; }
        public int ClayBots { get; set; }
        public int ObsidianBots { get; set; }
        public int GeodeBots { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geodes { get; set; }        
    }
}
