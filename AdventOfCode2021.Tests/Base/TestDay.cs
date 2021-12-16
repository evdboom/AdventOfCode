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
        private readonly string _testDataPartOne;
        private readonly string _testDataPartTwo;

        private readonly TestData _testData;

        protected abstract long ExpectedResultPartOne { get; }
        protected abstract long ExpectedResultPartTwo { get; }

        public TestDay() : this(string.Empty)
        {
        }

        public TestDay(string testDataPart) : this (testDataPart, testDataPart)
        {
        }

        public TestDay(string testDataPartOne, string testDataPartTwo)
        {
            _testData = new TestData();
            _testDataPartOne = testDataPartOne;
            _testDataPartTwo = testDataPartTwo;

            try
            {
                _day = (Day)Activator.CreateInstance(typeof(Day), _testData)!;
            }
            catch (MissingMethodException)
            {
                _day = (Day)Activator.CreateInstance(typeof(Day), _testData, new TestWriter())!;
            }
        }

        [Fact]
        public async Task TestPartOne()
        {
            _testData.SetTestDataPart(_testDataPartOne);

            var result = await _day.ProcessPartAsync(Part.One);

            Assert.Equal(ExpectedResultPartOne, result.answer);
        }

        [Fact]
        public async Task TestPartTwo()
        {
            _testData.SetTestDataPart(_testDataPartTwo);

            var result = await _day.ProcessPartAsync(Part.Two);

            Assert.Equal(ExpectedResultPartTwo, result.answer);
        }
    }
}
