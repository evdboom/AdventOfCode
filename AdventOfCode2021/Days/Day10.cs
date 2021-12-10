using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day10 : Day
    {
        private const char Open1 = '(';
        private const char Close1 = ')';
        private const char Open2 = '[';
        private const char Close2 = ']';
        private const char Open3 = '{';
        private const char Close3 = '}';
        private const char Open4 = '<';
        private const char Close4 = '>';

        private const int CompleteMultiplier = 5;

        private readonly char[] _openCharacters = new[]
        {
            Open1, Open2, Open3, Open4
        };

        private readonly Dictionary<char,char> _closeCharacters = new Dictionary<char, char>
        {
            { Open1, Close1 },
            { Open2, Close2 },
            { Open3, Close3 },
            { Open4, Close4 }
        };

        private readonly Dictionary<char, int> _corruptValues = new Dictionary<char, int>
        {
            { Close1, 3 },
            { Close2, 57 },
            { Close3, 1197 },
            { Close4, 25137 }
        };

        private readonly Dictionary<char, int> _incompleteValues = new Dictionary<char, int>
        {
            { Close1, 1 },
            { Close2, 2 },
            { Close3, 3 },
            { Close4, 4 }
        };

        public override int DayNumber => 10;

        public Day10(IFileImporter importer) : base(importer)
        {
        }        

        protected override long ProcessPartOne(string[] input)
        {
            List<char> illegal = new();
            foreach (var line in input)
            {
                if (IsCorrupted(line, out char corrupt))
                {
                    illegal.Add(corrupt);
                }
            }

            return illegal
                .Select(i => _corruptValues[i])
                .Sum();

        }

        protected override long ProcessPartTwo(string[] input)
        {
            var values = input
                .Where(i => !IsCorrupted(i, out _))
                .Select(i => GetCompleteValue(i))
                .OrderBy(i => i)
                .ToList();

            return values[values.Count / 2];
        }

        private long GetCompleteValue(string line)
        {
            long sum = 0;
            Stack<char> openChunks = new();
            foreach (var chunkPart in line)
            {
                if (_openCharacters.Contains(chunkPart))
                {
                    openChunks.Push(chunkPart);
                }
                else
                {
                    var open = openChunks.Pop();
                    if (chunkPart != _closeCharacters[open])
                    {
                        throw new InvalidOperationException("Forgot to remove corrupted?");
                    }
                }
            }
            
            while(openChunks.TryPop(out char open))
            {
                sum *= CompleteMultiplier;
                sum += _incompleteValues[_closeCharacters[open]];
            }

            return sum;
        }

        private bool IsCorrupted(string line, out char corrupt)
        {
            Stack<char> openChunks = new();
            foreach (var chunkPart in line)
            {
                if (_openCharacters.Contains(chunkPart))
                {
                    openChunks.Push(chunkPart);
                }
                else
                {
                    var open = openChunks.Pop();
                    if (chunkPart != _closeCharacters[open])
                    {
                        corrupt = chunkPart;
                        return true;
                    }
                }
            }
            corrupt = default;
            return false;
        }
    }
}
