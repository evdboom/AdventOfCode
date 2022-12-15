using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day15 : Day
    {
        public int HighestCoordinates { get; set; } = 4000000;
        public int RequestedRow { get; set; } = 2000000;

        public Day15(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 15;
        protected override long ProcessPartOne(string[] input)
        {
            var sensors = GetSensors(input);

            var result = new Dictionary<int, bool>();
            foreach (var pair in sensors)
            {
                var distance = Math.Abs(pair.Key.X - pair.Value.X) + Math.Abs(pair.Key.Y - pair.Value.Y);
                var yDistanceToRow = Math.Abs(pair.Key.Y - RequestedRow);
                if (yDistanceToRow > distance)
                {
                    continue;
                }

                if (pair.Key.Y == RequestedRow)
                {
                    result[pair.Key.X] = false;
                }
                if (pair.Value.Y == RequestedRow)
                {
                    result[pair.Value.X] = false;
                }

                for (int i = pair.Key.X - distance + yDistanceToRow; i <= pair.Key.X + distance - yDistanceToRow; i++)
                {
                    if (!result.ContainsKey(i))
                    {
                        result[i] = true;
                    }
                }

            }

            return result.Values
                .Where(x => x)
                .Count();           
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var sensors = GetSensors(input);

            for (int j = 0; j <= HighestCoordinates; j++)
            {
                var ranges = new List<Range>() { new Range(new Index(0), new Index(HighestCoordinates)) };
                foreach (var pair in sensors)
                {
                    var distance = Math.Abs(pair.Key.X - pair.Value.X) + Math.Abs(pair.Key.Y - pair.Value.Y);
                    var yDistanceToRow = Math.Abs(pair.Key.Y - j);
                    if (yDistanceToRow > distance)
                    {
                        continue;
                    }
                    var xMin = Math.Max(0, pair.Key.X - distance + yDistanceToRow);
                    var xMax = Math.Max(0, pair.Key.X + distance - yDistanceToRow);

                    var newRanges = new List<Range>();
                    var removeRanges = new List<Range>();
                    foreach(var range in ranges)
                    {
                        if (xMax >= range.Start.Value && xMin <= range.End.Value)
                        {
                            removeRanges.Add(range);
                            if (xMin <= range.Start.Value && xMax < range.End.Value)
                            {
                                newRanges.Add(new Range(new Index(xMax + 1), range.End));
                            }
                            else if (xMax >= range.End.Value && xMin > range.Start.Value)
                            {
                                newRanges.Add(new Range(range.Start, new Index(xMin - 1)));
                            }
                            else if (xMax < range.End.Value && xMin > range.Start.Value)
                            {
                                newRanges.Add(new Range(range.Start, new Index(xMin - 1)));
                                newRanges.Add(new Range(new Index(xMax + 1), range.End));
                            }
                        }
                    }
                    if (removeRanges.Any())
                    {
                        ranges = ranges
                            .Except(removeRanges)
                            .ToList();
                    }
                    if (newRanges.Any())
                    {
                        ranges.AddRange(newRanges);
                    }

                }
                if (ranges.Count == 1 && ranges[0].Start.Value == ranges[0].End.Value)
                {
                    return ranges[0].Start.Value * 4000000L + j;
                }
            }
            
            throw new InvalidOperationException("No point found");
        }

        private bool NoHit(long i, long j, Point sensor, Point beacon)
        {
            var distance = Math.Abs(sensor.X - beacon.X) + Math.Abs(sensor.Y - beacon.Y);
            var distanceToPoint = Math.Abs(sensor.X - i) + Math.Abs(sensor.Y - j);

            return distanceToPoint > distance;
        }

        private Dictionary<Point, Point> GetSensors(string[] input)
        {
            var sensors = new Dictionary<Point, Point>();

            foreach (var line in input)
            {
                var mainParts = line.Split(": closest beacon is at x=");
                var sensorPart = mainParts[0].Replace("Sensor at x=", string.Empty);
                var sensor = sensorPart
                    .Split(", y=")
                    .Select(int.Parse)
                    .ToArray();
                var beacon = mainParts[1]
                    .Split(", y=")
                    .Select(int.Parse)
                    .ToArray();
                var sensorPoint = new Point(sensor[0], sensor[1]);
                sensors[sensorPoint] = new Point(beacon[0], beacon[1]);
            }

            return sensors;
        }
    }
}
