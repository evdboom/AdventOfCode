using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2022.Days.Day20Group;

namespace AdventOfCode2022.Days
{
    public class Day20 : Day
    {
        public Day20(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 20;
        protected override long ProcessPartOne(string[] input)
        {
            var list = input
                .Select(line => new MixPart { Value = int.Parse(line) })
                .ToList();
            while (list.FirstOrDefault(p => p.TimesMoved == 0) is MixPart part)
            {
                if (part.Value == 0)
                {
                    part.TimesMoved++;
                    continue;
                }
                var index = list.IndexOf(part);
                var newIndex = index + (int)(part.Value % (list.Count - 1));
                if (newIndex <= 0)
                {
                    newIndex = list.Count - 1 + newIndex;
                }
                else if (newIndex >= list.Count)
                {
                    newIndex -= list.Count - 1;
                }
                list.RemoveAt(index);
                if (newIndex == list.Count)
                {
                    list.Add(part);
                }
                else
                {
                    list.Insert(newIndex, part);
                }
                part.TimesMoved++;
            }

            var zero = list.First(part => part.Value == 0);
            var zeroIndex = list.IndexOf(zero);

            var first = list[(zeroIndex + 1000) % list.Count];
            var second = list[(zeroIndex + 2000) % list.Count];
            var third = list[(zeroIndex + 3000) % list.Count];

            return first.Value + second.Value + third.Value;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var list = input
                .Select((line, index) => new MixPart { Value = int.Parse(line) * 811589153L, OriginalIndex = index })
                .ToList();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    var part = list.First(p => p.OriginalIndex == j);

                    if (part.Value == 0)
                    {
                        part.TimesMoved++;
                        continue;
                    }
                    var index = list.IndexOf(part);
                    var newIndex = index + (int)(part.Value % (list.Count - 1));
                    if (newIndex <= 0)
                    {
                        newIndex = list.Count - 1 + newIndex;
                    }
                    else if (newIndex >= list.Count)
                    {
                        newIndex -= list.Count - 1;
                    }
                    list.RemoveAt(index);
                    if (newIndex == list.Count)
                    {
                        list.Add(part);
                    }
                    else
                    {
                        list.Insert(newIndex, part);
                    }
                    part.TimesMoved++;
                }

            }

            var zero = list.First(part => part.Value == 0);
            var zeroIndex = list.IndexOf(zero);

            var first = list[(zeroIndex + 1000) % list.Count];
            var second = list[(zeroIndex + 2000) % list.Count];
            var third = list[(zeroIndex + 3000) % list.Count];

            return first.Value + second.Value + third.Value;

        }
    }
}