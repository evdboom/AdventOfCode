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

        public Day18(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 18;

        protected override long ProcessPartOne(string[] input)
        {
            var number = GetSnailNumber(input[0]);            
            for (int step = 1; step < input.Length; step++)
            {
                var newRight = GetSnailNumber(input[step]);

                number = ProcessNumber(number, newRight);                
            }            
            return number.GetMagnitude();
        }        

        protected override long ProcessPartTwo(string[] input)
        {
            long max = 0;
            Dictionary<Point, long> values = new();
            for (int step = 0; step < input.Length; step++)
            {
                var first = GetSnailNumber(input[step]);
                for (int i = 0; i < input.Length; i++)
                {
                    if (i == step)
                    {
                        continue;
                    }
                    var second = GetSnailNumber(input[i]);
                    var magnitude = ProcessNumber(first, second).GetMagnitude();

                    values.Add(new Point(step, i), magnitude);

                    max = Math.Max(max, magnitude);
                }
            }
            var v2 = values
                .OrderByDescending(d => d.Value)
                .ToDictionary(d => d.Key, d => d.Value);

            return max;
        }

        private SnailNumber ProcessNumber(SnailNumber first, SnailNumber second)
        {
            var newNumber = new SnailNumber
            {
                Left = first,
                Right = second
            };

            first.IsLeft = true;
            first.Parent = newNumber;
            first.AddDepth();

            second.Parent = newNumber;
            second.IsRight = true;
            second.AddDepth();

            newNumber.Reduce();

            return newNumber;
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
