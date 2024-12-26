namespace AdventOfCode2024.Days.Day24Group
{
    public static class GateTypeExtensions
    {
        public static GateType ParseGateType(this string source)
        {
            return source switch
            {
                "AND" => GateType.And,
                "OR" => GateType.Or,
                "XOR" => GateType.Xor,
                _ => throw new ArgumentException($"Unknown gate type: {source}")
            };
        }
    }
}
