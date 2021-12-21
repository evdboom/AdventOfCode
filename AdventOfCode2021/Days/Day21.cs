using AdventOfCode2021.Constructs.Day21;
using AdventOfCode2021.Services;

namespace AdventOfCode2021.Days
{
    public class Day21 : Day
    {
        private const string PlayerSplit = " starting position: ";
        private readonly Dictionary<int, int> _options = new Dictionary<int, int>
        {
            { 3, 1 },
            { 4, 3 },
            { 5, 6 },
            { 6, 7 },
            { 7, 6 },
            { 8, 3 },
            { 9, 1 }
        };

        public Day21(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 21;

        protected override long ProcessPartOne(string[] input)
        {
            var (players1, players2) = GetPlayers(input);

            int count = 0;

            while (players1.Any() && players2.Any())
            {             
                count += 3;
                var next = count % 1000;
                var turn = (next == 0 ? 1000 : next) * 3 - 3;

                players1 = GetNextRound(players1, out List<long> winningWeights, new Dictionary<int, int> { { turn, 1 } }, 1000);                
                if (winningWeights.Any())
                {
                    return players2.Single().Value.Scores.Single().Key * count;
                }

                count += 3;
                turn = (count % 1000) * 3 - 3;

                players2 = GetNextRound(players2, out winningWeights, new Dictionary<int, int> { { turn, 1 } }, 1000);
                if (winningWeights.Any())
                {
                    return players1.Single().Value.Scores.Single().Key * count;
                }
            }

            throw new InvalidOperationException("No winner found");
        }
      
        protected override long ProcessPartTwo(string[] input)
        {
            var (players1, players2) = GetPlayers(input);

            long player1Wins = 0;
            long player2Wins = 0;

            while (players1.Any() && players2.Any())
            {
                players1 = GetNextRound(players1, out List<long> winningWeights, _options, 21);
                player1Wins += winningWeights.Sum(w => w * players2.Select(p => p.Value.GetWeight()).Sum());
                players2 = GetNextRound(players2, out winningWeights, _options, 21);
                player2Wins += winningWeights.Sum(w => w * players1.Select(p => p.Value.GetWeight()).Sum());
            }

            return Math.Max(player1Wins, player2Wins);
        }

        private (Dictionary<int,Player> Players1, Dictionary<int,Player> Players2) GetPlayers(string[] input)
        {
            
            var playerPositions = input
                .Select(i => i.Split(PlayerSplit))
                .Select(p => int.Parse(p[1]))
                .ToArray();

            var players1 = new Dictionary<int, Player>();            
            players1[playerPositions[0]] = new Player(addScore: true);
            var players2 = new Dictionary<int, Player>();            
            players2[playerPositions[1]] = new Player(addScore: true);

            return (players1, players2);
        }

        private Dictionary<int, Player> GetNextRound(Dictionary<int, Player> players, out List<long> winningWeights, Dictionary<int, int> options, int target)
        {
            var nextRound = new Dictionary<int, Player>();
            winningWeights = new();
            foreach (var player in players)
            {
                foreach (var score in player.Value.Scores)
                {
                    foreach (var option in options)
                    {
                        var nextPosition = (player.Key + option.Key) % 10;
                        nextPosition = nextPosition == 0 ? 10 : nextPosition;
                        var newScore = score.Key + nextPosition;
                        var newWeight = score.Value.Weight * option.Value;

                        if (newScore >= target)
                        {
                            winningWeights.Add(newWeight);
                        }
                        else
                        {
                            if (!nextRound.TryGetValue(nextPosition, out Player? next))
                            {
                                next = new Player();
                                nextRound[nextPosition] = next;
                            }
                            if (!next.Scores.TryGetValue(newScore, out Score? nextScore))
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
    }
}
