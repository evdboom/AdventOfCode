using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2024.Days
{
    public class Day13(IFileImporter importer) : Day(importer)
    {

        public override int DayNumber => 13;

        protected override long ProcessPartOne(string[] input)
        {
            return GetGames(input)
                .Aggregate(0L, (total, game) => total + (IsValidGame(game.Prize, game.ButtonA, game.ButtonB, 100, out var cost) ? cost : 0));            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var offset = 10000000000000D;
            return GetGames(input)
                .Aggregate(0L, (total, game) => total + (IsValidGame((game.Prize.X + offset, game.Prize.Y + offset), game.ButtonA, game.ButtonB, null, out var cost) ? cost : 0));
        }

        private bool IsValidGame((double X, double Y) prize, (double dX, double dY) buttonA, (double dX, double dY) buttonB, int? maxPresses, out long cost)
        {
            cost = 0;
            // a * dXA + b * dXB = X
            // a * dYA + b * dYB = Y

            // from first:
            // a = (X - b * dXB) / dXA

            // substitute a in second:
            // (X - b * dXB) / dXA * dYA + b * dYB = Y
            // X * dYA / dXA - b * dXB * dYA / dXA  + b * dYB = Y
            // b * dYB - b * dXB * dYA / dXA = Y - X * dYA / dXA
            // b * (dYB - dXB * dYA / dXA) = Y - X * dYA / dXA
            // b = (Y - X * dYA / dXA) / (dYB - dXB * dYA / dXA)

            // after calculating b, calculate a
            // a = (X - b * dXB) / dXA

            var b = (prize.Y - prize.X * buttonA.dY / buttonA.dX) / (buttonB.dY - buttonB.dX * buttonA.dY / buttonA.dX);
            var a = (prize.X - b * buttonB.dX) / buttonA.dX;

            // as we use floating point numbers, we need to round them to integers (fixing floating point errors)
            var aPressed = (long)Math.Round(a);
            var bPressed = (long)Math.Round(b);

            if (aPressed < 0 || bPressed < 0 || maxPresses.HasValue && (aPressed > maxPresses || bPressed > maxPresses))
            {               
                return false;
            }

            // check if the calculated values are correct
            if ((long)buttonA.dX * aPressed + (long)buttonB.dX * bPressed == prize.X &&
                (long)buttonA.dY * aPressed + (long)buttonB.dY * bPressed == prize.Y)
            {
                cost = aPressed * 3 + bPressed;
            }

            return cost > 0;
        }

        private IEnumerable<((double X, double Y) Prize, (double dX, double dY) ButtonA, (double dX, double dY) ButtonB)> GetGames(string[] input)
        {
            int index = 0;
            while (index < input.Length)
            {
                var buttonA = ParseButton(input[index]);
                var buttonB = ParseButton(input[index + 1]);
                var prize = ParsePrize(input[index + 2]);
                yield return (prize, buttonA, buttonB);
                index += 4;
            }
        }

        private (double X, double Y) ParseButton(string input)
        {
            var parts = input[9..].Split(", ");
            return (double.Parse(parts[0][2..]), double.Parse(parts[1][2..]));
        }

        private (double X, double Y) ParsePrize(string input)
        {
            var parts = input[7..].Split(", ");
            return (double.Parse(parts[0][2..]), double.Parse(parts[1][2..]));
        }
    }
}
