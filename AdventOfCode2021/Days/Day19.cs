using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using AdventOfCode2021.Constructs.Day19;

namespace AdventOfCode2021.Days
{
    public class Day19 : Day
    {
        private const string ScannerLine = "---";
        private const char Split = ',';

        private readonly IScreenWriter _writer;

        public Day19(IFileImporter importer, IScreenWriter writer) : base(importer)
        {
            _writer = writer;
        }

        public override int DayNumber => 19;

        protected override long ProcessPartOne(string[] input)
        {
            _writer.Enable();
            var scanners = GetScanners(input);
            scanners[0].SetCoord(0, 0, 0);
            CreateBeaconMap(scanners[0].Beacons);
            FindScanners(scanners, out List<Beacon> beacons);
            return beacons.Count;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            _writer.Enable();
            var scanners = GetScanners(input);
            scanners[0].SetCoord(0, 0, 0);
            CreateBeaconMap(scanners[0].Beacons);
            FindScanners(scanners, out _);

            return scanners
                .Select(source => scanners
                    .Select(target => Math.Abs(source.X - target.X) + Math.Abs(source.Y - target.Y) + Math.Abs(source.Z - target.Z))
                    .Max())
                .Max();
        }

        private void FindScanners(List<Scanner> scanners, out List<Beacon> beacons)
        {
            while (scanners.Any(s => s.Unknown))
            {
                var unknowns = scanners
                    .Where(s => s.Unknown)
                    .ToList();

                var knowns = scanners
                    .Where(s => !s.Unknown)
                    .ToList();

                foreach (var unknown in unknowns)
                {
                    foreach (var known in knowns.Where(known => !unknown.NoMatch.Contains(known)))
                    {
                        if (FindScanner(unknown, known))
                        {
                            knowns.Add(unknown);
                            _writer.WriteTime();
                            _writer.WriteLine($"Found {unknown}, {scanners.Count(s => s.Unknown)} remaining");
                            break;
                        }
                        else
                        {
                            unknown.NoMatch.Add(known);
                        }
                    }
                }
            }

            beacons = scanners
                .SelectMany(TranslateBeaconsToOrigin)
                .Distinct()
                .ToList();
        }

        private IEnumerable<Beacon> TranslateBeaconsToOrigin(Scanner scanner)
        {
            return scanner.Beacons
                .Select(b => new Beacon
                {
                    X = b.X + scanner.X,
                    Y = b.Y + scanner.Y,
                    Z = b.Z + scanner.Z
                });
        }

        private bool FindScanner(Scanner unknown, Scanner known)
        {
            for (int direction = 0; direction < 6; direction++)
            {
                for (int rotation = 0; rotation < 4; rotation++)
                {
                    var rotated = Rotate(unknown.Beacons, direction, rotation);

                    foreach (var beacon in rotated)
                    {
                        foreach (var knownBeacon in known.Beacons)
                        {
                            if (beacon.Map.Intersect(knownBeacon.Map).Count() >= 12)
                            {
                                unknown.Beacons = rotated;
                                unknown.SetCoord(known.X + knownBeacon.X - beacon.X, known.Y + knownBeacon.Y - beacon.Y, known.Z + knownBeacon.Z - beacon.Z);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private List<Beacon> Rotate(List<Beacon> beacons, int direction, int rotation)
        {
            switch (direction)
            {
                case 0:
                    beacons = beacons // starting pos (nothing)
                        .Select(b => new Beacon(b.X, b.Y, b.Z))
                        .ToList();
                    break;
                case 1:
                    beacons = beacons // x backward (180 degrees over Y)
                        .Select(b => new Beacon(-b.X, b.Y, -b.Z))
                        .ToList();
                    break;
                case 2:
                    beacons = beacons // x to the left (90 degrees over Z)
                        .Select(b => new Beacon(b.Y, -b.X, b.Z))
                        .ToList();
                    break;
                case 3:
                    beacons = beacons // x to the right (270 degrees over Z)
                        .Select(b => new Beacon(-b.Y, b.X, b.Z))
                        .ToList();
                    break;
                case 4:
                    beacons = beacons // x up (90 degrees over Y)
                        .Select(b => new Beacon(b.Z, b.Y, -b.X))
                        .ToList();
                    break;
                case 5:
                    beacons = beacons // x down (270 degrees over Y)
                        .Select(b => new Beacon(-b.Z, b.Y, b.X))
                        .ToList();
                    break;
            }

            switch (rotation)
            {
                case 0:
                    beacons = beacons // starting pos (nothing)
                        .Select(b => new Beacon(b.X, b.Y, b.Z))
                        .ToList();
                    break;
                case 1:
                    beacons = beacons // rol 90 degrees over X
                        .Select(b => new Beacon(b.X, -b.Z, b.Y))
                        .ToList();
                    break;
                case 2:
                    beacons = beacons // roll 180 degrees over X
                        .Select(b => new Beacon(b.X, -b.Y, -b.Z))
                        .ToList();
                    break;
                case 3:
                    beacons = beacons // roll 270 degrees over X
                        .Select(b => new Beacon(b.X, b.Z, -b.Y))
                        .ToList();
                    break;
            }

            CreateBeaconMap(beacons);

            return beacons;
        }

        private void CreateBeaconMap(List<Beacon> beacons)
        {
            foreach (var beacon in beacons)
            {
                beacon.Map = beacons
                .Select(b => new Beacon
                {
                    X = b.X - beacon.X,
                    Y = b.Y - beacon.Y,
                    Z = b.Z - beacon.Z,
                })
                .ToList();
            }
        }

        private List<Scanner> GetScanners(string[] input)
        {
            var result = new List<Scanner>();
            Scanner? current = null;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else if (line.Contains(ScannerLine))
                {
                    var name = line
                        .Replace(ScannerLine, string.Empty)
                        .Trim();
                    current = new Scanner(name);
                    result.Add(current);
                }
                else
                {
                    var coords = line
                        .Split(Split)
                        .Select(i => int.Parse(i))
                        .ToArray();
                    current!.Beacons.Add(new Beacon
                    {
                        X = coords[0],
                        Y = coords[1],
                        Z = coords[2]
                    });
                }
            }

            return result;
        }
    }
}
