using AdventOfCode.Shared.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class GridExtensions
    {
        public static int[,] ToIntGrid(this string[] input)
        {
            return ToGrid(input, (c) => int.Parse($"{c}"));
        }

        public static T[,] ToGrid<T>(this string[] input, Func<char, T> parse)
        {
            var grid = new T[input[0].Length, input.Length];

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    grid[i, j] = parse(input[j][i]);
                }
            }

            return grid;
        }
        
        public static IEnumerable<Point> Adjacent(this Point point, bool allowDiagonal = false)
        {
            yield return point with { X = point.X + 1 };
            yield return point with { X = point.X - 1 };
            yield return point with { Y = point.Y + 1 };
            yield return point with { Y = point.Y - 1 };

            if (allowDiagonal)
            {
                yield return point with { X = point.X + 1, Y = point.Y + 1 };
                yield return point with { X = point.X - 1, Y = point.Y + 1 };
                yield return point with { X = point.X + 1, Y = point.Y - 1 };
                yield return point with { X = point.X - 1, Y = point.Y - 1 };
            }
        }

        public static bool TryGetPointInDirection<T>(this T[,] grid, Point point, Direction direction, Func<(T Origin, T Target), bool> compare, [NotNullWhen(true)] out Point? target)
        {
            return TryGetPointInDirection(grid, point.X, point.Y, direction, compare, out target);
        }

        public static bool TryGetPointInDirection<T>(this T[,] grid, int x, int y, Direction direction, Func<(T Origin, T Target), bool> compare, [NotNullWhen(true)] out Point? target)
        {
            target = direction switch
            {
                Direction.Up => new Point(x, y - 1),
                Direction.Down => new Point(x, y + 1),
                Direction.Left => new Point(x - 1, y),
                Direction.Right => new Point(x + 1, y),
                Direction.UpLeft => new Point(x - 1, y - 1),
                Direction.UpRight => new Point(x + 1, y - 1),
                Direction.DownLeft => new Point(x - 1, y + 1),
                Direction.DownRight => new Point(x + 1, y + 1),
                _ => throw new ArgumentException("Invalid Direction", nameof(direction)),
            };
            var valid = target.Value.X >= 0 &&
                        target.Value.X < grid.GetLength(0) &&
                        target.Value.Y >= 0 &&
                        target.Value.Y < grid.GetLength(1) &&
                        compare((grid[x, y], grid[target.Value.X, target.Value.Y]));

            if (!valid)
            {
                target = null;
            }

            return valid;
        }

        public static IEnumerable<(Point Point, Direction Direction)> AdjecentWithDirection<T>(this T[,] grid, Point point, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, point.X, point.Y, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Direction Direction)> AdjecentWithDirection<T>(this T[,] grid, int x, int y, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, x, y, (_) => true, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Direction Direction)> AdjecentWithDirection<T>(this T[,] grid, Point point, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, point.X, point.Y, compare, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Direction Direction)> AdjecentWithDirection<T>(this T[,] grid, int x, int y, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
            if (y > 0 && compare((grid[x, y], grid[x, y - 1])))
            {
                yield return (new Point(x, y - 1), Direction.Up);
            }
            if (y < grid.GetLength(1) - 1 && compare((grid[x, y], grid[x, y + 1])))
            {
                yield return (new Point(x, y + 1), Direction.Down);
            }
            if (x > 0 && compare((grid[x, y], grid[x - 1, y])))
            {
                yield return (new Point(x - 1, y), Direction.Left);
            }
            if (x < grid.GetLength(0) - 1 && compare((grid[x, y], grid[x + 1, y])))
            {
                yield return (new Point(x + 1, y), Direction.Right);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0 && compare((grid[x, y], grid[x - 1, y - 1])))
                {
                    yield return (new Point(x - 1, y - 1), Direction.UpLeft);
                }
                if (x > 0 && y < grid.GetLength(1) - 1 && compare((grid[x, y], grid[x - 1, y + 1])))
                {
                    yield return (new Point(x - 1, y + 1), Direction.DownLeft);
                }
                if (x < grid.GetLength(0) - 1 && y > 0 && compare((grid[x, y], grid[x + 1, y - 1])))
                {
                    yield return (new Point(x + 1, y - 1), Direction.UpRight);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && compare((grid[x, y], grid[x + 1, y + 1])))
                {
                    yield return (new Point(x + 1, y + 1), Direction.DownRight);
                }
            }
        }

        public static IEnumerable<Point> Adjacent<T>(this T[,] grid, Point point, bool allowDiagonal = false)
        {
            return Adjacent(grid, point.X, point.Y, allowDiagonal);
        }

        public static IEnumerable<Point> Adjacent<T>(this T[,] grid, int x, int y, bool allowDiagonal = false)
        {
            if (y > 0)
            {
                yield return new Point(x, y - 1);
            }
            if (y < grid.GetLength(1) - 1)
            {
                yield return new Point(x, y + 1);
            }
            if (x > 0)
            {
                yield return new Point(x - 1, y);
            }
            if (x < grid.GetLength(0) - 1)
            {
                yield return new Point(x + 1, y);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0)
                {
                    yield return new Point(x - 1, y - 1);
                }
                if (x > 0 && y < grid.GetLength(1) - 1)
                {
                    yield return new Point(x - 1, y + 1);
                }
                if (x < grid.GetLength(0) - 1 && y > 0)
                {
                    yield return new Point(x + 1, y - 1);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1)
                {
                    yield return new Point(x + 1, y + 1);
                }
            }
        }


        public static IEnumerable<Point> Adjacent(this int[,] grid, int x, int y, int compare, bool allowDiagonal = false)
        {
            if (y > 0 && grid[x, y - 1] <= compare)
            {
                yield return new Point(x, y - 1);
            }
            if (y < grid.GetLength(1) - 1 && grid[x, y + 1] <= compare)
            {
                yield return new Point(x, y + 1);
            }
            if (x > 0 && grid[x - 1, y] <= compare)
            {
                yield return new Point(x - 1, y);
            }
            if (x < grid.GetLength(0) - 1 && grid[x + 1, y] <= compare)
            {
                yield return new Point(x + 1, y);
            }

            if (allowDiagonal)
            {
                if (x > 0 && y > 0 && grid[x - 1, y - 1] <= compare)
                {
                    yield return new Point(x - 1, y - 1);
                }
                if (x > 0 && y < grid.GetLength(1) - 1 && grid[x - 1, y + 1] <= compare)
                {
                    yield return new Point(x - 1, y + 1);
                }
                if (x < grid.GetLength(0) - 1 && y > 0 && grid[x + 1, y - 1] <= compare)
                {
                    yield return new Point(x + 1, y - 1);
                }
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && grid[x + 1, y + 1] <= compare)
                {
                    yield return new Point(x + 1, y + 1);
                }
            }
        }
    }
}
