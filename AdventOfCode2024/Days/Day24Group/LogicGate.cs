using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024.Days.Day24Group
{
    public class LogicGate
    {
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public GateType Type { get; set; }

        public bool TryCalculateOutput(Dictionary<string, bool> wires, [NotNullWhen(true)] out bool? result)
        {
            if (!wires.TryGetValue(Input1, out var input1) || !wires.TryGetValue(Input2, out var input2))
            {
                result = null;
                return false;
            }
            
            result = Type switch
            {
                GateType.And => input1 & input2,
                GateType.Or => input1 | input2,
                GateType.Xor => input1 ^ input2,
                _ => throw new ArgumentException($"Unknown gate type: {Type}")
            };
            return true;
        }

        public bool IsInput(string wire) => Input1 == wire || Input2 == wire;

        public static LogicGate Parse(string source)
        {
            var parts = source.Split(" ");
            var gate = new LogicGate
            {
                Input1 = parts[0],
                Type = parts[1].ParseGateType(),
                Input2 = parts[2],
                Output = parts[^1]
            };

            return gate;
        }
    }
}
