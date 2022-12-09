using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day09 : Day
    {
        public Day09(IFileImporter importer) : base(importer)
        {

        }

        public override int DayNumber => 9;
        protected override long ProcessPartOne(string[] input)
        {
            var points = new List<Point>() { Point.Empty };
            var head = Point.Empty;
            var tail = Point.Empty;
            foreach(var line in input) 
            {
                var parts = line.Split(' ');
                var amount = int.Parse(parts[1]);
                switch (parts[0])
                {
                    case "R":
                        for (int i = 0; i < amount; i++)
                        {
                            head.X++;
                            if (ShouldMove(head, tail, out Point newTail))
                            {
                                tail = newTail;
                                if (!points.Contains(tail))
                                {
                                    points.Add(tail);
                                }
                            }                           
                        }
                        break;
                    case "L":
                        for (int i = 0; i < amount; i++)
                        {
                            head.X--;
                            if (ShouldMove(head, tail, out Point newTail))
                            {
                                tail = newTail;
                                if (!points.Contains(tail))
                                {
                                    points.Add(tail);
                                }
                            }
                        }
                        break;
                    case "U":
                        for (int i = 0; i < amount; i++)
                        {
                            head.Y++;
                            if (ShouldMove(head, tail, out Point newTail))
                            {
                                tail = newTail;
                                if (!points.Contains(tail))
                                {
                                    points.Add(tail);
                                }
                            }
                        }
                        break;
                    case "D":
                        for (int i = 0; i < amount; i++)
                        {
                            head.Y--;
                            if (ShouldMove(head, tail, out Point newTail))
                            {
                                tail = newTail;
                                if (!points.Contains(tail))
                                {
                                    points.Add(tail);
                                }
                            }
                        }
                        break;

                }
            }
            return points.Count;
        }

        private bool ShouldMove(Point head, Point tail, [MaybeNullWhen(false)] out Point newTail)
        {
            newTail = tail;
            var distanceX = head.X - tail.X;
            var distanceY = head.Y - tail.Y;
            if (distanceY == 2)
            {
                newTail.Y++;
                newTail.X += distanceX > 0 ? 1 : distanceX < 0 ? -1 : 0;
                return true;
            }
            else if (distanceY == -2)
            {
                newTail.Y--;
                newTail.X += distanceX > 0 ? 1 : distanceX < 0 ? -1 : 0;
                return true;
            }
            else if (distanceX == 2)
            {
                newTail.X++;
                newTail.Y += distanceY > 0 ? 1 : distanceY < 0 ? -1 : 0;
                return true;

            }
            else if (distanceX == -2)
            {
                newTail.X--;
                newTail.Y += distanceY > 0 ? 1 : distanceY < 0 ? -1 : 0;
                return true;
            }
            
            return false;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var points = new List<Point>() { Point.Empty };
            var knots = Enumerable
                .Range(0, 10)
                .Select(r => Point.Empty)
                .ToList();
            foreach (var line in input)
            {
                var parts = line.Split(' ');
                var amount = int.Parse(parts[1]);
                switch (parts[0])
                {
                    case "R":
                        for (int i = 0; i < amount; i++)
                        {
                            var head = knots[0];
                            head.X++;
                            knots[0] = head;
                            for (int j = 0; j < knots.Count - 1;j++)
                            {
                                if (ShouldMove(knots[j], knots[j+1], out Point newKnot))
                                {
                                    knots[j+1] = newKnot;
                                    if (j+1 == 9 && !points.Contains(knots[j+1]))
                                    {
                                        points.Add(knots[j+1]);
                                    }
                                }
                            }                            
                        }
                        break;
                    case "L":
                        for (int i = 0; i < amount; i++)
                        {
                            var head = knots[0];
                            head.X--;
                            knots[0] = head;
                            for (int j = 0; j < knots.Count - 1; j++)
                            {
                                if (ShouldMove(knots[j], knots[j + 1], out Point newKnot))
                                {
                                    knots[j + 1] = newKnot;
                                    if (j + 1 == 9 && !points.Contains(knots[j + 1]))
                                    {
                                        points.Add(knots[j + 1]);
                                    }
                                }
                            }
                        }
                        break;
                    case "U":
                        for (int i = 0; i < amount; i++)
                        {
                            var head = knots[0];
                            head.Y++;
                            knots[0] = head;
                            for (int j = 0; j < knots.Count - 1; j++)
                            {
                                if (ShouldMove(knots[j], knots[j + 1], out Point newKnot))
                                {
                                    knots[j + 1] = newKnot;
                                    if (j + 1 == 9 && !points.Contains(knots[j + 1]))
                                    {
                                        points.Add(knots[j + 1]);
                                    }
                                }
                            }
                        }
                        break;
                    case "D":
                        for (int i = 0; i < amount; i++)
                        {
                            var head = knots[0];
                            head.Y--;
                            knots[0] = head;
                            for (int j = 0; j < knots.Count - 1; j++)
                            {
                                if (ShouldMove(knots[j], knots[j + 1], out Point newKnot))
                                {
                                    knots[j + 1] = newKnot;
                                    if (j + 1 == 9 && !points.Contains(knots[j + 1]))
                                    {
                                        points.Add(knots[j + 1]);
                                    }
                                }
                            }
                        }
                        break;

                }
            }
            return points.Count;
        }

    }
}
