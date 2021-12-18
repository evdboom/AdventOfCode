using AdventOfCode2021.Constructs.Day18;
using AdventOfCode2021.Services;

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

                var newNumber = new SnailNumber();

                newNumber.Left = number;
                number.IsLeft = true;
                number.Parent = newNumber;
                number.AddDepth();

                var newRight = GetSnailNumber(input[step]);
                newNumber.Right = newRight;
                newRight.Parent = newNumber;
                newRight.IsRight = true;
                newRight.AddDepth();

                number = newNumber;

                number.Reduce();
            }            
            return number.GetMagnitude();
        }        

        protected override long ProcessPartTwo(string[] input)
        {
            return 0;
            //for(int i = 0; i <input.Length; i++)
            //{

            //}
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
