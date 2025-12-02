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
}
