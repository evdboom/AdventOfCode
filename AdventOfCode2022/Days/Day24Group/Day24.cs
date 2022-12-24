using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day24Group;
using System.Drawing;
using System.Security.Cryptography;

namespace AdventOfCode2022.Days
{
    public class Day24 : Day
    {
        private readonly IScreenWriter _writer;
        public Day24(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        private void DrawGrid(Point start, Point finish, Point player, List<Blizzard> blizzards, int width, int height)
        {
            _writer.NewLine();
            _writer.Write('#');
            for (int i = 0; i < width; i++)
            {
                if (i == start.X && player.Y > -1)
                {
                    _writer.Write('.');
                }
                else if (player.Y == -1 && player.X == i)
                {
                    _writer.Write('E');
                }
                else
                {
                    _writer.Write('#');
                }
            }
            _writer.Write('#');
            _writer.NewLine();
            for (int j = 0; j < height; j++) 
            {
                _writer.Write('#');
                for (int i = 0; i < width; i++) 
                {
                    if (player.X == i && player.Y == j)
                    {
                        _writer.Write('E');
                    }
                    else
                    {
                        var blizzardsOnPoint = blizzards.Where(b => b.Location.X == i && b.Location.Y == j).ToList();
                        if (blizzardsOnPoint.Count > 1)
                        {
                            _writer.Write(blizzardsOnPoint.Count);
                        }
                        else if (blizzardsOnPoint.Count == 1)
                        {
                            _writer.Write(blizzardsOnPoint[0].Direction);
                        }
                        else
                        {
                            _writer.Write('.');
                        }
                    }
                }
                _writer.Write('#');
                _writer.NewLine();
            }
            _writer.Write('#');
            for (int i = 0; i < width; i++)
            {
                if (i == finish.X)
                {
                    _writer.Write('.');
                }
                else
                {
                    _writer.Write('#');
                }
            }
            _writer.Write('#');
            _writer.NewLine();
        }

        private Point GetInitial(string[] input, out Point finish, out int width, out int height, out List<Blizzard> blizzards)
        {
            blizzards = new List<Blizzard>();
            var start = Point.Empty;
            finish = Point.Empty;
            width = input[0].Length - 2;
            height = input.Length - 2;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (input[j + 1][i + 1] != '.')
                    {
                        blizzards.Add(new Blizzard { Location = new Point(i, j), Direction = input[j + 1][i + 1] });
                    }
                }
            }

            start.X = input[0].IndexOf('.') - 1;
            start.Y = -1;

            finish.X = input[input.Length - 1].IndexOf('.') - 1;
            finish.Y = height;

            return start;

         
        }

        public override int DayNumber => 24;

        protected override long ProcessPartOne(string[] input)
        {
            var start = GetInitial(input, out Point finish, out int width, out int height, out List<Blizzard> blizzards);
            var blizzardCache = new Dictionary<int, List<Blizzard>> { { 0, blizzards } };
            var step = GetMinStep(start, finish, blizzardCache, width, height, 0);
            return step;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var start = GetInitial(input, out Point finish, out int width, out int height, out List<Blizzard> blizzards);
            var blizzardCache = new Dictionary<int, List<Blizzard>> { { 0, blizzards } };
            var step1 = GetMinStep(start, finish, blizzardCache, width, height, 0);
            var step2 = GetMinStep(finish, start, blizzardCache, width, height, step1);
            var step3 = GetMinStep(start, finish, blizzardCache, width, height, step2);
            return step3;
        }

        private int GetMinStep(Point start, Point finish, Dictionary<int, List<Blizzard>> blizzardCache, int width, int height, int startStep)
        {
            var queue = new PriorityQueue<(Point Point, int Step), int>();
            queue.Enqueue((start, startStep), 0);            
            var pointCache = new Dictionary<Point, List<int>> { { start, new List<int> { startStep } } };
            var minStep = int.MaxValue;
            while (queue.TryDequeue(out (Point Point, int Step) item, out _))
            {
                if (item.Point == finish)
                {
                    if (item.Step < minStep)
                    {
                        minStep = item.Step;
                    }
                }
                var distanceNow = Math.Abs(item.Point.X - finish.X) + Math.Abs(item.Point.Y - finish.Y);
                if (item.Step + distanceNow > minStep)
                {
                    continue;
                }

                var step = item.Step + 1;
                if (!blizzardCache.ContainsKey(step))
                {
                    blizzardCache[step] = blizzardCache[0].Select(b => b.LocationAtStep(step, width, height)).ToList();
                }
                foreach (var move in GetMoves(item.Point, finish, blizzardCache[step], width, height))
                {
                    var distance = Math.Abs(move.X - finish.X) + Math.Abs(move.Y - finish.Y);
                    if (distance + step < minStep)
                    {
                        if (!pointCache.ContainsKey(move))
                        {
                            pointCache[move] = new List<int>();
                        }

                        if (!pointCache[move].Contains(step))
                        {
                            pointCache[move].Add(step);
                            queue.Enqueue((move, step), distance);
                        }
                    }
                }
            }
            return minStep;
        }

        private IEnumerable<Point> GetMoves(Point origin, Point finish, List<Blizzard> blizzards, int width, int height)
        {
            bool hasMoved = false;
            var down = origin with { Y = origin.Y + 1 };
            if ((down.Y < height || down == finish) && !blizzards.Any(b => b.Location == down))
            {
                hasMoved = true;
                yield return down;
            }
            var up = origin with { Y = origin.Y - 1 };
            if ((up.Y >= 0 || up == finish) && !blizzards.Any(b => b.Location == up))
            {
                hasMoved = true;
                yield return up;
            }
            var right = origin with { X = origin.X + 1 };
            if (right.Y >= 0 && right.Y < height && right.X < width && !blizzards.Any(b => b.Location == right))
            {
                yield return right;
            }
            var left = origin with { X = origin.X - 1 };
            if (left.Y >= 0 && left.Y < height && left.X >= 0 && !blizzards.Any(b => b.Location == left))
            {
                yield return left;
            }
            if (!blizzards.Any(b => b.Location == origin))
            {
                yield return origin;
            }
        }      
    }
}