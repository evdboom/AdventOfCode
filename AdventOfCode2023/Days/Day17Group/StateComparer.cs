namespace AdventOfCode2023.Days.Day17Group
{
    public class StateComparer : IComparer<State>
    {
        public int Compare(State? x, State? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return x.HeatLoss == y.HeatLoss
                ? x.Distance - y.Distance
                : x.HeatLoss - y.HeatLoss;
        }
    }
}
