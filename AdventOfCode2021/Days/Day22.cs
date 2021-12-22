using AdventOfCode2021.Constructs.Day22;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day22 : Day
    {
        private const string On = "on";
        private const int MinArea = -50;
        private const int MaxArea = 50;

        public Day22(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 22;

        protected override long ProcessPartOne(string[] input)
        {
            var instructions = GetInstructions(input);

            var total = new Instruction
            {
                Xfrom = MinArea,
                Xto = MaxArea,
                Yfrom = MinArea,
                Yto = MaxArea,
                Zfrom = MinArea,
                Zto = MaxArea,
            };

            return GetTotalOn(instructions, total, instructions.Count - 1);

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var instructions = GetInstructions(input);

            var minX = instructions.Min(i => i.Xfrom);
            var maxX = instructions.Max(i => i.Xto);
            var minY = instructions.Min(i => i.Yfrom);
            var maxY = instructions.Max(i => i.Yto);
            var minZ = instructions.Min(i => i.Zfrom);
            var maxZ = instructions.Max(i => i.Zto);

            var total = new Instruction
            {
                Xfrom = minX,
                Xto = maxX,
                Yfrom = minY,
                Yto = maxY,
                Zfrom = minZ,
                Zto = maxZ,
            };

            return GetTotalOn(instructions, total, instructions.Count - 1);

        }

        private long GetTotalOn(List<Instruction> instructions, Instruction total, int index)
        {
            if (index < 0 || !total.Valid)
            {
                return 0;
            }
            var instruction = instructions[index];
            var intersection = total.Intersect(instruction);
            var onFromPrevious = GetTotalOn(instructions, total, index - 1);
            var onInIntersection = GetTotalOn(instructions, intersection, index - 1);
            var onInTotal = onFromPrevious - onInIntersection;

            return instruction.On ? onInTotal + intersection.Volume : onInTotal;
        }

        private List<Instruction> GetInstructions(string[] input)
        {
            return input
                .Select(line => line.Split(' '))
                .Select(l => (on: l[0] == On, coords: l[1]
                    .Split(',')
                    .SelectMany(c => c.Substring(2)
                        .Split("..")
                        .Select(c => int.Parse(c)))
                    .ToArray()))
                .Select(r => new Instruction
                {
                    On = r.on,
                    Xfrom = r.coords[0],
                    Xto = r.coords[1],
                    Yfrom = r.coords[2],
                    Yto = r.coords[3],
                    Zfrom = r.coords[4],
                    Zto = r.coords[5],
                })
                .ToList();
        }
    }
}
