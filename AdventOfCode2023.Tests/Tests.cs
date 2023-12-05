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

        public class Day05Test : TestDay<Day05, TestData>
        {
            protected override long ExpectedResultPartOne => 35;          
            protected override long ExpectedResultPartTwo => 46;
        }
    }
}