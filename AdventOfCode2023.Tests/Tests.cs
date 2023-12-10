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
    }
}