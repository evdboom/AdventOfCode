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

    public class Day06ATests : TestDay<Day06, TestData>
    {
        public Day06ATests() : base("A")
        {

        }

        protected override long ExpectedResultPartOne => 7;
        protected override long ExpectedResultPartTwo => 19;
    }

    public class Day06BTests : TestDay<Day06, TestData>
    {
        public Day06BTests() : base("B")
        {

        }

        protected override long ExpectedResultPartOne => 5;
        protected override long ExpectedResultPartTwo => 23;
    }

    public class Day06CTests : TestDay<Day06, TestData>
    {
        public Day06CTests() : base("C")
        {

        }

        protected override long ExpectedResultPartOne => 6;
        protected override long ExpectedResultPartTwo => 23;
    }

    public class Day06DTests : TestDay<Day06, TestData>
    {
        public Day06DTests() : base("D")
        {

        }

        protected override long ExpectedResultPartOne => 10;
        protected override long ExpectedResultPartTwo => 29;
    }

    public class Day06ETests : TestDay<Day06, TestData>
    {
        public Day06ETests() : base("E")
        {

        }

        protected override long ExpectedResultPartOne => 11;
        protected override long ExpectedResultPartTwo => 26;
    }

    public class Day07Tests : TestDay<Day07, TestData>
    {
        protected override long ExpectedResultPartOne => 95437;
        protected override long ExpectedResultPartTwo => 24933642;
    }
}
