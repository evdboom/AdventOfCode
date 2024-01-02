using System.Drawing;

namespace AdventOfCode2023.Days.Day23Group
{
    public record Intersection
    {
        public Point Location { get; set; }
        public Intersection? North { get; set; }
        public int? NorthDistance { get; set; }
        public Intersection? East { get; set; }
        public int? EastDistance { get; set; }
        public Intersection? South { get; set; }
        public int? SouthDistance { get; set; }
        public Intersection? West { get; set; }
        public int? WestDistance { get; set; }        

        public IEnumerable<(Intersection Intersection, int Distance)> Connections()
        {
            if (North is not null && NorthDistance.HasValue)
            {
                yield return (North, NorthDistance.Value);
            }
            if (East is not null && EastDistance.HasValue)
            {
                yield return (East, EastDistance.Value);
            }
            if (South is not null && SouthDistance.HasValue)
            {
                yield return (South, SouthDistance.Value);
            }
            if (West is not null && WestDistance.HasValue)
            {
                yield return (West, WestDistance.Value);
            }
        }
    }
}
