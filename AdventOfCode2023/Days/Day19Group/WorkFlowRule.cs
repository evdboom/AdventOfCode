namespace AdventOfCode2023.Days.Day19Group
{
    public record WorkFlowRule
    {
        public char RatingType { get; set; }
        public int Value { get; set; }
        public string Result { get; set; } = string.Empty;
        public char Operator { get; set; }
    }
}
