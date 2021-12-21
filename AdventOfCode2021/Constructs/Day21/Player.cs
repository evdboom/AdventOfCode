namespace AdventOfCode2021.Constructs.Day21
{
    public class Player
    {       
        public Dictionary<int, Score> Scores { get; set; } = new();
        
        public Player(bool addScore = false)
        {
            if (addScore)
            {
                Scores[0] = new Score();
            }
        }

        public long GetWeight()
        {
            return Scores.Values.Sum(s => s.Weight);
        }
    }
}
