namespace AdventOfCode2022.Days.Day11Group
{
    public class Monkey
    {
        public int Number { get; init; }
        public Queue<long> Items { get; } = new();
        public Func<long, long> WorryFactor { get; set; } = (i) => i;
        public int DivisionTest { get; set; }
        public int TargetTrue { get; set; }
        public int TargetFalse { get; set; }
        public long ItemsTested { get; set; }
    }
}
