namespace AdventOfCode2021.Constructs.Day19
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Coordinate compare)
            {
                return false;
            }
            else
            {
                return X == compare.X && Y == compare.Y && Z == compare.Z;
            }

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
