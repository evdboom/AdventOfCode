using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Extensions;
using AdventOfCode.Shared.Services;
using AdventOfCode2023.Days.Day15Group;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace AdventOfCode2023.Days
{
    public class Day15 : Day
    {
        public Day15(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 15;

        protected override long ProcessPartOne(string[] input)
        {
            return string
                .Join(string.Empty, input)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(GetHash)
                .Sum();            
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var boxes = new Dictionary<long, List<Lens>>();
            var entries = string
                .Join(string.Empty, input)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach(var entry in entries)
            {
                var parts = entry.Split('=');
                if (parts.Length == 2)
                {
                    var newLens = new Lens(parts[0], int.Parse(parts[1]));
                    var box = GetHash(newLens.Label);
                    if (boxes.TryGetValue(box, out var lenses))
                    {
                        if (lenses.SingleOrDefault(lens => lens.Label == newLens.Label) is Lens lens)
                        {
                            var index = lenses.IndexOf(lens);
                            lenses.Remove(lens);
                            lenses.Insert(index, newLens);
                        }
                        else
                        {
                            lenses.Add(newLens);
                        }
                    }
                    else
                    {
                        boxes[box] = [newLens];
                    }
                }
                else
                {
                    var label = entry.Replace("-", string.Empty);
                    var box = GetHash(label);
                    if (boxes.TryGetValue(box, out var lenses))
                    {
                        if (lenses.SingleOrDefault(lens => lens.Label == label) is Lens lens)
                        {
                            lenses.Remove(lens);
                        }
                    }
                }
            }

            return boxes
                .Select(box => box.Value
                    .Select((lens, index) => (box.Key + 1) * (index + 1) * lens.FocalLength)
                    .Sum())
                .Sum();                
        }

        private long GetHash(string entry)
        {
            return entry
                .Aggregate(0, GetHash);
        }

        private int GetHash(int currentHash, char value)
        {
            currentHash += value;
            currentHash *= 17;
            currentHash %= 256;

            return currentHash;
        }
    }
}