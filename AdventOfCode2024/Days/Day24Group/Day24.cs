using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2024.Days.Day24Group;

namespace AdventOfCode2024.Days
{
    public class Day24(IFileImporter importer, IScreenWriter writer) : Day(importer)
    {
        private readonly IScreenWriter _writer = writer;

        public override int DayNumber => 24;

        protected override long ProcessPartOne(string[] input)
        {
            var (wires, gates) = GetWiresAndGates(input);
            CalculateOutputs(wires, gates);
            return GetValue(wires, 'z');
        }

        protected override long ProcessPartTwo(string[] input)
        {
            if (input.Length == 0)
            {
                return -1;
            }

            var (wires, gates) = GetWiresAndGates(input);

            var count = wires.Count / 2; // (x and y inputs);

            var start = gates.First(gate =>
                gate.IsInput("x00") &&
                gate.IsInput("y00") &&
                gate.Type == GateType.Xor);
            var swaps = new HashSet<string>();
            if (start.Output != "z00")
            {
                swaps.Add(start.Output);
            }

            var carry = gates.First(gate =>
                gate.IsInput("x00") &&
                gate.IsInput("y00") &&
                gate.Type == GateType.And)
                .Output;

            for (int i = 1; i < count; i++)
            {
                var x = $"x{i:D2}";
                var y = $"y{i:D2}";
                var z = $"z{i:D2}";

                var baseXor = gates.First(gate =>
                    gate.IsInput(x) &&
                    gate.IsInput(y) &&
                    gate.Type == GateType.Xor)
                    .Output;

                var xor = gates
                    .First(gate =>
                    (gate.IsInput(baseXor) ||
                    gate.IsInput(carry)) &&
                    gate.Type == GateType.Xor);

                if (xor.Output != z)
                {
                    swaps.Add(xor.Output);
                    swaps.Add(z);
                }
                if (!xor.IsInput(baseXor))
                {
                    swaps.Add(baseXor);
                }
                if (!xor.IsInput(carry))
                {
                    swaps.Add(carry);
                }

                var baseCarry = gates.First(gate =>
                    gate.IsInput(x) &&
                    gate.IsInput(y) &&
                    gate.Type == GateType.And)
                    .Output;

                var cascadeCarry = gates.First(gate =>
                    (gate.IsInput(baseXor) ||
                    gate.IsInput(carry)) &&
                    gate.Type == GateType.And);

                if (!cascadeCarry.IsInput(baseXor))
                {
                    swaps.Add(baseXor);
                }
                if (!cascadeCarry.IsInput(carry))
                {
                    swaps.Add(carry);
                }

                var carryGate = gates.First(gate =>
                    (gate.IsInput(cascadeCarry.Output) ||
                    gate.IsInput(baseCarry)) &&
                    gate.Type == GateType.Or);

                if (!carryGate.IsInput(cascadeCarry.Output))
                {
                    swaps.Add(cascadeCarry.Output);
                }
                if (!carryGate.IsInput(baseCarry))
                {
                    swaps.Add(baseCarry);
                }

                carry = carryGate.Output;
            }

            _writer.WriteLine(string.Join(',', swaps.Order()));

            return -1;
        }

        private void CalculateOutputs(Dictionary<string, bool> wires, List<LogicGate> gates)
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var gate in gates)
                {
                    if (gate.TryCalculateOutput(wires, out var value))
                    {
                        if (wires.TryAdd(gate.Output, value.Value))
                        {
                            changed = true;
                        }
                    }
                }
            }
        }

        private long GetValue(Dictionary<string, bool> wires, char wanted)
        {
            var withChar = wires
                .Where(wire => wire.Key[0] == wanted)
                .OrderBy(wire => wire.Key)
                .ToList();

            long result = 0;
            for (int i = 0; i < withChar.Count; i++)
            {
                if (withChar[i].Value)
                {
                    result += 1L << i;
                }
            }

            return result;
        }

        private (Dictionary<string, bool> Wires, List<LogicGate> Gates) GetWiresAndGates(string[] input)
        {
            var gates_started = false;
            var wires = new Dictionary<string, bool>();
            var gates = new List<LogicGate>();

            foreach (var line in input)
            {
                if (line == string.Empty)
                {
                    gates_started = true;
                    continue;
                }
                if (!gates_started)
                {
                    var parts = line.Split(": ");
                    wires[parts[0]] = parts[1] == "1";
                }
                else
                {
                    gates.Add(LogicGate.Parse(line));
                }
            }

            return (wires, gates);
        }
    }
}
