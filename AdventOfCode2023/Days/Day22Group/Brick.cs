using System.Linq.Expressions;

namespace AdventOfCode2023.Days.Day22Group
{
    public class Brick
    {
        public int Id { get; set; }
        public int[] Start { get; set; } = new int[3];
        public int[] End { get; set; } = new int[3];
        public List<Brick> Supports { get; set; } = [];
        public List<Brick> SupportedBy { get; set; } = [];

        public int Bottom => Math.Min(Start[2], End[2]);
        public int Top => Math.Max(Start[2], End[2]);
        public int Left => Math.Min(Start[0], End[0]);
        public int Right => Math.Max(Start[0], End[0]);
        public int North => Math.Min(Start[1], End[1]);
        public int South => Math.Max(Start[1], End[1]);

        public void Fall()
        {
            if (Bottom == 1)
            {
                return;
            }

            Start[2]--;
            End[2]--;
        }

        public IEnumerable<int> WouldFallIfRemoved => !SupportedBy.Any() ? [-1] : SupportedBy.Select(by => by.Id);        
    }
}
