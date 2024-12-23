using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2024.Days.Day17Group;

namespace AdventOfCode2024.Days
{
    public class Day17(IFileImporter importer, IScreenWriter writer) : Day(importer)
    {
        private readonly IScreenWriter _writer = writer;
        private string? _resultOne;

        public override int DayNumber => 17;
        public string? PartOneResult() => _resultOne;

        protected override long ProcessPartOne(string[] input)
        {
            var program = GetProgram(input);        
            _resultOne = string.Join(",", RunProgram(program));
            _writer.WriteLine(_resultOne);
            return -1;
        }


        protected override long ProcessPartTwo(string[] input)
        {
            var program = GetProgram(input);
            var factor = program.Instructions
                .Select((instruction, index) => (Instruction: instruction, Index: index))
                .Where(value => value.Instruction == 0 && value.Index % 2 == 0)
                .Aggregate(1, (acc, value) =>
                {
                    if (program.Instructions[value.Index + 1] > 3)
                    {
                        throw new InvalidOperationException("Invalid instruction for automatic factor determination");
                    }
                    return acc * program.Instructions[value.Index + 1];
                });
            var result = new List<int>
            {
                0
            };

            for (int i = program.Instructions.Count - 1; i >= 0; i--)
            {
                var intermediateResult = new List<int>();
                for (int j = 0; j < result.Count; j++)
                {
                    for (int k = 0; k < factor; k++)
                    {
                        if (k == 0 && i == program.Instructions.Count - 1)
                        {
                            continue;
                        }

                        var operatorA = factor * result[j] + k;

                        program = GetProgram(input);
                        program.RegisterA = operatorA;

                        var value = RunProgram(program).First();
                        
                        if (value == program.Instructions[i])
                        {
                            intermediateResult.Add(k);
                        }
                    }
                }

                result = intermediateResult;
            }

            return result.Min();
        }

        private IEnumerable<int> RunProgram(ProgramRegister program)
        {
            for (int i = 0; i < program.Instructions.Count; i += 2)
            {
                var operation = (OpCode: program.Instructions[i], Operand: program.Instructions[i + 1]);
                var operationResult = PerfomOperation(operation, program, out var value);
                if (operationResult == OperationResult.Jump)
                {
                    i = value!.Value - 2;
                }
                else if (operationResult == OperationResult.Value)
                {
                    yield return value!.Value;
                }
            }
        }

        private ProgramRegister GetProgram(string[] input)
        {
            var program = new ProgramRegister
            {
                RegisterA = long.Parse(input[0]["Register A: ".Length..]),
                RegisterB = long.Parse(input[1]["Register B: ".Length..]),
                RegisterC = long.Parse(input[2]["Register C: ".Length..]),
            };
            program.Instructions.AddRange(input[4]["Program: ".Length..]
                .Split(',')
                .Select(int.Parse));
            return program;
        }

        private long GetCombo(int operand, ProgramRegister program)
        {
            return operand switch
            {
                0 or 1 or 2 or 3 => operand,
                4 => program.RegisterA,
                5 => program.RegisterB,
                6 => program.RegisterC,
                _ => throw new ArgumentException("Invalid operand", nameof(operand)),
            };
        }

        private OperationResult PerfomOperation((int OpCode, int Operand) operation, ProgramRegister program, out int? value)
        {
            value = null;
            var result = OperationResult.None;
            switch (operation.OpCode)
            {
                case 0:
                    program.RegisterA = program.RegisterA / (long)Math.Pow(2, GetCombo(operation.Operand, program));
                    break;
                case 1:
                    program.RegisterB = program.RegisterB ^ operation.Operand;
                    break;
                case 2:
                    program.RegisterB = GetCombo(operation.Operand, program) % 8;
                    break;
                case 3:
                    if (program.RegisterA != 0)
                    {
                        result = OperationResult.Jump;
                        value = operation.Operand;
                    }
                    break;
                case 4:
                    program.RegisterB = program.RegisterB ^ program.RegisterC;
                    break;
                case 5:
                    result = OperationResult.Value;
                    value = (int)GetCombo(operation.Operand, program) % 8;
                    break;
                case 6:
                    program.RegisterB = program.RegisterA / (long)Math.Pow(2, GetCombo(operation.Operand, program));
                    break;
                case 7:
                    program.RegisterC = program.RegisterA / (long)Math.Pow(2, GetCombo(operation.Operand, program));
                    break;
            };

            return result;
        }
    }

}
