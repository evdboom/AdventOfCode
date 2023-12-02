using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;

namespace AdventOfCode2023.Days
{
    public class Day02 : Day
    {
        private const int MaxRed = 12;
        private const int MaxGreen = 13;
        private const int MaxBlue = 14;

        public Day02(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 2;

        protected override long ProcessPartOne(string[] input)
        {
            return input
                .Aggregate(0L, (previous, current) =>
                {
                    var parts = current.Split(":");
                    var id = int.Parse(parts[0].Replace("Game ", string.Empty));
                    var rounds = parts[1].Split(";");
                    if (rounds.All(IsValid))
                    {
                        previous += id;
                    }

                    return previous;
                });
        }

        private bool IsValid(string round)
        {
            var items = round.Split(",");
            foreach (var item in items)
            {
                if (int.TryParse(item.Trim().Replace(" red", string.Empty), out var red))
                {
                    if (red > MaxRed)
                    {
                        return false;
                    }
                }
                else if (int.TryParse(item.Trim().Replace(" blue", string.Empty), out var blue))
                {
                    if (blue > MaxBlue)
                    {
                        return false;
                    }
                }
                else if (int.TryParse(item.Trim().Replace(" green", string.Empty), out var green))
                {
                    if (green > MaxGreen)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            return input
                .Aggregate(0L, (previous, current) =>
                {
                    var parts = current.Split(":");
                    var rounds = parts[1].Split(";");
                    var highest = rounds
                        .Aggregate((Red: 0, Green: 0, Blue: 0), (highest, round) =>
                        {
                            var items = round.Split(",");
                            foreach (var item in items)
                            {
                                if (int.TryParse(item.Trim().Replace(" red", string.Empty), out var red))
                                {
                                    if (red > highest.Red)
                                    {
                                        highest.Red = red;
                                    }
                                }
                                else if (int.TryParse(item.Trim().Replace(" blue", string.Empty), out var blue))
                                {
                                    if (blue > highest.Blue)
                                    {
                                        highest.Blue = blue;
                                    }
                                }
                                else if (int.TryParse(item.Trim().Replace(" green", string.Empty), out var green))
                                {
                                    if (green > highest.Green)
                                    {
                                        highest.Green = green;
                                    }
                                }
                            }

                            return highest;
                        });

                    return previous + (highest.Red * highest.Green * highest.Blue);
                });      
        }
    }
}
