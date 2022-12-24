namespace AdventOfCode2022.Days.Day24Group
{
    public class StateComparer : IComparer<State>
    {
        public int Compare(State? x, State? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            if (x.Distance == y.Distance)
            {
                return x.Steps - y.Steps;
            }
            else
            {
                return x.Distance - y.Distance;
            }
        }
    }
}
