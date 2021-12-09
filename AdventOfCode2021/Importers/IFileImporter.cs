namespace AdventOfCode2021.Importers
{
    public interface IFileImporter
    {
        Task<string[]> GetInputAsync(int dayNumber);
    }
}
