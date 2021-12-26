using AdventOfCode2021.Constructs.Day23;
using AdventOfCode2021.Days;
using AdventOfCode2021.Tests.Base;
using System;
using Xunit;

namespace AdventOfCode2021.Tests
{
    public class Day01Tests : TestDay<Day01>
    {
        protected override long ExpectedResultPartOne => 7;
        protected override long ExpectedResultPartTwo => 5;
    }

    public class Day02Tests : TestDay<Day02>
    {
        protected override long ExpectedResultPartOne => 150;
        protected override long ExpectedResultPartTwo => 900;
    }

    public class Day03Tests : TestDay<Day03>
    {
        protected override long ExpectedResultPartOne => 198;
        protected override long ExpectedResultPartTwo => 230;
    }

    public class Day04Tests : TestDay<Day04>
    {
        protected override long ExpectedResultPartOne => 4512;
        protected override long ExpectedResultPartTwo => 1924;
    }

    public class Day05Tests : TestDay<Day05>
    {
        protected override long ExpectedResultPartOne => 5;
        protected override long ExpectedResultPartTwo => 12;
    }

    public class Day06Tests : TestDay<Day06>
    {
        protected override long ExpectedResultPartOne => 5934;
        protected override long ExpectedResultPartTwo => 26984457539;
    }

    public class Day07Tests : TestDay<Day07>
    {
        protected override long ExpectedResultPartOne => 37;
        protected override long ExpectedResultPartTwo => 168;
    }

    public class Day08Tests : TestDay<Day08>
    {
        protected override long ExpectedResultPartOne => 26;
        protected override long ExpectedResultPartTwo => 61229;
    }

    public class Day09Tests : TestDay<Day09>
    {
        protected override long ExpectedResultPartOne => 15;
        protected override long ExpectedResultPartTwo => 1134;
    }

    public class Day10Tests : TestDay<Day10>
    {
        protected override long ExpectedResultPartOne => 26397;
        protected override long ExpectedResultPartTwo => 288957;
    }

    public class Day11Tests : TestDay<Day11>
    {
        protected override long ExpectedResultPartOne => 1656;
        protected override long ExpectedResultPartTwo => 195;
    }

    public class Day12Tests : TestDay<Day12>
    {
        protected override long ExpectedResultPartOne => 226;
        protected override long ExpectedResultPartTwo => 3509;
    }

    public class Day13Tests : TestDay<Day13>
    {
        protected override long ExpectedResultPartOne => 17;
        /// <summary>
        /// Actual result of day 13 part 2 is a string, but other then building OCR count the number of points found and return this. for the O char found it's 16
        /// </summary>
        protected override long ExpectedResultPartTwo => 16;
    }

    public class Day14Tests : TestDay<Day14>
    {
        protected override long ExpectedResultPartOne => 1588;
        protected override long ExpectedResultPartTwo => 2188189693529;
    }

    public class Day15Tests : TestDay<Day15>
    {
        protected override long ExpectedResultPartOne => 40;
        protected override long ExpectedResultPartTwo => 315;
    }

    public class Day16ATests : TestDay<Day16>
    {
        public Day16ATests() : base ("A1", "A2")
        {
        }

        protected override long ExpectedResultPartOne => 16;
        protected override long ExpectedResultPartTwo => 3;
    }

    public class Day16BTests : TestDay<Day16>
    {
        public Day16BTests() : base("B1", "B2")
        {
        }

        protected override long ExpectedResultPartOne => 12;
        protected override long ExpectedResultPartTwo => 54;
    }

    public class Day16CTests : TestDay<Day16>
    {
        public Day16CTests() : base("C1", "C2")
        {
        }

        protected override long ExpectedResultPartOne => 23;
        protected override long ExpectedResultPartTwo => 7;
    }

    public class Day16DTests : TestDay<Day16>
    {
        public Day16DTests() : base("D1", "D2")
        {
        }

        protected override long ExpectedResultPartOne => 31;
        protected override long ExpectedResultPartTwo => 9;
    }

    public class Day16ETests : TestDay<Day16>
    {
        public Day16ETests() : base("A1", "E2")
        {
        }

        protected override long ExpectedResultPartOne => 16;
        protected override long ExpectedResultPartTwo => 1;
    }

    public class Day16FTests : TestDay<Day16>
    {
        public Day16FTests() : base("B1", "F2")
        {
        }

        protected override long ExpectedResultPartOne => 12;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day16GTests : TestDay<Day16>
    {
        public Day16GTests() : base("C1", "G2")
        {
        }

        protected override long ExpectedResultPartOne => 23;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day16HTests : TestDay<Day16>
    {
        public Day16HTests() : base("D1", "H2")
        {
        }

        protected override long ExpectedResultPartOne => 31;
        protected override long ExpectedResultPartTwo => 1;
    }

    public class Day17Tests : TestDay<Day17>
    {
        protected override long ExpectedResultPartOne => 45;
        protected override long ExpectedResultPartTwo => 112;
    }

    public class Day18Tests : TestDay<Day18>
    {
        protected override long ExpectedResultPartOne => 4140;
        protected override long ExpectedResultPartTwo => 3993;
    }

    public class Day18MagnitudeATests : TestDay<Day18>
    {
        public Day18MagnitudeATests() : base("A")
        {
        }

        protected override long ExpectedResultPartOne => 143;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeBTests : TestDay<Day18>
    {
        public Day18MagnitudeBTests() : base("B")
        {
        }

        protected override long ExpectedResultPartOne => 1384;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeCTests : TestDay<Day18>
    {
        public Day18MagnitudeCTests() : base("C")
        {
        }

        protected override long ExpectedResultPartOne => 445;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeDTests : TestDay<Day18>
    {
        public Day18MagnitudeDTests() : base("D")
        {
        }

        protected override long ExpectedResultPartOne => 791;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeETests : TestDay<Day18>
    {
        public Day18MagnitudeETests() : base("E")
        {
        }

        protected override long ExpectedResultPartOne => 1137;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeFTests : TestDay<Day18>
    {
        public Day18MagnitudeFTests() : base("F")
        {
        }

        protected override long ExpectedResultPartOne => 3488;
        protected override long ExpectedResultPartTwo => 0;
    }

    public class Day18MagnitudeGTests : TestDay<Day18>
    {
        public Day18MagnitudeGTests() : base("G")
        {
        }

        protected override long ExpectedResultPartOne => 3488;
        protected override long ExpectedResultPartTwo => 3805;
    }

    public class Day18MagnitudeHTests : TestDay<Day18>
    {
        public Day18MagnitudeHTests() : base("H")
        {
        }

        protected override long ExpectedResultPartOne => 3993;
        protected override long ExpectedResultPartTwo => 3993;
    }

    public class Day19Tests : TestDay<Day19>
    {
        protected override long ExpectedResultPartOne => 79;
        protected override long ExpectedResultPartTwo => 3621;
    }

    public class Day20Tests : TestDay<Day20>
    {
        protected override long ExpectedResultPartOne => 35;
        protected override long ExpectedResultPartTwo => 3351;
    }

    public class Day21Tests : TestDay<Day21>
    {
        protected override long ExpectedResultPartOne => 739785;
        protected override long ExpectedResultPartTwo => 444356092776315;
    }

    public class Day22Tests : TestDay<Day22>
    {
        protected override long ExpectedResultPartOne => 590784;
        protected override long ExpectedResultPartTwo => 39769202357779;
    }

    public class Day22ATests : TestDay<Day22>
    {
        public Day22ATests() : base("A")
        { }

        protected override long ExpectedResultPartOne => 474140;
        protected override long ExpectedResultPartTwo => 2758514936282235;
    }

    public class Day23Tests : TestDay<Day23>
    {
        protected override long ExpectedResultPartOne => 12521;
        protected override long ExpectedResultPartTwo => 44169;

        [Fact]
        public void SmallWinIsValid()
        {
            var board = new Board("...........", "AA", "BB", "CC", "DD");
            Assert.True(board.Wins());                
        }

        [Fact]
        public void LargeWinIsValid()
        {
            var board = new Board("...........", "AAAA", "BBBB", "CCCC", "DDDD");
            Assert.True(board.Wins());
        }

        [Fact]
        public void InHallwayIsInvalid()
        {
            var board = new Board("A..........", ".AAA", "BBBB", "CCCC", "DDDD");
            Assert.False(board.Wins());
        }

        [Fact]
        public void InWrongWroomIsInvalid()
        {
            var board = new Board("...........", "AAAB", "BBBA", "CCCC", "DDDD");
            Assert.False(board.Wins());
        }
    }

    public class Day25Tests : TestDay<Day25>
    {
        protected override long ExpectedResultPartOne => 58;
        protected override long ExpectedResultPartTwo => 0;
    }

}