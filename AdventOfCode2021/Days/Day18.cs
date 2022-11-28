using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Constructs.Day18;

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
            foreach (var step in Enumerable.Range(1, input.Length - 1))
            {
                number = GetSum(number, numbers[step]);
            }
            return number.GetMagnitude();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            long max = 0;
            var range = Enumerable.Range(0, input.Length);
            foreach (var i in range)
            {
                foreach (var j in range.Where(j => i != j))
                {
                    var first = GetSnailNumber(input[i]);
                    var second = GetSnailNumber(input[j]);
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
            foreach (char c in line)
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
                        if (left)
                        {
                            current!.Left = new SnailNumber();
                            current = current.Left;
                        }
                        else
                        {
                            current!.Right = new SnailNumber();
                            current = current.Right;
                        }

                        left = true;
                    }
                }
                else if (c == Close)
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        current!.RightValue = long.Parse(currentValue);
                    }

                    currentValue = string.Empty;
                    current = current?.Parent;
                }
                else if (c == Split)
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        current!.LeftValue = long.Parse(currentValue);
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
