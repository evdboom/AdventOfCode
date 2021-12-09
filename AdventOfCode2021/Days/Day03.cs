using AdventOfCode2021.Importers;

namespace AdventOfCode2021.Days
{
    public class Day03 : Day
    {
        private const char Zero = '0';
        private const char One = '1';
        protected override int DayNumber => 3;

        public Day03(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] lines)
        {
            var bitCount = GetBitPositionCount(lines);
            var (gamma, epsilon) = CalculateValues(bitCount);

            return gamma * epsilon;
        }

        protected override long ProcessPartTwo(string[] lines)
        {
            var oxygen = CalculateRating(lines, wanted: One, unwanted: Zero);
            var co2 = CalculateRating(lines, wanted: Zero, unwanted: One);

            return oxygen * co2;
        }

        private Dictionary<int, (int ones, int zeros)> GetBitPositionCount(string[] lines)
        {
            var result = new Dictionary<int, (int ones, int zeros)>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                for (int i = 0; i < line.Length; i++)
                {
                    if (!result.ContainsKey(i))
                    {
                        result[i] = (0, 0);
                    }

                    result[i] = line[i] == Zero 
                        ? (result[i].ones, result[i].zeros + 1)
                        : (result[i].ones + 1, result[i].zeros);
                }
            }

            return result;
        }

        private int CalculateRating(string[] sourceLines, char wanted, char unwanted)
        {
            var lines = sourceLines
                .Where(line => !string.IsNullOrEmpty(line))
                .ToArray();

            int currentColumn = 0;
            while (lines.Length > 1)
            {
                GetNextRating(ref lines, wanted, unwanted, currentColumn);
                currentColumn++;
            }

            return Convert.ToInt32(lines[0], 2);
        }

        private void GetNextRating(ref string[] lines, char wanted, char unwanted, int currentColumn)
        {
            var bitCount = GetBitPositionCount(lines);
            var (ones, zeros) = bitCount[currentColumn];

            lines = lines
                .Where(line => line[currentColumn] == (ones >= zeros ? wanted : unwanted))
                .ToArray();
        }

        private (int gamma, int epsilon) CalculateValues(Dictionary<int, (int ones, int zeros)> bitCount)
        {
            string resultGamma = string.Empty;
            string resultEpsilon = string.Empty;
            foreach (var column in bitCount.OrderBy(b => b.Key))
            {
                if (column.Value.ones > column.Value.zeros)
                {
                    resultGamma += One;
                    resultEpsilon += Zero;
                }
                else
                {
                    resultGamma += Zero;
                    resultEpsilon += One;
                }
            }

            return (Convert.ToInt32(resultGamma, 2), Convert.ToInt32(resultEpsilon, 2));
        }
    }
}
