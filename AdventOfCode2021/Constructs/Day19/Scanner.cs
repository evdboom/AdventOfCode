namespace AdventOfCode2021.Constructs.Day19
{
    public class Scanner : Coordinate
    {
        public string Name { get; init; }
        public List<Beacon> Beacons { get; set; }

        public bool Unknown { get; private set; }

        public Scanner(string name)
        {
            Unknown = true;
            Name = name;
            Beacons = new();
        }

        internal void SetCoord(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Unknown = false;
        }

        public override string ToString()
        {
            return Unknown 
                ? $"{Name} {nameof(Unknown)}"
                : $"{Name} {base.ToString()}";
        }
    }
}
