using AdventOfCode2021.Constructs;
using AdventOfCode2021.Services;
using System.Drawing;

namespace AdventOfCode2021.Days
{
    public class Day05 : Day
    {
        private const string CoordinateSplit = " -> ";
        private const char ValueSplit = ',';

        public override int DayNumber => 5;

        public Day05(IFileImporter importer) : base(importer)
        {
        }

        protected override long ProcessPartOne(string[] input)
        {
            return Process(input, false);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return Process(input, true);
        }

        private long Process(string[] input, bool includeDiagonal)
        {
            var lines = GetLines(input);
            var (width, heigth) = GetSize(lines);

            var board = new int[width + 1, heigth + 1];

            FillBoard(board, lines, includeDiagonal);

            return board
                .Cast<int>()
                .Count(v => v > 1);
        }

        private void FillBoard(int[,] board, List<Day05Line> lines, bool includeDiagonal)
        {
            foreach (var line in lines)
            {
                FillDiagonal(board, line, includeDiagonal);
                FillHorizontal(board, line);
                FillVertical(board, line);                
            }
        }

        private void FillVertical(int[,] board, Day05Line line)
        {
            if (line.Start.X != line.End.X)
            {
                return;
            }

            for (int i = Math.Min(line.Start.Y, line.End.Y); i <= Math.Max(line.Start.Y, line.End.Y); i++)
            {
                board[line.Start.X, i]++;
            }
        }

        private void FillHorizontal(int[,] board, Day05Line line)
        {
            if (line.Start.Y != line.End.Y)
            {
                return;
            }

            for (int i = Math.Min(line.Start.X, line.End.X); i <= Math.Max(line.Start.X, line.End.X); i++)
            {
                board[i, line.End.Y]++;
            }
        }

        private void FillDiagonal(int[,] board, Day05Line line, bool includeDiagonal)
        {
            if (!includeDiagonal || !IsDiagonal(line))
            {
                return;
            }

            if (line.Start.X < line.End.X)
            {
                FillDiagonal(board, line.Start, line.End);
            }
            else
            {
                FillDiagonal(board, line.End, line.Start);
            }
        }

        private void FillDiagonal(int[,] board, Point start, Point end)
        {
            bool up = start.Y < end.Y;
            int y = start.Y;
            for(int i = start.X; i <= end.X; i++)
            {
                board[i, y]++;
                y = up
                    ? y + 1
                    : y - 1;                
            }
        }

        private (int Width, int Heigth) GetSize(List<Day05Line> lines)
        {
            var width = 0;
            var height = 0;

            foreach (var line in lines)
            {
                width = Math.Max(Math.Max(width, line.Start.X), line.End.X);
                height = Math.Max(Math.Max(height, line.Start.Y), line.End.Y);
            }

            return (width, height);
        }

        private List<Day05Line> GetLines(string[] input)
        {
            List<Day05Line> result = new();
            foreach (var row in input)
            {
                var line = row
                    .Split(CoordinateSplit)
                    .Select(c => GetCoordinate(c))
                    .ToArray();

                result.Add(new Day05Line
                {
                    Start = line[0],
                    End = line[1]
                });
            }

            return result;
        }

        private Point GetCoordinate(string point)
        {
            var coordinates = point
                .Split(ValueSplit)
                .Select(p => int.Parse(p))
                .ToArray();

            return new Point(coordinates[0], coordinates[1]);
        }

        private bool IsDiagonal(Day05Line line)
        {
            return
                line.Start.X != line.End.X &&
                line.Start.Y != line.End.Y;
        }
    }
}
