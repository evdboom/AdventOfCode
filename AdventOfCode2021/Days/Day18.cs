using AdventOfCode2021.Constructs.Day18;
using AdventOfCode2021.Services;
using System.Drawing;

namespace AdventOfCode2021.Days
{
    public class Day18 : Day
    {
        private const char Open = '[';
        private const char Close = ']';
        private const char Split = ',';

        private readonly IScreenWriter _writer;

        public Day18(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
            _writer.Disable();
        }

        public override int DayNumber => 18;

        protected override long ProcessPartOne(string[] input)
        {
            var numbers = input
                .Select(i => GetSnailNumber(i))
                .ToArray();
            var number = numbers[0];            
            for (int step = 1; step < input.Length; step++)
            {                
                number = GetSum(number, numbers[step]);                
            }            
            return number.GetMagnitude();
        }        

        protected override long ProcessPartTwo(string[] input)
        {
            long max = 0;
            for (int step = 0; step < input.Length; step++)
            {                
                for (int i = 0; i < input.Length; i++)
                {
                    if (i == step)
                    {
                        continue;
                    }

                    var first = GetSnailNumber(input[step]);
                    var second = GetSnailNumber(input[i]);
                    var magnitude = GetSum(first, second).GetMagnitude();                    

                    max = Math.Max(max, magnitude);
                }
            }
            
            return max;
        }

        private SnailNumber GetSum(SnailNumber first, SnailNumber second)
        {
            var number = new SnailNumber
            {
                Left = first,
                Right = second
            };

            number.Reduce(_writer);

            return number;
        }

        private SnailNumber GetSnailNumber(string line)
        {
            SnailNumber? result = null;
            SnailNumber? current = null;
            var left = true;
            string currentValue = string.Empty;
            foreach(char c in line)
            {
                if (c == Open)
                {
                    if (result == null)
                    {
                        result = new SnailNumber();
                        current = result;
                    }
                    else
                    {
                        var newNumber = new SnailNumber
                        {
                            Parent = current,
                            IsLeft = left,
                            IsRight = !left,
                            Depth = current!.Depth + 1
                        };

                        if (left)
                        {
                            current.Left = newNumber;
                        }
                        else
                        {
                            current.Right = newNumber;
                        }

                        left = true;
                        current = newNumber;
                    }
                }
                else if (c == Close)
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        if (left)
                        {
                            current!.LeftValue = long.Parse(currentValue);
                        }
                        else
                        {
                            current!.RightValue = long.Parse(currentValue);
                        }
                    }

                    currentValue = string.Empty;
                    current = current?.Parent;                    
                }
                else if (c == Split)
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        if (left)
                        {
                            current!.LeftValue = long.Parse(currentValue);
                        }
                        else
                        {
                            current!.RightValue = long.Parse(currentValue);
                        }
                    }

                    currentValue = string.Empty;

                    left = false;
                }
                else
                {
                    currentValue += c;
                }
            }

            return result!;
        }
    }
}
