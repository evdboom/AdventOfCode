using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day10 : Day
    {
        private readonly IScreenWriter _writer;

        public Day10(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 10;
        protected override long ProcessPartOne(string[] input)
        {
            var wantedCycles = new[] { 20, 60, 100, 140, 180, 220 };
            var x = 1;
            var cycle = 0;
            var value = 0;

            foreach(var line in input)
            {                
                switch(line) 
                {
                    case "noop":
                        cycle++;
                        if (wantedCycles.Contains(cycle))
                        {
                            value += x * cycle;
                        }
                        break;
                    default:
                        var parts = line.Split(' ');
                        var addx = int.Parse(parts[1]);
                        cycle++;
                        if (wantedCycles.Contains(cycle))
                        {
                            value += x * cycle;
                        }
                        cycle++;
                        if (wantedCycles.Contains(cycle))
                        {
                            value += x * cycle;
                        }
                        x += addx;
                        break;
                }
            }

            return value;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var x = 1;
            var cycle = 0;
            

            foreach (var line in input)
            {
                switch (line)
                {
                    case "noop":
                        Write(cycle, x);
                        cycle++;                        
                        break;
                    default:
                        var parts = line.Split(' ');
                        var addx = int.Parse(parts[1]);
                        Write(cycle, x);
                        cycle++;
                        Write(cycle, x);
                        cycle++;                        
                        x += addx;
                        break;
                }
            }
            _writer.NewLine();
            return -1;
        }

        private void Write(int cycle, int x)
        {
            if (cycle % 40 == 0)
            {
                _writer.NewLine();
            }

            if (Math.Abs((cycle % 40) - x) <= 1)
            {
                _writer.Write("#");
            }
            else
            {
                _writer.Write(".");
            }
        }
    }
}
