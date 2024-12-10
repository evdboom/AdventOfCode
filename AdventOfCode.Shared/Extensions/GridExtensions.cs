using AdventOfCode.Shared.Enums;
using AdventOfCode.Shared.Grid;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode.Shared.Extensions
{
    public static class GridExtensions
    {
        public static Grid<int> ToIntGrid(this string[] input)
        {
            return ToGrid(input, (c) => int.Parse($"{c}"));
        }

        public static Grid<T> ToGrid<T>(this string[] input, Func<char, T> parse, Func<char, bool> findStart, out Point? start)
        {
            var grid = new Grid<T>(input[0].Length, input.Length);
            start = null;
            foreach (var cell in grid)
            {
                var value = input[cell.Point.Y][cell.Point.X];
                grid[cell.Point] = parse(value);
                if (findStart(value))
                {
                    start = cell.Point;
                }
            }                
            return grid;
        }

        public static Grid<char> ToGrid(this string[] input)
        {
            return ToGrid(input, c => c);
        }

        public static Grid<T> ToGrid<T>(this string[] input, Func<char, T> parse)
        {
            return ToGrid(input, parse, _ => false, out _);
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

        public static void Replace<T>(this Grid<T> grid, Func<GridCell<T>, T> replace)
        {
            foreach (var cell in grid)
            {
                grid[cell.Point] = replace(cell);
            }
        }

        public static bool TryGetPointInDirection<T>(this Grid<T> grid, Point point, Directions direction, [NotNullWhen(true)] out Point? target)
        {
            return TryGetPointInDirection(grid, point, direction, _ => true, out target);
        }

        public static bool TryGetPointInDirection<T>(this Grid<T> grid, Point point, Directions direction, Func<(T Origin, T Target), bool> compare, [NotNullWhen(true)] out Point? target)
        {
            return TryGetPointInDirection(grid, point.X, point.Y, direction, compare, out target);
        }

        public static bool TryGetPointInDirection<T>(this Grid<T> grid, int x, int y, Directions direction, [NotNullWhen(true)] out Point? target)
        {
            return TryGetPointInDirection(grid, x, y, direction, _ => true, out target);
        }

        public static bool TryGetPointInDirection<T>(this Grid<T> grid, int x, int y, Directions direction, Func<(T Origin, T Target), bool> compare, [NotNullWhen(true)] out Point? target)
        {
            target = direction switch
            {
                Directions.Up => new Point(x, y - 1),
                Directions.Down => new Point(x, y + 1),
                Directions.Left => new Point(x - 1, y),
                Directions.Right => new Point(x + 1, y),
                Directions.UpLeft => new Point(x - 1, y - 1),
                Directions.UpRight => new Point(x + 1, y - 1),
                Directions.DownLeft => new Point(x - 1, y + 1),
                Directions.DownRight => new Point(x + 1, y + 1),
                _ => throw new ArgumentException("Invalid Direction", nameof(direction)),
            };
            var valid = target.Value.X >= 0 &&
                        target.Value.X <= grid.MaxX &&
                        target.Value.Y >= 0 &&
                        target.Value.Y <= grid.MaxY &&
                        compare((grid[x, y], grid[target.Value]));

            if (!valid)
            {
                target = null;
            }

            return valid;
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, Point point, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, point.X, point.Y, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, int x, int y, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, x, y, _ => true, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, Point point, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, point.X, point.Y, compare, allowDiagonal);
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, int x, int y, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
           return allowDiagonal
                ? AdjecentWithDirection(grid, x, y, compare, Directions.All)
                : AdjecentWithDirection(grid, x, y, compare, Directions.Cardinal);
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, Point point, Func<(T Origin, T Target), bool> compare, Directions directions)
        {
            return AdjecentWithDirection(grid, point.X, point.Y, compare, directions);
        }

        public static IEnumerable<(Point Point, Directions Direction, T Value)> AdjecentWithDirection<T>(this Grid<T> grid, int x, int y, Func<(T Origin, T Target), bool> compare, Directions directions)
        {
            if ((directions & Directions.Up) > 0 && y > 0)
            {
                var point = new Point(x, y - 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.Up, value);
                }
            }
            if ((directions & Directions.Down) > 0 && y < grid.MaxY)
            {
                var point = new Point(x, y + 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.Down, value);
                }
            }
            if ((directions & Directions.Left) > 0 && x > 0)
            {
                var point = new Point(x - 1, y);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.Left, value);
                }
            }
            if ((directions & Directions.Right) > 0 && x < grid.MaxX)
            {
                var point = new Point(x + 1, y);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.Right, value);
                }
            }
            if ((directions & Directions.UpLeft) > 0 && x > 0 && y > 0)
            {
                var point = new Point(x - 1, y - 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.UpLeft, value);
                }
            }
            if ((directions & Directions.DownLeft) > 0 && x > 0 && y < grid.MaxY)
            {
                var point = new Point(x - 1, y + 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.DownLeft, value);
                }

            }
            if ((directions & Directions.UpRight) > 0 && x < grid.MaxX && y > 0)
            {
                var point = new Point(x + 1, y - 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.UpRight, value);
                }
            }
            if ((directions & Directions.DownRight) > 0 && x < grid.MaxX && y < grid.MaxY)
            {
                var point = new Point(x + 1, y + 1);
                var value = grid[point];
                if (compare((grid[x, y], value)))
                {
                    yield return (point, Directions.DownRight, value);
                }
            }
        }

        public static IEnumerable<Point> Adjacent<T>(this Grid<T> grid, Point point, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
            return Adjacent(grid, point.X, point.Y, compare, allowDiagonal);
        }

        public static IEnumerable<Point> Adjacent<T>(this Grid<T> grid, Point point, bool allowDiagonal = false)
        {
            return Adjacent(grid, point.X, point.Y, allowDiagonal);
        }

        public static IEnumerable<Point> Adjacent<T>(this Grid<T> grid, int x, int y, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, x, y, _ => true, allowDiagonal)
                .Select(adjecent => adjecent.Point);
        }

        public static IEnumerable<Point> Adjacent<T>(this Grid<T> grid, int x, int y, Func<(T Origin, T Target), bool> compare, bool allowDiagonal = false)
        {
            return AdjecentWithDirection(grid, x, y, compare, allowDiagonal)
                .Select(adjecent => adjecent.Point);
        }

        public static IEnumerable<Point> Adjacent(this Grid<int> grid, int x, int y, int compare, bool allowDiagonal = false)
        {
            return Adjacent(grid, x, y, (adjecent) => adjecent.Target <= compare, allowDiagonal);
        }
    }
}
