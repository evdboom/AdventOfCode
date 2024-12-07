using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days
{
    public class Day07 : Day
    {
        public Day07(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 7;

        protected override long ProcessPartOne(string[] input)
        {
            return input.Aggregate(0L, (acc, line) =>
            {
                var (result, parts) = ParseLine(line);
                if (ProcessPart(result, 0, 0, parts).Any(c => c))
                {
                    return acc + result;
                }

                return acc;
            });
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input.Aggregate(0L, (acc, line) =>
            {
                var (result, parts) = ParseLine(line);
                if (ProcessPart(result, 0, 0, parts, true).Any(c => c))
                {
                    return acc + result;
                }

                return acc;
            });
        }

        private IEnumerable<bool> ProcessPart(long target, long current, long index, long[] parts, bool includeConcat = false)
        {
            if (index == parts.Length)
            {
                yield return target == current;
                yield break;
            }

            var sum = current + parts[index];
            var mul = current * parts[index];          

            if (sum <= target)
            {
                foreach (var result in ProcessPart(target, sum, index + 1, parts, includeConcat))
                {
                    yield return result;
                }
            }

            if (mul <= target)
            {
                foreach (var result in ProcessPart(target, mul, index + 1, parts, includeConcat))
                {
                    yield return result;
                }
            }

            if (includeConcat)
            {
                var con = long.Parse($"{current}{parts[index]}");
                if (con <= target)
                {
                    foreach (var result in ProcessPart(target, con, index + 1, parts, includeConcat))
                    {
                        yield return result;
                    }
                }
            }
        }

        private (long Result, long[] Parts) ParseLine(string line)
        {
            var all = line.Split(": ");
            var result = long.Parse(all[0]);
            var parts = all[1]
                .Split(" ")
                .Select(long.Parse)
                .ToArray();

            return (result, parts);
        }
    }
}
