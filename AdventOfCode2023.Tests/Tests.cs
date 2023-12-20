using AdventOfCode.Shared.Testing;
using AdventOfCode2023.Days;

namespace AdventOfCode2023.Tests
{
    public class Tests
    {
        public class Day01Tests : TestDay<Day01, TestData>
        {
            public Day01Tests() : base("Part01", "Part02")
            {

            }

            protected override long ExpectedResultPartOne => 142;
            protected override long ExpectedResultPartTwo => 281;
        }

        public class Day02Tests : TestDay<Day02, TestData>
        {
            protected override long ExpectedResultPartOne => 8;
            protected override long ExpectedResultPartTwo => 2286;
        }

        public class Day03Tests: TestDay<Day03, TestData>
        {
            protected override long ExpectedResultPartOne => 4361;
            protected override long ExpectedResultPartTwo => 467835;
        }

        public class Day04Tests : TestDay<Day04, TestData>
        {
            protected override long ExpectedResultPartOne => 13;
            protected override long ExpectedResultPartTwo => 30;
        }

        public class Day05Tests : TestDay<Day05, TestData>
        {
            protected override long ExpectedResultPartOne => 35;          
            protected override long ExpectedResultPartTwo => 46;
        }

        public class Day06Tests : TestDay<Day06, TestData>
        {
            protected override long ExpectedResultPartOne => 288;
            protected override long ExpectedResultPartTwo => 71503;
        }

        public class Day07Tests : TestDay<Day07, TestData>
        {
            protected override long ExpectedResultPartOne => 6440;
            protected override long ExpectedResultPartTwo => 5905;
        }

        public class Day08Tests : TestDay<Day08, TestData>
        {
            public Day08Tests() : base("Part01", "Part02")
            {

            }

            protected override long ExpectedResultPartOne => 6;
            protected override long ExpectedResultPartTwo => 6;
        }

        public class Day09Tests : TestDay<Day09, TestData>
        {
            protected override long ExpectedResultPartOne => 114;
            protected override long ExpectedResultPartTwo => 2;
        }

        public class Day10Tests : TestDay<Day10, TestData>
        {
            public Day10Tests() : base("Part01", "Part02")
            {

            }

            protected override long ExpectedResultPartOne => 8;
            protected override long ExpectedResultPartTwo => 10;
        }

        public class Day11Tests : TestDay<Day11, TestData>
        {
            protected override long ExpectedResultPartOne => 374;
            protected override long ExpectedResultPartTwo => 82000210;
        }

        public class Day12Tests : TestDay<Day12, TestData>
        {
            protected override long ExpectedResultPartOne => 21;
            protected override long ExpectedResultPartTwo => 525152;
        }

        public class Day13Tests : TestDay<Day13, TestData>
        {
            protected override long ExpectedResultPartOne => 405;
            protected override long ExpectedResultPartTwo => 400;
        }

        public class Day14Tests : TestDay<Day14, TestData>
        {
            protected override long ExpectedResultPartOne => 136;
            protected override long ExpectedResultPartTwo => 64;
        }

        public class Day15Tests : TestDay<Day15, TestData>
        {
            protected override long ExpectedResultPartOne => 1320;
            protected override long ExpectedResultPartTwo => 145;
        }

        public class Day16Tests : TestDay<Day16, TestData>
        {
            protected override long ExpectedResultPartOne => 46;
            protected override long ExpectedResultPartTwo => 51;
        }

        public class Day17Tests : TestDay<Day17, TestData>
        {
            public Day17Tests() : base("first")
            { }

            protected override long ExpectedResultPartOne => 102;
            protected override long ExpectedResultPartTwo => 94;
        }

        public class Day17SecondTests : TestDay<Day17, TestData>
        {
            public Day17SecondTests(): base("second")
            { }

            protected override long ExpectedResultPartOne => 59;
            protected override long ExpectedResultPartTwo => 71;
        }

        public class Day18Tests : TestDay<Day18, TestData>
        {
            protected override long ExpectedResultPartOne => 62;
            protected override long ExpectedResultPartTwo => 952408144115;
        }

        public class Day19Tests : TestDay<Day19, TestData>
        {
            protected override long ExpectedResultPartOne => 19114;
            protected override long ExpectedResultPartTwo => 167409079868000;
        }

        public class Day20FirstTests : TestDay<Day20, TestData>
        {
            public Day20FirstTests() : base("first")
            {

            }

            protected override long ExpectedResultPartOne => 32000000;
            protected override long ExpectedResultPartTwo => -1;
        }

        public class Day20SecondTests : TestDay<Day20, TestData>
        {
            public Day20SecondTests() : base("second")
            {

            }

            protected override long ExpectedResultPartOne => 11687500;
            protected override long ExpectedResultPartTwo => -1;
        }
    }
}