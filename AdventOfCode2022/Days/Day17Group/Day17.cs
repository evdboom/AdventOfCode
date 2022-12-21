using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day17Group;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day17 : Day
    {
        public Day17(IFileImporter importer) : base(importer)
        {
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
            var actions = input[0].Trim();
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
                var rock = i % 5;
                DropRock(actions, (int)rock, columns, ref actionCounter);
            }

            return columns
                .Max(c => c.Value);
        }

        protected override long ProcessPartTwo(string[] input)
        {
            //return new Tunnel(input[0], 100).AddRocks(1000000000000).Height;

            var actions = input[0].Trim();
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
            var add = 1000000000000L;
            var max = 1000000000000L;
            var states = new Dictionary<State, (long Counter,long Height)>();
            var foundRepetition = false;
            while(add > 0)
            {
                var rock = (max - add) % 5;
                if (!foundRepetition)
                {                    
                    var action = actionCounter;
                    var minHeight = columns.Min(c => c.Value);
                    var maxHeight = columns.Max(c => c.Value);
                    var state = new State
                    {
                        Action = action,
                        Rock = (int)rock,
                        C0 = columns[0] - minHeight,
                        C1 = columns[1] - minHeight,
                        C2 = columns[2] - minHeight,
                        C3 = columns[3] - minHeight,
                        C4 = columns[4] - minHeight,
                        C5 = columns[5] - minHeight,
                        C6 = columns[6] - minHeight,
                    };
                    if (states.TryGetValue(state, out (long Add, long Height) values))
                    {
                        foundRepetition = true;
                        long difTicks = values.Add - add;
                        long difHeight = maxHeight - values.Height;
   
                        long addedHeight = difHeight * (add / difTicks);
                        add %= difTicks;
                        columns[0] += addedHeight;
                        columns[1] += addedHeight;
                        columns[2] += addedHeight;
                        columns[3] += addedHeight;
                        columns[4] += addedHeight;
                        columns[5] += addedHeight;
                        columns[6] += addedHeight;

                    }
                    else
                    {
                        states[state] = (add, maxHeight);
                    }
                }

                DropRock(actions, (int)rock, columns, ref actionCounter);
                add--;
            }

            var result = columns
                .Max(c => c.Value);
            var dif = 1564705882327 - result;
            // works for test set...
            return result;
        }



        private void DropRock(string actions, int rockId, Dictionary<int, long> columns, ref int actionCounter)
        {
            var rock = _rocks[rockId].Invoke();
            rock.Left = 2;
            var height = 3 + columns.Max(c => c.Value);

            var canFall = true;
            while (canFall)
            {
                switch (actions[actionCounter])
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
            foreach(var point in rock.Absolute)
            {
                columns[point.X] = Math.Max(columns[point.X], height + point.Y + 1);
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
    }
}
