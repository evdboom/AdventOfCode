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
}
