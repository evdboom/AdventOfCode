using AdventOfCode.Shared.Services;

namespace AdventOfCode.Shared.Testing
{
    public interface ITestData<Data> : IFileImporter
        where Data : ITestData<Data>, new()
    {
        void SetTestDataPart(string testDataPart);
    }
}
