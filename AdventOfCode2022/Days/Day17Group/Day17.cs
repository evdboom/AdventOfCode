using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day17Group;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day17 : Day
    {
        private readonly IScreenWriter _writer;
        public Day17(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        private readonly Dictionary<int, Func<Rock>> _rocks = new()
        {
            { 0, () => new Rock(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0)) },
            { 1, () => new Rock(new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 2)) },
            { 2, () => new Rock(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(2, 1), new Point(2, 2)) },
            { 3, () => new Rock(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3)) },
            { 4, () => new Rock(new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1)) }
        };



        public override int DayNumber => 17;
        protected override long ProcessPartOne(string[] input)
        {
            //var rocks = new List<Rock>() { new Rock(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0), new Point(6, 0)) { StopsAt = -1 } };
            //int actionCounter = 0;
            //for (int i = 0; i < 2022; i++)
            //{
            //    DropRock(input[0], i, rocks, ref actionCounter);
            //}

            //return rocks
            //    .Max(r => r.StopsAt + r.Height);            
            var columns = new Dictionary<int, long>()
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
            };
            int actionCounter = 0;
            for (long i = 0; i < 2022; i++)
            {
                DropRock(input[0], i, columns, ref actionCounter);
            }

            return columns
                .Max(c => c.Value);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var columns = new Dictionary<int, long>()
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
            };
            var checks = new Dictionary<int, int>();            
            var last = 0L;
            int actionCounter = 0;
            var max = 1000000000000;
            var i = 0L;            
            while(i < max)            
            {
                if (i > 2022 && i % 5 == 0)
                {
                    var current = columns.Max(c => c.Value);
                    var delta = (int)(current - last);
                    if (!checks.Any() && last == 0L)
                    {
                        last = current;
                    }
                    else if (checks.TryGetValue(delta, out int value))
                    {
                        checks[delta] = value + 1;
                    }
                    else
                    {
                        checks[delta] = 1;
                    }

                    if (checks.Any() && checks.All(c => c.Value % 2 == 0))
                    {
                        var todo = max - i;
                        var times = checks.Sum(c => c.Value / 2) * 5;
                        var increase = checks.Sum(c => c.Key * (c.Value / 2));

                        var steps = todo / times;
                        for (int j = 0; j < 7; j++)
                        {
                            columns[j] = steps * increase;
                        }
                        i += steps * times;
                    }
                    last = current;
                }
                
                DropRock(input[0], i, columns, ref actionCounter);
                                
                i++;
            }

            return columns
                .Max(c => c.Value);
        }

        private void DropRock(string actions, long i, Dictionary<int, long> columns, ref int actionCounter)
        {
            var rock = _rocks[(int)(i % 5)].Invoke();
            rock.Left = 2;
            var height = 3 + columns.Max(c => c.Value);

            var canFall = true;
            while (canFall)
            {
                switch (actions[actionCounter % actions.Length])
                {
                    case '<':
                        if (!columns.Any(c => HitRock(c, rock, height, -1, 0)))
                        {
                            rock.Left = Math.Max(0, rock.Left - 1);
                        }
                        break;
                    case '>':
                        if (!columns.Any(c => HitRock(c, rock, height, 1, 0)))
                        {
                            rock.Left = Math.Min(7 - rock.Width, rock.Left + 1);
                        }
                        break;
                }
                actionCounter = (actionCounter + 1) % actions.Length;

                if (columns.Any(c => HitRock(c, rock, height, 0, -1)))
                {
                    canFall = false;
                    SetColumns(columns, rock, height);
                }
                else
                {
                    height--;
                }
            }
        }

        private void SetColumns(Dictionary<int, long> columns, Rock rock, long height)
        {
            foreach(var point in rock.Points)
            {
                columns[rock.Left + point.X] = Math.Max(columns[rock.Left + point.X], height + point.Y + 1);
            }
        }

        private bool HitRock(KeyValuePair<int, long> column, Rock rock, long height, int difX, int difY)
        {
            foreach(var point in rock.Absolute.Where(p => p.X + difX == column.Key))
            {
                if (height + point.Y + difY < column.Value)
                {
                    return true;
                }
            }
            return false;           
        }

        private void DropRock(string actions, long i, List<Rock> rocks, ref int actionCounter)
        {
            var rock = _rocks[(int)(i % 5)].Invoke();
            rock.Left = 2;
            rock.StopsAt = 3 + rocks.Max(r => r.StopsAt + r.Height);

            var canFall = true;
            while (canFall)
            {
                switch (actions[actionCounter % actions.Length])
                {
                    case '<':
                        if (!rocks.Any(r => HitRock(r, rock, -1, 0)))
                        {
                            rock.Left = Math.Max(0, rock.Left - 1);
                        }                        
                        break;
                    case '>':
                        if (!rocks.Any(r => HitRock(r, rock, 1, 0)))
                        {
                            rock.Left = Math.Min(7 - rock.Width, rock.Left + 1);
                        }                        
                        break;
                }
                actionCounter++;
                if (rocks.Any(r => HitRock(r, rock, 0, -1)))
                {
                    canFall = false;
                    rocks.Add(rock);
                }
                else
                {
                    rock.StopsAt--;
                }
            }
        }

        private bool HitRock(Rock dropped, Rock rock, int difX, int difY)
        {
            var afterMove = rock.Absolute
                .Select(point => new Point(point.X + difX, point.Y + difY));
            return dropped.Absolute
                .Any(point => afterMove.Contains(point));
        }
    }
}
