using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023.Days
{
    public class Day13 : Day
    {
        public Day13(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 13;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Aggregate(new List<List<string>>() { new() }, (fields, line) =>
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        fields.Add([]);
                    }
                    else
                    {
                        fields.Last().Add(line);
                    }

                    return fields;
                })
                .Select(field => field.ToArray().ToGrid(position => position == '#'))
                .Aggregate(0L, (result, grid) =>
                {
                    return TryFindMirrorColumn(grid, out var position)
                        ? result + position.Value
                        : result + (FindMirrorRow(grid) * 100);  
                });                            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Aggregate(new List<List<string>>() { new() }, (fields, line) =>
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        fields.Add([]);
                    }
                    else
                    {
                        fields.Last().Add(line);
                    }

                    return fields;
                })
                .Select(field => field.ToArray().ToGrid(position => position == '#'))
                .Aggregate(0L, (result, grid) =>
                {
                    return TryFindMirrorColumn(grid, out var position, allowSmudge: true)
                        ? result + position.Value
                        : result + (FindMirrorRow(grid, allowSmudge: true) * 100);
                });
        }

        private bool TryFindMirrorColumn(bool[,] grid, [NotNullWhen(true)] out int? position, bool allowSmudge = false)
        {
            List<List<bool>> columns = new();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                var column = new List<bool>();
                columns.Add(column);
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    column.Add(grid[i, j]);
                }
            }

            var indexes = GetMirrorIndexes(columns, allowSmudge);
            var index = GetMirrorIndex(indexes, columns, allowSmudge);

            if (index == -1)
            {
                position = null;
                return false;
            }

            position = index;
            return true;
        }

        private int FindMirrorRow(bool[,] grid, bool allowSmudge = false)
        {
            List<List<bool>> rows = new();
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var row = new List<bool>();
                rows.Add(row);
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    row.Add(grid[i, j]);
                }
            }

            var indexes = GetMirrorIndexes(rows, allowSmudge);
            var index = GetMirrorIndex(indexes, rows, allowSmudge);
            
            if (index == -1)
            {
                throw new InvalidOperationException("Could not find mirror");
            }

            return index;            
        }

        private List<(int Index, bool Smudged)> GetMirrorIndexes(List<List<bool>> arrays, bool allowSmudge)
        {
            var indexes = new List<(int, bool)>();
            for (int i = 0; i < arrays.Count - 1; i++)
            {
                if (arrays[i].SequenceEqual(arrays[i + 1]))
                {
                    indexes.Add((i, false));
                }
                else if (allowSmudge)
                {
                    if (arrays[i]
                        .Zip(arrays[i + 1])
                        .Where(zip => zip.First != zip.Second)
                        .Count() == 1)
                    {
                        indexes.Add((i, true));
                    }
                }
            }

            return indexes;
        }

        private int GetMirrorIndex(List<(int Index, bool Smudged)> indexes, List<List<bool>> arrays, bool allowSmudge)
        {
            foreach (var (index, smudged) in indexes)
            {
                var smudgeFound = smudged;
                if (index == 0 && (!allowSmudge || smudgeFound))
                {
                    return 1;
                }
                else if (index == arrays.Count - 2 && (!allowSmudge || smudgeFound))
                {
                    return arrays.Count - 1;
                }

                if (index > 0)
                {
                    var delta = Math.Min(index + 1, arrays.Count - (index + 1));
                    var valid = true;
                    for (int i = 1; i < delta; i++)
                    {
                        if (!arrays[index - i]
                            .SequenceEqual(arrays[index + i + 1]))
                        {
                            if (!allowSmudge || smudgeFound)
                            {
                                valid = false;
                                break;
                            }
                            else if (allowSmudge)
                            {
                                if (arrays[index - i]
                                    .Zip(arrays[index + i + 1])
                                    .Where(zip => zip.First != zip.Second)
                                    .Count() == 1)
                                {
                                    smudgeFound = true;
                                }                                
                            }
                        }

                    }

                    if (valid && (!allowSmudge || smudgeFound))
                    {
                        return index + 1;
                    }
                }
            }

            return -1;
        }    
    }
}