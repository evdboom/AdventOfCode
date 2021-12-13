using AdventOfCode2021.Enums;
using AdventOfCode2021.Services;
using System.Diagnostics;

namespace AdventOfCode2021.Days
{
    public abstract class Day : IDay
    {
        private readonly IFileImporter _importer;
        public abstract int DayNumber { get; }

        public Day(IFileImporter importer)
        {
            _importer = importer;
        }

        public async Task<(long answer, long duration)> ProcessPartAsync(Part part)
        {
            var sw = Stopwatch.StartNew();
            var result = await ProcessPartAsyncInternal(part);
            sw.Stop();

            return (result, sw.ElapsedMilliseconds);
        }

        private async Task<long> ProcessPartAsyncInternal(Part part)
        {
            var input = await _importer.GetInputAsync(DayNumber);
            return part switch
            {
                Part.One => ProcessPartOne(input),
                Part.Two => ProcessPartTwo(input),
                _ => throw new ArgumentException($"{part} is not valid (should be 1 or 2)", nameof(part))
            };
        }

        protected abstract long ProcessPartOne(string[] input);

        protected abstract long ProcessPartTwo(string[] input);        
    }
}
