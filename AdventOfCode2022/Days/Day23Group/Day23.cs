using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Grid;
using AdventOfCode.Shared.Services;
using System.Drawing;
using System.Text;

namespace AdventOfCode2022.Days
{
    public class Day23 : Day
    {
        private const string Directions = "NSWE";
        public Day23(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 23;
        protected override long ProcessPartOne(string[] input)
        {
            var grid = input.ToGrid((c) => c == '#');

            for (int round = 0; round < 10; round ++)
            {
                ProcessRound(grid, round, out grid);
            }

            grid = ShrinkGrid(grid);

            var free = 0L;
            for (int j = 0; j < grid.Height; j++)
            {
                for (int i = 0; i < grid.Width; i++)
                {
                    if (!(grid[i, j]))
                    {
                        free++;
                    }
                }
            }

            return free;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = input.ToGrid((c) => c == '#');
            var round = 0;
            while(ProcessRound(grid, round, out grid) > 0)
            {
                round++;
            }

            return round + 1;
        }

        protected int ProcessRound(Grid<bool> grid, int round, out Grid<bool> newGrid)
        {
            grid = ExpandGrid(grid);
            var start = round % 4;
            var moves = new Dictionary<Point, Point>();
            for (int j = 1; j < grid.Height - 1; j++)
            {
                for (int i = 1; i < grid.Width - 1; i++)
                {
                    if (grid[i, j])
                    {
                        var operation = start;
                        var moveFound = false;
                        var adjacent = grid.Adjacent(i, j, true)
                            .Select(a => (Point: a, Value: grid[a.X, a.Y]))
                            .ToList();
                        if (adjacent.All(a => !a.Value))
                        {
                            moveFound = true;
                        }

                        while (!moveFound)
                        {
                            switch (Directions[operation])
                            {
                                case 'N':
                                    if (adjacent
                                        .Where(a => a.Point.Y == j - 1)
                                        .All(a => !(a.Value)))
                                    {
                                        moveFound = true;
                                        var move = new Point(i, j - 1);
                                        if (moves.ContainsKey(move))
                                        {
                                            moves.Remove(move);
                                        }
                                        else
                                        {
                                            moves[move] = new Point(i, j);
                                        }
                                    }
                                    break;
                                case 'S':
                                    if (adjacent
                                        .Where(a => a.Point.Y == j + 1)
                                        .All(a => !(a.Value)))
                                    {
                                        moveFound = true;
                                        var move = new Point(i, j + 1);
                                        if (moves.ContainsKey(move))
                                        {
                                            moves.Remove(move);
                                        }
                                        else
                                        {
                                            moves[move] = new Point(i, j);
                                        }
                                    }
                                    break;
                                case 'W':
                                    if (adjacent
                                        .Where(a => a.Point.X == i - 1)
                                        .All(a => !(a.Value)))
                                    {
                                        moveFound = true;
                                        var move = new Point(i - 1, j);
                                        if (moves.ContainsKey(move))
                                        {
                                            moves.Remove(move);
                                        }
                                        else
                                        {
                                            moves[move] = new Point(i, j);
                                        }
                                    }
                                    break;
                                case 'E':
                                    if (adjacent
                                        .Where(a => a.Point.X == i + 1)
                                        .All(a => !(a.Value)))
                                    {
                                        moveFound = true;
                                        var move = new Point(i + 1, j);
                                        if (moves.ContainsKey(move))
                                        {
                                            moves.Remove(move);
                                        }
                                        else
                                        {
                                            moves[move] = new Point(i, j);
                                        }
                                    }
                                    break;
                            }

                            operation = (operation + 1) % 4;
                            if (operation == start)
                            {
                                moveFound = true;
                            }
                        }
                    }
                }
            }
            foreach (var move in moves)
            {
                grid[move.Key.X, move.Key.Y] = true;
                grid[move.Value.X, move.Value.Y] = false;
            }
            newGrid = grid;
            return moves.Count;
        }

        private Grid<bool> ShrinkGrid(Grid<bool> grid)
        {
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;
            for (int j = 0; j < grid.Height / 2; j++)
            {
                var topFound = false;
                var bottomFound = false;
                for (int i = 0; i < grid.Width; i ++)
                {
                    if (grid[i, j])
                    {
                        topFound = true;
                    }
                    if (grid[i, grid.Height - 1 - j])
                    {
                        bottomFound = true;
                    }
                    if (topFound && bottomFound)
                    {
                        break;
                    }
                }
                if (!topFound)
                {
                    top++;
                }
                if (!bottomFound)
                {
                    bottom++;
                }
                if (topFound && bottomFound)
                {
                    break;
                }
            }
            for (int i = 0; i < grid.Width / 2; i++)
            {
                var leftFound = false;
                var rightFound = false;
                for (int j = 0; j < grid.Height; j++)
                {
                    if (grid[i, j])
                    {
                        leftFound = true;
                    }
                    if (grid[grid.Width - 1 - i, j])
                    {
                        rightFound = true;
                    }
                    if (leftFound && rightFound)
                    {
                        break;
                    }
                }
                if (!leftFound)
                {
                    left++;
                }
                if (!rightFound)
                {
                    right++;
                }
                if (leftFound && rightFound)
                {
                    break;
                }
            }

            var newHeight = grid.Height - top - bottom;
            var newWidth = grid.Width - left - right;

            var result = new Grid<bool>(newWidth, newHeight);
            for (int j = 0; j < result.Height; j++)
            {
                for (int i = 0; i < result.Width; i++)
                {
                    result[i , j] = grid[i + left, j + top];
                }
            }
            return result;
        }

        private Grid<bool> ExpandGrid(Grid<bool> grid)
        {
            var expand = false;
            var max = Math.Max(grid.Width, grid.Height);
            for (int i = 0; i < max; i++) 
            {
                if (i < grid.Width)
                {
                    expand = expand || (grid[i, 0]) || (grid[i, grid.Height - 1]);
                }
                if (i < grid.Height )
                {
                    expand = expand || (grid[0, i]) || (grid[grid.Width - 1, i]);
                }
                if (expand)
                {
                    break;
                }
            }

            if (expand)
            {
                var newGrid = new Grid<bool>(grid.Width + 2, grid.Height + 2);
                for (int j = 0; j < grid.Height; j++)
                {
                    for (int i = 0; i < grid.Width; i++)
                    {
                        newGrid[i + 1, j + 1] = grid[i, j];
                    }
                }
                return newGrid;
            }
            else
            {
                return grid;
            }
        }
    }
}