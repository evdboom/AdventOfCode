using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day17Group;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day18 : Day
    {
        public Day18(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 18;
        protected override long ProcessPartOne(string[] input)
        {
            var lavaGrid = BuildGrid(input);
            var freeSides = GetFreeSides(lavaGrid);
            return freeSides;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var lavaGrid = BuildGrid(input);
            var freeSides = GetFreeSides(lavaGrid);
            var pockets = new List<List<int[]>>();
            return freeSides;
        }

        private int GetFreeSides(bool[,,] lavaGrid)
        {
            var freeSides = 0;
            for (int k = 0; k < lavaGrid.GetLength(2); k++)
            {
                for (int j = 0; j < lavaGrid.GetLength(1); j++)
                {
                    for (int i = 0; i < lavaGrid.GetLength(0); i++)
                    {
                        if (lavaGrid[i, j, k])
                        {
                            // top
                            if (j == 0 || !lavaGrid[i, j - 1, k])
                            {
                                freeSides++;
                            }
                            // bottom
                            if (j == lavaGrid.GetLength(1) - 1 || !lavaGrid[i, j + 1, k])
                            {
                                freeSides++;
                            }
                            // left                            
                            if (i == 0 || !lavaGrid[i - 1, j, k])
                            {
                                freeSides++;
                            }
                            // right
                            if (i == lavaGrid.GetLength(0) - 1 || !lavaGrid[i + 1, j, k])
                            {
                                freeSides++;
                            }
                            // front                            
                            if (k == 0 || !lavaGrid[i, j, k - 1])
                            {
                                freeSides++;
                            }
                            // back
                            if (k == lavaGrid.GetLength(2) - 1 || !lavaGrid[i, j, k + 1])
                            {
                                freeSides++;
                            }
                        }
                    }
                }
            }

            return freeSides;
        }

        private bool[,,] BuildGrid(string[] input)
        {
            var values = input
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray());

            var max = values.Aggregate(new int[3], (maxes, value) =>
            {
                maxes[0] = Math.Max(maxes[0], value[0]);
                maxes[1] = Math.Max(maxes[1], value[1]);
                maxes[2] = Math.Max(maxes[2], value[2]);
                return maxes;
            });

            var result = new bool[max[0] + 1, max[1] + 1, max[2] + 1];
            foreach(var value in values)
            {
                result[value[0], value[1], value[2]] = true;
            }

            return result;
        }
    }
}