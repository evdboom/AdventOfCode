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

    public class Day03Tests : TestDay<Day03, TestData>
    {
        protected override long ExpectedResultPartOne => 157;
        protected override long ExpectedResultPartTwo => 70;
    }

    public class Day04Tests : TestDay<Day04, TestData>
    {
        protected override long ExpectedResultPartOne => 2;
        protected override long ExpectedResultPartTwo => 4;
    }

    public class Day05Tests : TestDay<Day05, TestData>
    {
        protected override long ExpectedResultPartOne => -1;
        protected override long ExpectedResultPartTwo => -1;

        [Fact]
        public async Task OwnTestPartOne()
        {
            await RunPartOne();
            Assert.Equal("CMZ", _day.PartOneResult());
        }

        [Fact]
        public async Task OwnTestPartTwo()
        {
            await RunPartTwo();
            Assert.Equal("MCD", _day.PartTwoResult());
        }
    }
}
