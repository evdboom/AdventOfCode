namespace AdventOfCode2021.Constructs.Day24
{
    public class InstructionGroup
    {
        public List<string[]> Instructions { get; }
        public InstructionGroup? PreviousGroup { get; set; }

        public int Index { get; set; }
        public bool Processed { get; set; }

        public int AddToX { get; set; }
        public int AddToY { get; set; }

        public int High { get; set; }
        public int Low { get; set; }

        public InstructionGroup()
        {
            Instructions = new();
        }

        public long Run(int input, long z)
        {
            long w = 0;
            long x = 0;
            long y = 0;

            foreach (var instruction in Instructions)
            {
                var variable = instruction[1][0] switch
                {
                    'w' => w,
                    'x' => x,
                    'y' => y,
                    'z' => z,
                    _ => throw new InvalidOperationException("unknown variable")
                };
                long target;
                if (instruction[0] == "inp")
                {
                    target = input;
                }
                else
                {
                    target = instruction[2] switch
                    {
                        "w" => w,
                        "x" => x,
                        "y" => y,
                        "z" => z,
                        _ => int.Parse(instruction[2])
                    };
                }
                var result = Instruction(instruction[0], variable, target);
                switch (instruction[1][0])
                {
                    case 'w':
                        w = result;
                        break;
                    case 'x':
                        x = result;
                        break;
                    case 'y':
                        y = result;
                        break;
                    case 'z':
                        z = result;
                        break;
                }
            }

            return z;
        }

        private long Instruction(string instruction, long a, long b)
        {
            return instruction switch
            {
                "inp" => b,
                "add" => a + b,
                "mul" => a * b,
                "div" => a / b,
                "mod" => a % b,
                "eql" => a == b ? 1 : 0,
                _ => throw new InvalidOperationException($"Unknown operation: {instruction}")
            };
        }

        public override string ToString()
        {
            return $"{Index} {AddToX} {AddToY} {High} {Low}";
        }
    }
}
