using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day11Group;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day11 : Day
    {        
        public Day11(IFileImporter importer) : base(importer)
        {
     
        }

        public override int DayNumber => 11;
        protected override long ProcessPartOne(string[] input)
        {
            var monkeys = GetMonkeys(input);
            for (int i = 0; i < 20; i++)
            {
                foreach(var monkey in monkeys.Values)
                {
                    while(monkey.Items.TryDequeue(out long item))
                    {
                        monkey.ItemsTested++;
                        item = monkey.WorryFactor(item) / 3;
                        if (item % monkey.DivisionTest == 0)
                        {
                            monkeys[monkey.TargetTrue].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.TargetFalse].Items.Enqueue(item);
                        }
                    }
                }    
            }

            return monkeys.Values
                .Select(monkey => monkey.ItemsTested)
                .OrderDescending()
                .Take(2)
                .Aggregate((a, b) => a * b);            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var monkeys = GetMonkeys(input);
            var factor = monkeys.Values
                .Aggregate(1, (a, b) => a * b.DivisionTest);
            for (int i = 0; i < 10000; i++)
            {
                foreach (var monkey in monkeys.Values)
                {
                    while (monkey.Items.TryDequeue(out long item))
                    {
                        monkey.ItemsTested++;
                        item = monkey.WorryFactor(item) % factor;
                        if (item % monkey.DivisionTest == 0)
                        {
                            monkeys[monkey.TargetTrue].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.TargetFalse].Items.Enqueue(item);
                        }
                    }
                }
            }

            return monkeys.Values
                .Select(monkey => monkey.ItemsTested)
                .OrderDescending()
                .Take(2)
                .Aggregate((a, b) => a * b);
        }

        private Dictionary<int, Monkey> GetMonkeys(string[] input)
        {
            var result = new Dictionary<int, Monkey>();
            int currentMonkey = -1;
            foreach(var line in input)
            {
                if (line.StartsWith("Monkey"))
                {
                    var parts = line.Split(' ');
                    currentMonkey = int.Parse(parts[1].Replace(":", string.Empty));
                    result[currentMonkey] = new Monkey { Number = currentMonkey };
                }
                else if (line.StartsWith("  Starting items"))
                {
                    var parts = line.Split(": ");
                    var items = parts[1].Split(", ").Select(int.Parse);
                    foreach(var item in items)
                    {
                        result[currentMonkey].Items.Enqueue(item);
                    }
                }
                else if (line.StartsWith("  Operation"))
                {
                    var parts = line.Split(" = ");
                    var operation = parts[1];
                    var addition = operation.Split(" + ");
                    if (addition.Length == 2)
                    {
                        if (int.TryParse(addition[1], out int value))
                        {
                            result[currentMonkey].WorryFactor = (i) => i + value;
                        }
                        else
                        {
                            result[currentMonkey].WorryFactor = (i) => i + i;
                        }
                    }
                    else
                    {
                        var multiply = operation.Split(" * ");
                        if (int.TryParse(multiply[1], out int multiplyValue))
                        {
                            result[currentMonkey].WorryFactor = (i) => i * multiplyValue;
                        }
                        else
                        {
                            result[currentMonkey].WorryFactor = (i) => i * i;
                        }
                    }
                }
                else if (line.StartsWith("  Test"))
                {
                    var parts = line.Split(" by ");
                    result[currentMonkey].DivisionTest = int.Parse(parts[1]);
                }
                else if (line.StartsWith("    If true"))
                {
                    var parts = line.Split("monkey ");
                    result[currentMonkey].TargetTrue = int.Parse(parts[1]);
                }
                else if (line.StartsWith("    If false"))
                {
                    var parts = line.Split("monkey ");
                    result[currentMonkey].TargetFalse = int.Parse(parts[1]);
                }
            }
            return result;
        }


    }
}
