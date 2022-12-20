namespace AdventOfCode2022.Days.Day19Group
{
    public record Blueprint
    {
        public int Number { get; set; }
        public int OreBotCosts { get; set; }
        public int ClayBotCosts { get; set; }
        public (int Ore, int Clay) ObsidianBotCosts { get; set; }
        public (int Ore, int Obsidian) GeodeBotCosts { get; set; }
    }
}
