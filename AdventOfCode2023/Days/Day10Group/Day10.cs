using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day10 : Day
    {
        public Day10(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 10;

        protected override long ProcessPartOne(string[] input)
        {
            var (grid, start) = GetGrid(input);

            return FollowPipe(start, grid).Furthest;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (grid, start) = GetGrid(input);

            var pipe = FollowPipe(start, grid).Pipe;
            return GetContainedParts(grid, pipe);
        }

        private long GetContainedParts(char[,] grid, List<(int X, int Y)> pipe)
        {
            var parts = 0L;
            for (var j = 0; j < grid.GetLength(1); j++)
            {
                for(int i = 0; i < grid.GetLength(0); i++)
                {
                    if (pipe.Contains((i, j)))
                    {
                        continue;
                    }

                    var matches = 0;                    
                    char? matchStart = null;
                    for (int x = i; x >= 0; x--)
                    {
                        if (pipe.Contains((x, j)))
                        {
                            var pipePart = grid[x, j];
                            if (pipePart == '|')
                            {
                                matches++;
                            }
                            else if (matchStart is null)
                            {
                                matchStart = pipePart;
                            }
                            else if (pipePart != '-')
                            {
                                if (matchStart == 'J' && pipePart == 'F')
                                {
                                    matches++;
                                }
                                else if (matchStart == '7' && pipePart == 'L')
                                {
                                    matches++;
                                }
                                matchStart = null;
                            }
                        }
                    }

                    if (matches % 2 != 0)
                    {
                        parts++;
                    }
                }
            }

            return parts;
        }

        private (List<(int X, int Y)> Pipe, long Furthest) FollowPipe((int X, int Y) start, char[,] grid)
        {
            var result = new List<(int X, int Y)>
            {
                start
            };

            var startPositions = FindStartPipes(start, grid);

            var position1 = startPositions.First();
            var position2 = startPositions.Last();
            result.Add((position1.X, position1.Y));
            result.Add((position2.X, position2.Y));            

            var oldPosition1 = (start.X, start.Y);
            var oldPosition2 = (start.X, start.Y);

            var counter = 1L;

            while (position1 != position2)
            {
                var newPosition1 = FindNextPipe(position1, oldPosition1, grid);
                oldPosition1 = (position1.X, position1.Y);
                position1 = newPosition1;
                if (position1 == position2)
                {
                    break;
                }
                result.Add((position1.X, position1.Y));
                var newPosition2 = FindNextPipe(position2, oldPosition2, grid);
                oldPosition2 = (position2.X, position2.Y);
                position2 = newPosition2;
                if (position1 != position2)
                {
                    result.Add((position2.X, position2.Y));
                }
                counter++;
            }

            return (result, counter);
        }

        private (int X, int Y, char Pipe) FindNextPipe((int X, int Y, char Pipe) source, (int X, int Y) previous, char[,] grid)
        {
            return source.Pipe switch
            {
                '|' => previous.Y < source.Y
                    ? (source.X, source.Y + 1, grid[source.X, source.Y + 1])
                    : (source.X, source.Y - 1, grid[source.X, source.Y - 1]),
                '-' => previous.X < source.X
                    ? (source.X + 1, source.Y, grid[source.X + 1, source.Y])
                    : (source.X - 1, source.Y, grid[source.X - 1, source.Y]),
                'L' => previous.Y < source.Y
                    ? (source.X + 1, source.Y, grid[source.X + 1, source.Y])
                    : (source.X, source.Y - 1, grid[source.X, source.Y - 1]),
                'J' => previous.Y < source.Y
                    ? (source.X - 1, source.Y, grid[source.X - 1, source.Y])
                    : (source.X, source.Y - 1, grid[source.X, source.Y - 1]),
                '7' => previous.Y > source.Y
                    ? (source.X - 1, source.Y, grid[source.X - 1, source.Y])
                    : (source.X, source.Y + 1, grid[source.X, source.Y + 1]),
                'F' => previous.Y > source.Y
                    ? (source.X + 1, source.Y, grid[source.X + 1, source.Y])
                    : (source.X, source.Y + 1, grid[source.X, source.Y + 1]),
                _ => throw new InvalidOperationException("Unknown Pipe to follow")
            };
        }

        private IEnumerable<(int X, int Y, char Pipe)> FindStartPipes((int X, int Y) source, char[,] grid, bool secondOption = false)
        {
            bool found = false;
            if (source.Y > 0)
            {
                var top = grid[source.X, source.Y - 1];
                if (top == '|' || top == '7' || top == 'F')
                {                
                    if (!secondOption)
                    {
                        yield return (source.X, source.Y - 1, top);
                    }
                    found = true;
                }
            }
            if (source.X > 0)
            {
                var left = grid[source.X - 1, source.Y];
                if (left == 'L' || left == 'F' || left == '-')
                {
                    if (!secondOption || found)
                    {
                        yield return (source.X - 1, source.Y, left);
                    }
                    found = true;
                }
            }
            if (source.Y < grid.GetLength(1) - 1)
            {
                var bottom = grid[source.X, source.Y + 1];
                if (bottom == 'L' || bottom == '|' || bottom == 'J')
                {
                    if (!secondOption || found)
                    {
                        yield return (source.X, source.Y + 1, bottom);
                    }            
                }
            }
            if (source.X < grid.GetLength(0) - 1)
            {
                var right = grid[source.X + 1, source.Y];
                if (right == '7' || right == '-' || right == 'J')
                {
                    if (!secondOption || found)
                    {
                        yield return (source.X + 1, source.Y, right);
                    }
                }
            }                       
        }

        private (char[,] Grid, (int X, int Y) Start) GetGrid(string[] input)
        {
            var grid = new char[input[0].Length, input.Length];
            var start = (0, 0);
            for (int j = 0; j < input.Length; j++)
            {
                for (int i = 0; i < input[j].Length; i++)
                {
                    grid[i,j] = input[j][i];
                    if (grid[i,j] == 'S')
                    {
                        start = (i, j);
                    }
                }
            }

            return (grid, start);
        }
    }
}
