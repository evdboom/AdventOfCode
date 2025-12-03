using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2025.Days
{
    public class Day03(IFileImporter fileImporter) : Day(fileImporter)
    {
        public override int DayNumber => 3;

        protected override long ProcessPartOne(string[] input)
        {
            return GetBanks(input).Sum(banks => GetHighestJoltage(banks, 2));
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return GetBanks(input).Sum(banks => GetHighestJoltage(banks, 12));
        }

        private static IEnumerable<IEnumerable<int>> GetBanks(string[] input)
        {
            return input.Select(bank => bank.Select(battery => battery - '0'));
        }

        private static long GetHighestJoltage(IEnumerable<int> bank, int count)
        {
            var batteries = bank.Aggregate(
                new int[count],
                (acc, joltage) =>
                {
                    for (int i = 1; i < count; i++)
                    {
                        if (acc[i - 1] < acc[i])
                        {
                            acc[i - 1] = acc[i];
                            acc[i] = 0;
                        }
                    }

                    if (joltage > acc[count - 1])
                    {
                        acc[count - 1] = joltage;
                    }

                    return acc;
                }
            );

            var result = long.Parse(string.Join(string.Empty, batteries));
            System.Diagnostics.Debug.WriteLine(result);

            return result;
        }
    }
}
