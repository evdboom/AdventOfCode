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
    }
}