using AdventOfCode2021.Constructs.Day24;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day24 : Day
    {
        private const int Xindex = 6;
        private const int Yindex = 16;

        public Day24(IFileImporter importer) : base(importer)
        {
        }
        
        public override int DayNumber => 24;

        protected override long ProcessPartOne(string[] input)
        {        
            var instructions = GetInstructions(input);
            ProcessInstructions(instructions);           
            return long.Parse(string.Join("", instructions.Select(i => i.High)));
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var instructions = GetInstructions(input);
            ProcessInstructions(instructions);
            return long.Parse(string.Join("", instructions.Select(i => i.Low)));
        }

        private void ProcessInstructions(List<InstructionGroup> instructions)
        {
            foreach (var instruction in instructions.Where(i => i.AddToX <= 0))
            {
                InstructionGroup previous = instruction.PreviousGroup!;
                while (previous!.Processed)
                {
                    previous = previous.PreviousGroup!;
                }

                var factor = previous.AddToY + instruction.AddToX;
                instruction.High = Math.Min(9, 9 + factor);
                instruction.Low = Math.Max(1, 1 + factor);
                previous.High = Math.Min(9, 9 - factor);
                previous.Low = Math.Max(1, 1 - factor);

                instruction.Processed = true;
                previous.Processed = true;
            }
        }


        private List<InstructionGroup> GetInstructions(string[] input)
        {
            var all = input
                .Select(line => line.Split(' '))
                .ToArray();
            List<InstructionGroup> result = new();
            InstructionGroup? current = null;
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i][0] == "inp")
                {
                    var newCurrent = new InstructionGroup
                    {
                        PreviousGroup = current,
                        Index = result.Count
                    };

                    current = newCurrent;
                    
                    result.Add(current);
                    current.Instructions.Add(all[i]);
                }
                else
                {
                    current!.Instructions.Add(all[i]);
                }

                if (current!.Instructions.Count == Xindex)
                {
                    current!.AddToX = int.Parse(all[i][2]);
                }
                else if (current.Instructions.Count == Yindex)
                {
                    current!.AddToY = int.Parse(all[i][2]);
                }
            }

            return result;
        }
    }
}
