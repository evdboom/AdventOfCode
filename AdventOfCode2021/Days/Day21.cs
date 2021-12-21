using AdventOfCode2021.Constructs.Day21;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day21 : Day
    {
        private const string PlayerSplit = " starting position: ";

        public Day21(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 21;

        protected override long ProcessPartOne(string[] input)
        {
            var players = input
                .Select(i => i.Split(PlayerSplit))
                .ToDictionary(p => p[0], p => (Position: int.Parse(p[1]), Score: 0));
            int count = 0;
            int roll = 0;
            while (players.All(p => p.Value.Score < 1000))
            {
                foreach(var player in players)
                {
                    var score = 0;
                    for (int i = 0; i < 3; i ++)
                    {
                        count++;
                        roll++;
                        if (roll > 1000)
                        {
                            roll = 1;
                        }
                        score += roll;                        
                    }
                    var pos = (player.Value.Position + score) % 10;
                    if (pos == 0)
                    {
                        pos = 10;
                    }
                    players[player.Key] = (pos, player.Value.Score + pos);
                    if (players[player.Key].Score >= 1000)
                    {
                        break;
                    }
                }                
            }

            var other = players.Single(p => p.Value.Score < 1000);
            return other.Value.Score * count;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var playerPositions = input
                .Select(i => i.Split(PlayerSplit))
                .Select(p => int.Parse(p[1]))
                .ToArray();            

            var players1 = new Dictionary<int, Player>();            
            var initial1 = new Player();
            initial1.Scores[0] = new Score();            
            players1[playerPositions[0]] = initial1;
            var players2 = new Dictionary<int, Player>();
            var initial2 = new Player();
            initial2.Scores[0] = new Score();
            players2[playerPositions[1]] = initial2;
            
            long player1Wins = 0;
            long player2Wins = 0;

            while (players1.Any() && players2.Any())
            {
                players1 = GetNextRound(players1, out List<long> winningWeights);
                player1Wins += winningWeights.Sum(w => w * players2.Select(p => p.Value.GetWeight()).Sum());

                players2 = GetNextRound(players2, out winningWeights);
                player2Wins += winningWeights.Sum(w => w * players1.Select(p => p.Value.GetWeight()).Sum());
            }

            return Math.Max(player1Wins, player2Wins);
        }

        private Dictionary<int, Player> GetNextRound(Dictionary<int, Player> players, out List<long> winningWeights)
        {
            var nextRound = new Dictionary<int, Player>();
            winningWeights = new();
            foreach (var player in players)
            {
                foreach (var score in player.Value.Scores)
                {
                    foreach (var option in GetOptions())
                    {
                        var nextPosition = (player.Key + option.Key) % 10;
                        nextPosition = nextPosition == 0 ? 10 : nextPosition;
                        var newScore = score.Key + nextPosition;
                        var newWeight = score.Value.Weight * option.Value;

                        if (newScore >= 21)
                        {
                            winningWeights.Add(newWeight);
                        }
                        else
                        {
                            if (!nextRound.TryGetValue(nextPosition, out Player next))
                            {
                                next = new Player();
                                nextRound[nextPosition] = next;
                            }
                            if (!next.Scores.TryGetValue(newScore, out Score nextScore))
                            {
                                nextScore = new Score { Weight = newWeight };
                                next.Scores[newScore] = nextScore;
                            }
                            else
                            {
                                nextScore.Weight += newWeight;
                            }
                        }
                    }
                }
            }

            return nextRound;
        }
  
        private Dictionary<int, int> GetOptions()
        {
            return new Dictionary<int, int>
            {
                { 3, 1 },
                { 4, 3 },
                { 5, 6 },
                { 6, 7 },
                { 7, 6 },
                { 8, 3 },
                { 9, 1 }
            };
        }
    }
}
