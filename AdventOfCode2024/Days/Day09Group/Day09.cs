using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day09(IFileImporter importer) : Day(importer)
    {
        public override int DayNumber => 9;

        protected override long ProcessPartOne(string[] input)
        {
            var fileSystem = input[0]
                .Select(c => c - '0')
                .ToList();
            return CompactFileSystem(fileSystem);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var fileSystem = input[0]
                .Select(c => c - '0')
                .ToList();
            return MoveFiles(fileSystem);
        }

        private long MoveFiles(List<int> fileSystem)
        {
            var moves = new Dictionary<int, List<int>>();
            var moved = new List<int>();

            var maxFileIndex = fileSystem.Count - 1;
            if (maxFileIndex % 2 != 0)
            {
                maxFileIndex--;
            }
            for (int i = maxFileIndex; i >= 0; i -= 2)
            {
                var size = fileSystem[i];
                for (int j = 1; j < fileSystem.Count - 2; j += 2)
                {
                    var freeSize = fileSystem[j];
                    if (freeSize >= size)
                    {
                        if (!moves.TryGetValue(j, out List<int>? value))
                        {
                            value = [];
                            moves[j] = value;
                        }

                        value.Add(i);
                        moved.Add(i);
                        fileSystem[j] -= size;
                        break;
                    }
                }
            }

            var result = 0L;
            var checkSumIndex = 0;
            for (int i = 0; i < fileSystem.Count - 1; i++)
            {
                var isFile = i % 2 == 0;
                if (isFile)
                {
                    if (moved.Contains(i))
                    {
                        checkSumIndex += fileSystem[i];
                    }
                    else
                    {
                        var index = i / 2;
                        var size = fileSystem[i];
                        for (int j = 0; j < size; j++)
                        {
                            result += index * checkSumIndex;
                            checkSumIndex++;
                        }
                    }
                }
                else
                {
                    if (moves.TryGetValue(i, out var value))
                    {
                        foreach (var fileIndex in value)
                        {
                            var index = fileIndex / 2;
                            var size = fileSystem[fileIndex];
                            for (int j = 0; j < size; j++)
                            {
                                result += index * checkSumIndex;
                                checkSumIndex++;
                            }
                        }
                    }
                    checkSumIndex += fileSystem[i];
                }
            }

            return result;
        }

        private long CompactFileSystem(List<int> fileSystem)
        {
            var result = 0L;
            var checkSumIndex = 0;
            var maxFileIndex = fileSystem.Count - 1;
            if (maxFileIndex % 2 != 0)
            {
                maxFileIndex--;
            }

            for (int i = 0; i < fileSystem.Count - 1; i++)
            {
                var isFile = i % 2 == 0;
                if (i >= maxFileIndex && !isFile)
                {
                    break;
                }

                var size = fileSystem[i];
                var index = i / 2;
                

                if (isFile)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result += checkSumIndex * index;
                        checkSumIndex++;
                    }
                }
                else
                {
                    var done = 0;
                    while (done < size)
                    {
                        var lastFile = fileSystem[maxFileIndex];
                        var lastFileIndex = maxFileIndex / 2;
                        result += checkSumIndex * lastFileIndex;
                        checkSumIndex++;
                        done++;
                        lastFile--;
                        fileSystem[maxFileIndex] = lastFile;
                        if (lastFile == 0)
                        {                         
                            maxFileIndex -= 2;
                        }
                    }
                }

            }
            return result;
        }

        private List<(int Index, bool IsFile, int Size)> GetHardDiscContents(string input)
        {
            return input
                .Select((c, i) => (i / 2, i % 2 == 0, c - '0'))
                .ToList();
        }
    }
}
