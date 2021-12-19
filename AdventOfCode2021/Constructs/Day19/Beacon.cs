namespace AdventOfCode2021.Constructs.Day19
{
    public class Beacon : Coordinate
    {
        public List<Beacon> Map { get; set; }

        public Beacon()
        {
            Map = new List<Beacon>();
        }

        public Beacon(int x, int y, int z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
