using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Constructs.Day21;

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
            var (player1, player2) = GetPlayers(input);

            int count = 0;

            while (player1.Any() && player2.Any())
            {
                count += 3;
                var next = count % 1000;
                var turn = (next == 0 ? 1000 : next) * 3 - 3;

                player1 = GetNextRound(player1, turn, 1000);
                if (!player1.Any())
                {
                    return player2.Single().Value.Single().Key * count;
                }

                count += 3;
                next = count % 1000;
                turn = (next == 0 ? 1000 : next) * 3 - 3;

                player2 = GetNextRound(player2, turn, 1000);
                if (!player2.Any())
                {
                    return player1.Single().Value.Single().Key * count;
                }
            }

            throw new InvalidOperationException("No winner found");
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var (player1, player2) = GetPlayers(input);

            long player1Wins = 0;
            long player2Wins = 0;

            while (player1.Any() && player2.Any())
            {
                player1 = GetNextRound(player1, _options, 21, out long winningWeight);
                player1Wins += winningWeight * player2.GetWeight();
                player2 = GetNextRound(player2, _options, 21, out winningWeight);
                player2Wins += winningWeight * player1.GetWeight();
            }

            return Math.Max(player1Wins, player2Wins);
        }

        private (Player Player1, Player Player2) GetPlayers(string[] input)
        {
            var playerPositions = input
                .Select(i => i.Split(PlayerSplit))
                .Select(p => int.Parse(p[1]))
                .ToArray();

            var player1 = new Player();
            player1[playerPositions[0]][0] = 1;
            var player2 = new Player();
            player2[playerPositions[1]][0] = 1;

            return (player1, player2);
        }

        private Player GetNextRound(Player player, int turn, int target)
        {
            return GetNextRound(player, new Dictionary<int, int> { { turn, 1 } }, target, out _);
        }

        private Player GetNextRound(Player player, Dictionary<int, int> options, int target, out long winningWeight)
        {
            var nextRound = new Player();
            winningWeight = 0;
            foreach (var scores in player)
            {
                foreach (var score in scores.Value)
                {
                    foreach (var option in options)
                    {
                        var nextPosition = (scores.Key + option.Key) % 10;
                        nextPosition = nextPosition == 0 ? 10 : nextPosition;
                        var newScore = score.Key + nextPosition;
                        var newWeight = score.Value * option.Value;

                        if (newScore >= target)
                        {
                            winningWeight += newWeight;
                        }
                        else
                        {
                            if (!nextRound[nextPosition].ContainsKey(newScore))
                            {
                                nextRound[nextPosition][newScore] = newWeight;
                            }
                            else
                            {
                                nextRound[nextPosition][newScore] += newWeight;
                            }
                        }
                    }
                }
            }

            return nextRound;
        }
    }
}
