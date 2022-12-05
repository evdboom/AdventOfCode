using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day05 : Day
    {
        private readonly IScreenWriter _writer;

        private string? _resultOne;
        private string? _resultTwo;

        public Day05(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 5;

        public string? PartOneResult() => _resultOne;
        public string? PartTwoResult() => _resultTwo;

        protected override long ProcessPartOne(string[] input)
        {
            var stacks = GetStacks(input, out int procedureStart);
            var procedues = GetProcesdures(input, procedureStart);

            foreach (var procedure in procedues)
            {
                for (int i = 0; i < procedure[0]; i++)
                {
                    stacks[procedure[2]].Push(stacks[procedure[1]].Pop());
                }
            }

            _resultOne = string.Join("", stacks
                .Select(stack => stack.Value.Peek()));

            _writer.WriteLine($"Stacks for day 5 part one: {_resultOne}");            

            return -1;
        }

        protected override long ProcessPartTwo(string[] input)
        { 
            var stacks = GetStacks(input, out int procedureStart);
            var procedues = GetProcesdures(input, procedureStart);

            foreach (var procedure in procedues)
            {
                var craneStack = new Stack<char>();
                for (int i = 0; i < procedure[0]; i++)
                {
                    craneStack.Push(stacks[procedure[1]].Pop());
                }
                while (craneStack.TryPop(out char crate))
                {
                    stacks[procedure[2]].Push(crate);
                }
            }

            _resultTwo = string.Join("", stacks
                .Select(stack => stack.Value.Peek()));
            _writer.WriteLine($"Stacks for day 5 part two: {_resultTwo}");

            return -1;
        }

        private Dictionary<int, Stack<char>> GetStacks(string[] input, out int procedureStart)
        {
            var strings = new Stack<string>();
            procedureStart = 0;
            foreach (var item in input)
            {
                procedureStart++;
                if (string.IsNullOrEmpty(item))
                {
                    return ProcessStacks(strings);
                }
                else
                {
                    strings.Push(item);
                }
            }

            throw new InvalidOperationException("No empty line found!");
        }

        private IEnumerable<int[]> GetProcesdures(string[] input, int procedureStart)
        {
            return input
                .Where((line, index) => index >= procedureStart)
                .Select(line => line
                    .Split(new[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .Select(int.Parse)
                    .ToArray());
        }

        private Dictionary<int, Stack<char>> ProcessStacks(Stack<string> strings)
        {
            var first = strings.Pop();
            var stacks = first
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(stack => new { Number = int.Parse(stack), Location = first.IndexOf(stack) });

            var result = stacks
                .ToDictionary(stack => stack.Number, stack => new Stack<char>());
            while (strings.TryPop(out string? line))
            {
                foreach (var stack in stacks)
                {
                    var crate = line[stack.Location];
                    if (crate != ' ')
                    {
                        result[stack.Number].Push(crate);
                    }
                }
            }

            return result;
        }
    }
}
