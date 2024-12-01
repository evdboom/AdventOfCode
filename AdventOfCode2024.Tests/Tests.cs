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
    }
}
