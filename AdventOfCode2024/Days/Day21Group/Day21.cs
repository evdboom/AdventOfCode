using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2024.Days
{
    public class Day21(IFileImporter importer) : Day(importer)
    {
        private static readonly string[] NumericKeypad = ["789", "456", "123", " 0A"];
        private static readonly string[] DirectionalKeypad = [" ^A","<v>"];

        public override int DayNumber => 21;

        protected override long ProcessPartOne(string[] input)
        {
            return PerformKeypadPresses(input, 2);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return PerformKeypadPresses(input, 25);
        }

        private long PerformKeypadPresses(string[] input, int chainLength)
        {
            var numeric = NumericKeypad
                .ToGrid()
                .ToDictionary(c => c.Value, c => c.Point);
            var directional = DirectionalKeypad
                .ToGrid()
                .ToDictionary(c => c.Value, c => c.Point);

            var numericStart = new Point(2, 3);            
            var directionalStart = new Point(2, 0);
            var cache = new Dictionary<(string, int), long>();
            var result = 0L;
            foreach (var numericCode in input)
            {
                var length = 0L;
                var codeValue = long.Parse(numericCode[..^1]);
                var codes = GetPaths(numericCode, numeric, numericStart).ToList();

                foreach (var code in codes)
                {
                    length += GetCodeLength(code, directional, directionalStart, cache, chainLength);
                }

                result += length * codeValue;
            }

            return result;
        }

        private static long GetCodeLength(string code, Dictionary<char, Point> keypad, Point start, Dictionary<(string, int), long> cache, int chainLength)
        {
            if (chainLength == 0)
            {
                return code.Length;
            }
            else if (cache.TryGetValue((code, chainLength), out var value))
            {
                return value;
            }

            var length = GetPaths(code, keypad, start)
                .Aggregate(0L, (acc, path) =>
                {
                    return acc + GetCodeLength(path, keypad, start, cache, chainLength - 1);
                });
            cache[(code, chainLength)] = length;
            return length;
        }

        private static IEnumerable<string> GetPaths(string code, Dictionary<char, Point> keypad, Point start)
        {
            var current = start;
            foreach (var direction in code)
            {
                var result = string.Empty;
                var target = keypad[direction];
                var illegal = keypad[' '];
                if ((illegal.X != target.X || illegal.Y != current.Y) && target.X < current.X)
                {
                    for (int i = 0; i < current.X - target.X; i++)
                    {
                        result += "<";
                    }
                    current.X = target.X;
                }
                if (illegal.X != current.X || illegal.Y != target.Y)
                {
                    for (int i = 0; i < Math.Abs(target.Y - current.Y); i++)
                    {
                        var toAdd = target.Y < current.Y ? "^" : "v";
                        result += toAdd;
                    }
                    current.Y = target.Y;
                }
                for (int i = 0; i < Math.Abs(target.X - current.X); i++)
                {
                    var toAdd = target.X < current.X ? "<" : ">";
                    result += toAdd;                    
                }
                for (int i = 0; i < Math.Abs(target.Y - current.Y); i++)
                {
                    var toAdd = target.Y < current.Y ? "^" : "v";
                    result += toAdd;                    
                }
                current = target;
                result += "A";
                yield return result;
            }            
        }

    }
}
