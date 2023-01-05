using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

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
            var freeSides = GetWithoutPockets(lavaGrid);
            return freeSides;
        }

        private int GetWithoutPockets(bool[,,] lavaGrid)
        {
            var visited = new Dictionary<(int x, int y, int z), bool>();
            var touched = new Dictionary<(int x, int y, int z, int side), bool>();
            var point1 = (0, 0, 0);
            var queue = new Queue<(int x, int y, int z)>();
            queue.Enqueue(point1);

            while (queue.TryDequeue(out var result))
            {
                if (visited.ContainsKey(result))
                {
                    continue;
                }

                visited[result] = true;

                if (result.x > 0)
                {
                    if (lavaGrid[result.x - 1, result.y, result.z])
                    {
                        touched[(result.x - 1, result.y, result.z, 1)] = true;
                    }
                    else if (!visited.ContainsKey((result.x - 1, result.y, result.z)))
                    {
                        queue.Enqueue((result.x - 1, result.y, result.z));
                    }
                }
                if (result.x < lavaGrid.GetLength(0) - 1)
                {
                    if (lavaGrid[result.x + 1, result.y, result.z])
                    {
                        touched[(result.x + 1, result.y, result.z, 2)] = true;
                    }
                    else if (!visited.ContainsKey((result.x + 1, result.y, result.z)))
                    {
                        queue.Enqueue((result.x + 1, result.y, result.z));
                    }
                }

                if (result.y > 0)
                {
                    if (lavaGrid[result.x, result.y - 1, result.z])
                    {
                        touched[(result.x, result.y - 1, result.z, 3)] = true;
                    }
                    else if (!visited.ContainsKey((result.x, result.y - 1, result.z)))
                    {
                        queue.Enqueue((result.x, result.y - 1, result.z));
                    }
                }
                if (result.y < lavaGrid.GetLength(1) - 1)
                {
                    if (lavaGrid[result.x, result.y + 1, result.z])
                    {
                        touched[(result.x, result.y + 1, result.z, 4)] = true;
                    }
                    else if (!visited.ContainsKey((result.x, result.y + 1, result.z)))
                    {
                        queue.Enqueue((result.x, result.y + 1, result.z));
                    }
                }

                if (result.z > 0)
                {
                    if (lavaGrid[result.x, result.y, result.z - 1])
                    {
                        touched[(result.x, result.y, result.z - 1, 5)] = true;
                    }
                    else if (!visited.ContainsKey((result.x, result.y, result.z - 1)))
                    {
                        queue.Enqueue((result.x, result.y, result.z - 1));
                    }
                }
                if (result.z < lavaGrid.GetLength(2) - 1)
                {
                    if (lavaGrid[result.x, result.y, result.z + 1])
                    {
                        touched[(result.x, result.y, result.z + 1, 6)] = true;
                    }
                    else if (!visited.ContainsKey((result.x, result.y, result.z + 1)))
                    {
                        queue.Enqueue((result.x, result.y, result.z + 1));
                    }
                }
            }

            return touched.Count;
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

            var result = new bool[max[0] + 3, max[1] + 3, max[2] + 3];
            foreach (var value in values)
            {
                result[value[0] + 1, value[1] + 1, value[2] + 1] = true;
            }

            return result;
        }
    }
}