namespace AdventOfCode2021.Services
{
    public interface IFileImporter
    {
        Task<string[]> GetInputAsync(int dayNumber);
    }
}
