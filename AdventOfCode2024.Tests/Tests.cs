using AdventOfCode.Shared.Testing;
using AdventOfCode2024.Days;

namespace AdventOfCode2024.Tests
{
    public class Tests
    {
        public class Day01Tests : TestDay<Day01, TestData>
        {            
            protected override long ExpectedResultPartOne => 11;
            protected override long ExpectedResultPartTwo => 31;
        }

        public class Day02Tests : TestDay<Day02, TestData>
        {
            protected override long ExpectedResultPartOne => 2;
            protected override long ExpectedResultPartTwo => 4;
        }

        public class Day03Tests : TestDay<Day03, TestData>
        {
            protected override long ExpectedResultPartOne => 161;
            protected override long ExpectedResultPartTwo => 48;
        }

        public class Day04Tests : TestDay<Day04, TestData>
        {
            protected override long ExpectedResultPartOne => 18;
            protected override long ExpectedResultPartTwo => 9;
        }

        public class Day05Tests : TestDay<Day05, TestData>
        {
            protected override long ExpectedResultPartOne => 143;
            protected override long ExpectedResultPartTwo => 123;
        }

        public class Day06Tests : TestDay<Day06, TestData>
        {
            protected override long ExpectedResultPartOne => 41;
            protected override long ExpectedResultPartTwo => 6;
        }

        public class Day07Tests : TestDay<Day07, TestData>
        {
            protected override long ExpectedResultPartOne => 3749;
            protected override long ExpectedResultPartTwo => 11387;
        }

        public class Day08Tests : TestDay<Day08, TestData>
        {
            protected override long ExpectedResultPartOne => 14;
            protected override long ExpectedResultPartTwo => 34;
        }

        public class Day09Tests : TestDay<Day09, TestData>
        {
            protected override long ExpectedResultPartOne => 1928;
            protected override long ExpectedResultPartTwo => 2858;
        }

        public class Day10Tests : TestDay<Day10, TestData>
        {
            protected override long ExpectedResultPartOne => 36;
            protected override long ExpectedResultPartTwo => 81;
        }
    }
}
