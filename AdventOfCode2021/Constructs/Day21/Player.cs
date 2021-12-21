namespace AdventOfCode2021.Constructs.Day21
{
    public class Player
    {       
        public Dictionary<int, Score> Scores { get; set; } = new();

        public long GetWeight()
        {
            return Scores.Values.Sum(s => s.Weight);
        }
    }
}
