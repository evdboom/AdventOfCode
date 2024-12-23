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

        public class Day11Tests : TestDay<Day11, TestData>
        {
            protected override long ExpectedResultPartOne => 55312;
            protected override long ExpectedResultPartTwo => 65601038650482;
        }

        public class Day12Tests : TestDay<Day12, TestData>
        {
            protected override long ExpectedResultPartOne => 1930;
            protected override long ExpectedResultPartTwo => 1206;
        }

        public class Day13Tests : TestDay<Day13, TestData>
        {
            protected override long ExpectedResultPartOne => 480;
            protected override long ExpectedResultPartTwo => 875318608908;
        }

        public class Day14Tests : TestDay<Day14, TestData>
        {
            protected override void ChangeDay(Day14 day)
            {                
                day.GridWidth = 11;
                day.GridHeight = 7;
            }


            protected override long ExpectedResultPartOne => 12;
            // actually no solution possible with the testdata.
            protected override long ExpectedResultPartTwo => -1;
        }

        public class Day15Tests : TestDay<Day15, TestData>
        {
            protected override long ExpectedResultPartOne => 10092;
            protected override long ExpectedResultPartTwo => 9021;
        }

        public class Day16Tests : TestDay<Day16, TestData>
        {
            protected override long ExpectedResultPartOne => 11048;
            protected override long ExpectedResultPartTwo => 64;
        }

        public class Day17Tests() : TestDay<Day17, TestData>("PartOne", "PartTwo")
        {
            protected override long ExpectedResultPartOne => -1;
            protected override long ExpectedResultPartTwo => 117440;

            [Fact]
            public async Task OwnTestPartOne()
            {
                await RunPartOne();
                Assert.Equal("4,6,3,5,6,3,5,2,1,0", _day.PartOneResult());
            }
        }

        public class Day18Tests : TestDay<Day18, TestData>
        {
            protected override void ChangeDay(Day18 day)
            {
                day.GridSize = 7;
                day.BytesToRead = 12;
            }

            protected override long ExpectedResultPartOne => 22;
            protected override long ExpectedResultPartTwo => -1;

            [Fact]
            public async Task OwnTestPartTwo()
            {
                await RunPartTwo();
                Assert.Equal("6,1", _day.PartTwoResult());
            }
        }

        public class Day19Tests : TestDay<Day19, TestData>
        {
            protected override long ExpectedResultPartOne => 6;
            protected override long ExpectedResultPartTwo => 16;
        }

        public class Day20Tests : TestDay<Day20, TestData>
        {
            protected override void ChangeDay(Day20 day)
            {
                day.TimeToSave = 60;
            }

            protected override long ExpectedResultPartOne => 1;
            protected override long ExpectedResultPartTwo => 129;
        }
    }
}
