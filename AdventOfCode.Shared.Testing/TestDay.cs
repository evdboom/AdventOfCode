using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Enums;

namespace AdventOfCode.Shared.Testing
{
    public abstract class TestDay<Day, Data> 
        where Day : IDay
        where Data : ITestData<Data>, new()
    {
        protected readonly Day _day;
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

        protected virtual void ChangeDay(Day day)
        {
        }

        protected async Task<long> RunPartOne()
        {
            _testData.SetTestDataPart(_testDataPartOne);
            ChangeDay(_day);
            var (answer, _) = await _day.ProcessPartAsync(Part.One);

            return answer;
        }

        protected async Task<long> RunPartTwo()
        {
            _testData.SetTestDataPart(_testDataPartTwo);
            ChangeDay(_day);
            var (answer, _) = await _day.ProcessPartAsync(Part.Two);

            return answer;
        }

        [Fact]
        public async Task TestPartOne()
        {
            var answer = await RunPartOne();

            Assert.Equal(ExpectedResultPartOne, answer);
        }

        [Fact]
        public async Task TestPartTwo()
        {
            var answer = await RunPartTwo();

            Assert.Equal(ExpectedResultPartTwo, answer);
        }
    }
}
