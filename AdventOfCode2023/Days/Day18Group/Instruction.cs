namespace AdventOfCode2023.Days.Day18Group
{
    public record Instruction
    {
        public char Direction { get; set; }
        public int Length { get; set; }
        public string ColorCode { get; set; } = string.Empty;
    }
}
