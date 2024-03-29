﻿using AdventOfCode.Shared.Testing;
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

    public class Day08Tests : TestDay<Day08, TestData>
    {
        protected override long ExpectedResultPartOne => 21;
        protected override long ExpectedResultPartTwo => 8;
    }

    public class Day09Tests : TestDay<Day09, TestData>
    {
        protected override long ExpectedResultPartOne => 13;
        protected override long ExpectedResultPartTwo => 1;
    }

    public class Day09ATests : TestDay<Day09, TestData>
    {
        public Day09ATests() : base("A")
        {

        }

        protected override long ExpectedResultPartOne => 88;
        protected override long ExpectedResultPartTwo => 36;
    }

    public class Day10Tests : TestDay<Day10, TestData>
    {
        protected override long ExpectedResultPartOne => 13140;
        protected override long ExpectedResultPartTwo => -1;
    }

    public class Day11Tests : TestDay<Day11, TestData>
    {
        protected override long ExpectedResultPartOne => 10605;
        protected override long ExpectedResultPartTwo => 2713310158;
    }

    public class Day12Tests : TestDay<Day12, TestData>
    {
        protected override long ExpectedResultPartOne => 31;
        protected override long ExpectedResultPartTwo => 29;
    }

    public class Day13Tests : TestDay<Day13, TestData>
    {
        protected override long ExpectedResultPartOne => 13;
        protected override long ExpectedResultPartTwo => 140;
    }

    public class Day14Tests : TestDay<Day14, TestData>
    {
        protected override long ExpectedResultPartOne => 24;
        protected override long ExpectedResultPartTwo => 93;
    }

    public class Day15Tests : TestDay<Day15, TestData>
    {
        public Day15Tests() : base()
        {
            _day.RequestedRow = 10;
            _day.HighestCoordinates = 20;
        }

        protected override long ExpectedResultPartOne => 26;
        protected override long ExpectedResultPartTwo => 56000011;
    }

    public class Day16Tests : TestDay<Day16, TestData>
    {
        protected override long ExpectedResultPartOne => 1651;
        protected override long ExpectedResultPartTwo => 1707;
    }

    public class Day17Tests : TestDay<Day17, TestData>
    {
        protected override long ExpectedResultPartOne => 3068;
        protected override long ExpectedResultPartTwo => 1514285714288;
    }

    public class Day18Tests : TestDay<Day18, TestData>
    {
        protected override long ExpectedResultPartOne => 64;
        protected override long ExpectedResultPartTwo => 58;
    }

    public class Day19Tests : TestDay<Day19, TestData>
    {
        protected override long ExpectedResultPartOne => 33;
        protected override long ExpectedResultPartTwo => 3472;
    }

    public class Day20Tests : TestDay<Day20, TestData>
    {
        protected override long ExpectedResultPartOne => 3;
        protected override long ExpectedResultPartTwo => 1623178306;
    }

    public class Day21Tests : TestDay<Day21, TestData>
    {
        protected override long ExpectedResultPartOne => 152;
        protected override long ExpectedResultPartTwo => 301;
    }

    public class Day22Tests : TestDay<Day22, TestData>
    {
        public Day22Tests() : base()
        {
            _day.CubeSize = 4;
            _day.FaceMappings = new()
            {
                { (1, 0), (6, 2, (c, p) => p with { X = c - 1, Y = c - 1 - p.X }) },
                { (1, 1), (4, 1, (c, p) => p with { Y = 0 }) },
                { (1, 2), (3, 1, (c, p) => p with { X = p.Y, Y = 0 }) },
                { (1, 3), (2, 1, (c, p) => p with { X = c - 1 - p.X, Y = 0 }) },

                { (2, 0), (3, 0, (c, p) => p with { X = 0 }) },
                { (2, 1), (5, 3, (c, p) => p with { X = c - 1 - p.X, Y = c - 1 }) },
                { (2, 2), (6, 3, (c, p) => p with { X = p.Y, Y = c - 1 }) },
                { (2, 3), (1, 1, (c, p) => p with { X = c - 1 - p.X, Y = 0 }) },

                { (3, 0), (4, 0, (c, p) => p with { X = 0 }) },
                { (3, 1), (5, 0, (c, p) => p with { X = 0, Y = c - 1 - p.X }) },
                { (3, 2), (2, 2, (c, p) => p with { X = c - 1 }) },
                { (3, 3), (1, 0, (c, p) => p with { X = 0, Y = p.X }) },

                { (4, 0), (6, 1, (c, p) => p with { X = c - 1 - p.Y, Y = 0 }) },
                { (4, 1), (5, 1, (c, p) => p with { Y = 0 }) },
                { (4, 2), (3, 2, (c, p) => p with { X = c - 1 }) },
                { (4, 3), (1, 3, (c, p) => p with { Y = c - 1 }) },

                { (5, 0), (6, 0, (c, p) => p with { X = 0 }) },
                { (5, 1), (2, 3, (c, p) => p with { X = c - 1 - p.X, Y = c - 1 }) },
                { (5, 2), (3, 3, (c, p) => p with { X = c - 1 - p.Y, Y = c - 1 }) },
                { (5, 3), (4, 3, (c, p) => p with { Y = c - 1 }) },

                { (6, 0), (1, 2, (c, p) => p with { X = c - 1, Y = c - 1 - p.Y }) },
                { (6, 1), (2, 0, (c, p) => p with { X = 0, Y = c - 1 - p .X }) },
                { (6, 2), (5, 2, (c, p) => p with { X = c - 1 }) },
                { (6, 3), (4, 2, (c, p) => p with { X = c - 1, Y = c - 1 - p.X }) },
            };
        }

        protected override long ExpectedResultPartOne => 6032;
        protected override long ExpectedResultPartTwo => 5031;
    }

    public class Day23Tests : TestDay<Day23, TestData>
    {
        protected override long ExpectedResultPartOne => 110;
        protected override long ExpectedResultPartTwo => 20;
    }

    public class Day24Tests : TestDay<Day24, TestData>
    {
        protected override long ExpectedResultPartOne => 18;
        protected override long ExpectedResultPartTwo => 54;
    }

    public class Day25Tests : TestDay<Day25, TestData>
    {
        protected override long ExpectedResultPartOne => 4890;
        protected override long ExpectedResultPartTwo => -1;

        [Fact]
        public async Task TestBobPartOne()
        {
            await RunPartOne();
            Assert.Equal("2=-1=0", _day.BobCommand);
        }
    }
}
