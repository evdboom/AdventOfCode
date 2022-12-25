using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{    
    public class Day25 : Day
    {
        private IScreenWriter _writer;
        public string BobCommand { get; set; } = string.Empty;
        public Day25(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }
        public override int DayNumber => 25;

        protected override long ProcessPartOne(string[] input)
        {
            var normal = input.Select(TranslateToNormal).Sum();            
            var snafu = TranslateToSnafu(normal);
            BobCommand = snafu;
            _writer.WriteLine(snafu);
            return normal;
        }

        private string TranslateToSnafu(long value)
        {
            var found = false;
            var pow = 0;
            var calc = Math.Abs(value);
            while (!found)
            {
                var max = Math.Pow(5, pow) * 2;
                if (calc - max <= 0)
                {
                    found = true;
                }
                else
                {
                    pow++; 
                }
            }

            var result = string.Empty;
            for (int i = pow; i > 0; i--)
            {
                var factor = (long)Math.Pow(5, i);
                var baseValue = calc / factor;
                var lower = GetMaxLower(i);
                var dif = calc - baseValue * factor;
                if (Math.Abs(dif) > lower)
                {
                    if (baseValue > 0 || dif > 0)
                    {
                        baseValue++;
                    }
                    else
                    {
                        baseValue--;
                    }
                }
                switch (baseValue)
                {
                    case 2:
                        result += '2';
                        break;
                    case 1:
                        result += '1';
                        break;
                    case 0:
                        result += '0';
                        break;
                    case -1:
                        result += '-';
                        break;
                    case -2:
                        result += '=';
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown value {baseValue}");
                       
                }
                calc = calc - baseValue * factor;
            }
            switch (calc)
            {
                case 2:
                    result += '2';
                    break;
                case 1:
                    result += '1';
                    break;
                case 0:
                    result += '0';
                    break;
                case -1:
                    result += '-';
                    break;
                case -2:
                    result += '=';
                    break;
                default:
                    throw new InvalidOperationException($"Unknown value {calc}");
            }

            if (value < 0)
            {
                var newResult = string.Empty;
                var mapping = new Dictionary<char, char>
                {
                    { '2', '=' },
                    { '1', '-' },
                    { '0', '0' },
                    { '-', '1' },
                    { '=', '2' }
                };
                foreach(var c in result)
                {
                    newResult += mapping[c];
                }
                result = newResult;
            }

            return result;
        }

        private long GetMaxLower(int i)
        {
            var result = 0L;
            for (int j = i - 1; j >= 0; j--)
            {
                result += (long)Math.Pow(5, j) * 2;
            }
            return result;
        }

        private long TranslateToNormal(string input)
        {
            long sum = 0;
            for (int i = input.Length - 1; i >= 0; i --)
            {
                var factor = (long)Math.Pow(5, input.Length - 1 - i);
                sum += input[i] switch
                {
                    '2' => factor * 2,
                    '1' => factor,
                    '0' => 0,
                    '-' => factor * -1,
                    '=' => factor * -2,
                    _ => throw new InvalidOperationException("Unknown value")
                };
            }
            return sum;

        }

        protected override long ProcessPartTwo(string[] input)
        {
            return -1;
        }
    }
}