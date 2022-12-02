using AdventOfCode.Shared.Testing;
using AdventOfCode2022.Days;

namespace AdventOfCode2022.Tests
{
    public class Day01Tests : TestDay<Day01, TestData>
    {
        protected override long ExpectedResultPartOne => 24000;
        protected override long ExpectedResultPartTwo => 45000;
    }

    public class Day02Tests : TestDay<Day02, TestData>
    {
        protected override long ExpectedResultPartOne => 15;
        protected override long ExpectedResultPartTwo => 12;
    }
}
