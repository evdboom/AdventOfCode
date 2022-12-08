using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2022.Days
{
    public class Day08 : Day
    {
        public Day08(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 8;
        protected override long ProcessPartOne(string[] input)
        {
            var trees = GetTrees(input);
            var visible = trees.GetLength(0) * 2 + trees.GetLength(1) * 2 - 4;
            for (int i = 1; i < trees.GetLength(0) - 1; i ++)
            {
                for (int j = 1; j < trees.GetLength(1) - 1; j++)
                {
                    var tree = trees[i, j];
                    var above = Enumerable.Range(0, j)
                        .Select(r => trees[i, r])
                        .All(v => v < tree);
                    var left = Enumerable.Range(0, i)
                        .Select(r => trees[r, j])
                        .All(v => v < tree);
                    var below = Enumerable.Range(j + 1, trees.GetLength(1) - j - 1)
                        .Select(r => trees[i, r])
                        .All(v => v < tree);
                    var rigt = Enumerable.Range(i + 1, trees.GetLength(0) - i - 1)
                        .Select(r => trees[r, j])
                        .All(v => v < tree);
                    if (above || left || below || rigt)
                    {
                        visible++;
                    }
                }
            }
            return visible;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var trees = GetTrees(input);
            var max = 0;
            for (int i = 0; i < trees.GetLength(0); i++)
            {

                for (int j = 0; j < trees.GetLength(1); j++)
                {
                    var tree = trees[i, j];
                    var left = 0;
                    var right = 0;
                    var top = 0;
                    var bottom = 0;

                    var leftFound = false;
                    var rightFound = false;
                    var topFound = false;
                    var bottomFound = false;

                    var counter = 1;
                    while (!leftFound || !rightFound || !topFound || !bottomFound)
                    {
                        if (!leftFound && i - counter >= 0)
                        {
                            if (trees[i - counter, j] < tree)
                            {
                                left++;
                            }
                            else
                            {
                                left++;
                                leftFound = true;
                            }                        
                        }
                        else
                        {
                            leftFound = true;
                        }

                        if (!rightFound && i + counter < trees.GetLength(0))
                        {
                            if (trees[i + counter, j] < tree)
                            {
                                right++;
                            }
                            else
                            {
                                right++;
                                rightFound = true;
                            }
                        }
                        else
                        {
                            rightFound = true;
                        }

                        if (!topFound && j - counter >= 0)
                        {
                            if (trees[i, j  - counter] < tree)
                            {
                                top++;
                            }
                            else
                            {
                                top++;
                                topFound = true;
                            }
                        }
                        else
                        {
                            topFound = true;
                        }

                        if (!bottomFound && j + counter < trees.GetLength(1))
                        {
                            if (trees[i, j + counter] < tree)
                            {
                                bottom++;
                            }
                            else
                            {
                                bottom++;
                                bottomFound = true;
                            }
                        }
                        else
                        {
                            bottomFound = true;
                        }
                        counter++;
                    }

                    max = Math.Max(max, left * right * top * bottom);

                }
            }

            return max;
        }

        private int[,] GetTrees(string[] input)
        {

            var result = new int[input[0].Length, input.Length];

            for (int i = 0; i < input[0].Length; i++) 
            {
                for (int j = 0; j < input.Length; j++)
                {
                    result[i, j] = int.Parse($"{input[j][i]}");
                }
            }

            return result;

        }
    }
}
