namespace AdventOfCode2023.Days.Day24Group
{
    public class HailStone
    {
        public int Id { get; set; }
        public long[] Position { get; set; } = Array.Empty<long>();
        public long[] Velocity { get; set; } = Array.Empty<long>();

        public double DY => (double)Velocity[1] / Velocity[0];
        public double Y0 => Position[1] - DY * Position[0];        
    }
}
