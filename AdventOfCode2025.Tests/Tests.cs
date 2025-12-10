using AdventOfCode.Shared.Testing;
using AdventOfCode2025.Days;

namespace AdventOfCode2025.Tests;

public class Tests
{
    public class Day01Tests : TestDay<Day01, TestData>
    {
        protected override long ExpectedResultPartOne => 3;
        protected override long ExpectedResultPartTwo => 6;
    }

    public class Day02Tests : TestDay<Day02, TestData>
    {
        protected override long ExpectedResultPartOne => 1227775554;
        protected override long ExpectedResultPartTwo => 4174379265;
    }

    public class Day03Tests : TestDay<Day03, TestData>
    {
        protected override long ExpectedResultPartOne => 357;
        protected override long ExpectedResultPartTwo => 3121910778619;
    }

    public class Day04Tests : TestDay<Day04, TestData>
    {
        protected override long ExpectedResultPartOne => 13;
        protected override long ExpectedResultPartTwo => 43;
    }

    public class Day05Tests : TestDay<Day05, TestData>
    {
        protected override long ExpectedResultPartOne => 3;
        protected override long ExpectedResultPartTwo => 14;
    }

    public class Day06Tests : TestDay<Day06, TestData>
    {
        protected override long ExpectedResultPartOne => 4277556;
        protected override long ExpectedResultPartTwo => 3263827;
    }

    public class Day07Tests : TestDay<Day07, TestData>
    {
        protected override long ExpectedResultPartOne => 21;
        protected override long ExpectedResultPartTwo => 40;
    }

    public class Day08Tests : TestDay<Day08, TestData>
    {
        public Day08Tests()
            : base()
        {
            _day.Steps = 10;
        }

        protected override long ExpectedResultPartOne => 40;
        protected override long ExpectedResultPartTwo => 25272;
    }

    public class Day09Tests : TestDay<Day09, TestData>
    {
        protected override long ExpectedResultPartOne => 50;
        protected override long ExpectedResultPartTwo => 24;
    }
}
