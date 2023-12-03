using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day03 : Day
    {
        public Day03(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 3;

        protected override long ProcessPartOne(string[] input)
        {
            var (numbers, symbols) = GetNumbersAndSymbols(input);

            return numbers
                .Where(number => HasSymbolMatch(number, symbols))
                .Select(number => number.Value)
                .Sum();
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (numbers, symbols) = GetNumbersAndSymbols(input);

            return symbols
                .Where(symbol => symbol.Symbol == '*')
                .Select(symbol => GetMatchesForSymbol(symbol, numbers))
                .Where(matches => matches.Count == 2)
                .Select(matches => matches[0] * matches[1])
                .Sum();
        }

        private List<long> GetMatchesForSymbol((int Row, int Column, char Symbol) symbol, List<(int Row, int Start, int End, long Value)> numbers)
        {
            return numbers
                .Where(number => HasSymbolMatch(number, symbol))
                .Select(number => number.Value)
                .ToList();
        }

        private (List<(int Row, int Start, int End, long Value)> Numbers, List<(int Row, int Column, char Symbol)> Symbols) GetNumbersAndSymbols(string[] input)
        {
            var grid = new int[input[0].Length, input.Length];
            var numbers = new List<(int Row, int Start, int End, long Value)>();
            var symbols = new List<(int Row, int Column, char Symbol)>();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var currentNumber = string.Empty;
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    var ch = input[j][i];
                    if (char.IsNumber(ch))
                    {
                        currentNumber += ch;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentNumber))
                        {
                            numbers.Add((j, i - currentNumber.Length, i - 1, long.Parse(currentNumber)));
                            currentNumber = string.Empty;
                        }
                        if (ch != '.')
                        {
                            symbols.Add((j, i, ch));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    numbers.Add((j, grid.GetLength(0) - currentNumber.Length, grid.GetLength(0) - 1, long.Parse(currentNumber)));
                }
            }

            return (numbers, symbols);
        }
           
        private bool HasSymbolMatch((int Row, int Start, int End, long Value) number, List<(int Row, int Column, char Symbol)> symbols)
        {
            return symbols
                .Any(symbol => HasSymbolMatch(number, symbol));
        }

        private bool HasSymbolMatch((int Row, int Start, int End, long Value) number, (int Row, int Column, char Symbol) symbol)
        {
            return
                symbol.Row >= (number.Row - 1) &&
                symbol.Row <= (number.Row + 1) &&
                symbol.Column >= (number.Start - 1) &&
                symbol.Column <= (number.End + 1);
        }
    }
}
