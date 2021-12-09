using AdventOfCode2021.Days;
using AdventOfCode2021.Enums;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2021.Tests.Base
{
    public abstract class TestDay<Day> where Day : IDay
    {
        private readonly Day _day;

        protected abstract long ExpectedResultPartOne { get; }
        protected abstract long ExpectedResultPartTwo { get; }

        public TestDay()
        {
            _day = (Day)Activator.CreateInstance(typeof(Day), new TestData())!;
        }

        [Fact]
        public async Task TestPartOne()
        {
            var result = await _day.ProcessPartAsync(Part.One);

            Assert.Equal(ExpectedResultPartOne, result.answer);
        }

        [Fact]
        public async Task TestPartTwo()
        {
            var result = await _day.ProcessPartAsync(Part.Two);

            Assert.Equal(ExpectedResultPartTwo, result.answer);
        }
    }
}
