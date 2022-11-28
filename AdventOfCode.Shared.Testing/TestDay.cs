using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Testing
{
    public abstract class TestDay<Day, Data> 
        where Day : IDay
        where Data : ITestData<Data>, new()
    {
        private readonly Day _day;
        private readonly string _testDataPartOne;
        private readonly string _testDataPartTwo;

        private readonly Data _testData;

        protected abstract long ExpectedResultPartOne { get; }
        protected abstract long ExpectedResultPartTwo { get; }

        public TestDay() : this(string.Empty)
        {

        }

        public TestDay(string testDataPart) : this(testDataPart, testDataPart)
        {
        }

        public TestDay(string testDataPartOne, string testDataPartTwo)
        {
            _testData = new();
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

            var (answer, _) = await _day.ProcessPartAsync(Part.One);

            Assert.Equal(ExpectedResultPartOne, answer);
        }

        [Fact]
        public async Task TestPartTwo()
        {
            _testData.SetTestDataPart(_testDataPartTwo);

            var (answer, _) = await _day.ProcessPartAsync(Part.Two);

            Assert.Equal(ExpectedResultPartTwo, answer);
        }
    }
}
