using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day08 : Day
    {
        public Day08(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 8;

        protected override long ProcessPartOne(string[] input)
        {
            var (instructions, nodes) = GetInstructionsAndNodes(input);
            return GetCountToTarget("AAA", "ZZZ", nodes, instructions);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (instructions, nodes) = GetInstructionsAndNodes(input);

            var values = nodes
                .Keys
                .Where(nodeCode => nodeCode.EndsWith('A'))
                .Select(nodeCode => GetCountToTarget(nodeCode, "Z", nodes, instructions))
                .ToList();

            var max = values.Max();
            var step = 1L;
            while (true)
            {
                var checkValue = step * max;
                if (values.All(v => checkValue % v == 0))
                {
                    return checkValue;
                }
                step++;
            }
        }

        private long GetCountToTarget(string nodeCode, string target, Dictionary<string, (string Left, string Right)> nodes, string instructions)
        {
            var count = 0L;
            while (!nodeCode.EndsWith(target))
            {
                var step = count % instructions.Length;
                var instruction = instructions[(int)step];
                nodeCode = instruction == 'L'
                    ? nodes[nodeCode].Left
                    : nodes[nodeCode].Right;
                count++;
            }

            return count;
        }

        private (string Instructions, Dictionary<string, (string Left, string Right)> Nodes) GetInstructionsAndNodes(string[] input)
        {
            var instructions = input[0];
            var nodes = input
                .Skip(2)
                .Select(line =>
                {
                    var parts = line.Split(" = ");
                    var targets = parts[1].Split(", ");
                    return new { Node = parts[0], Target = (Left: targets[0].Replace("(", string.Empty), Right: targets[1].Replace(")", string.Empty)) };
                })
                .ToDictionary(item => item.Node, item => item.Target);

            return (instructions, nodes);
        }
    }
}
