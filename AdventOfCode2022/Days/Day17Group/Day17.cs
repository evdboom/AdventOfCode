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
            var state = new State();
            var height = 0L;
            for (long i = 0; i < 2022; i++)
            {
                var (newState, newHeight) = DropRock(actions, state, height);
                height = newHeight;
                state = newState;
            }

            return height;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var actions = input[0].Trim();
            var state = new State();
            var height = 0L;
            var toAdd = 1000000000000L;
            var states = new Dictionary<State, (long ToAdd, long Height)>();
            while (toAdd > 0)
            {
                if (states.TryGetValue(state, out (long ToAdd, long Height) values))
                {
                    long difTicks = values.ToAdd - toAdd;
                    long difHeight = height - values.Height;
                    long addedHeight = difHeight * (toAdd / difTicks);
                    toAdd %= difTicks;
                    height += addedHeight;
                }
                else
                {
                    states[state] = (toAdd, height);
                }
                var (newState, newHeight) = DropRock(actions, state, height);
                height = newHeight;
                state = newState;

                toAdd--;
            }

            return height;

        }

        private (State State, long NewHeight) DropRock(string actions, State state, long oldHeight)
        {
            var rock = _rocks[state.Rock].Invoke();
            rock.Left = 2;
            var height = 3 + oldHeight;
            var actionCounter = state.Action;
            while (true)
            {
                switch (actions[actionCounter])
                {
                    case '<':
                        if (!HitRock(state, rock, height, oldHeight, -1, 0))
                        {
                            rock.Left = Math.Max(0, rock.Left - 1);
                        }
                        break;
                    case '>':
                        if (!HitRock(state, rock, height, oldHeight, 1, 0))
                        {
                            rock.Left = Math.Min(7 - rock.Width, rock.Left + 1);
                        }
                        break;
                }
                actionCounter = (actionCounter + 1) % actions.Length;

                if (HitRock(state, rock, height, oldHeight, 0, -1))
                {
                    var newHeight = Math.Max(height + rock.Height, oldHeight);
                    var newState = state with
                    {
                        Action = actionCounter,
                        Rock = (state.Rock + 1) % 5,
                    };
                    SetColumns(newState, rock, height, newHeight, oldHeight);
                    return (newState, newHeight);
                }
                else
                {
                    height--;
                }
            }
        }

        private void SetColumns(State state, Rock rock, long height, long newHeight, long oldHeight)
        {
            var heightDif = (int)(newHeight - oldHeight);
            if (heightDif > 0)
            {
                var add = new string('.', heightDif);
                foreach(var column in state.GetColumns())
                {
                    var newValue = $"{add}{column.Column}";
                    var length = Math.Min(100, newValue.Length);
                    state.SetColumn(column.Index, newValue[..length]);
                }
            }

            foreach(var point in rock.Points)
            {
                var column = point.X + rock.Left;
                var row = newHeight - (height + point.Y + 1);
                var current = state.GetColumn(column).ToCharArray();                
                current[row] = '#';
                state.SetColumn(column, new string(current));
            }
        }

        private bool HitRock(State state, Rock rock, long height, long oldHeight, int difX, int difY)
        {           
            foreach(var point in rock.Points)
            {
                var column = rock.Left + point.X + difX;
                var row = oldHeight - (height + point.Y + difY);
                if (state.PointFromTop(column, (int)row) == '#')
                {
                    return true;
                }
            }
            return false;           
        }
    }
}
