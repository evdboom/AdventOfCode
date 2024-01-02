namespace AdventOfCode2023.Days.Day23Group
{
    public class PriorityInverter : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }
}
