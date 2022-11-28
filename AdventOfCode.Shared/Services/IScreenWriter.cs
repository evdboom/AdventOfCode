namespace AdventOfCode.Shared.Services
{
    public interface IScreenWriter
    {
        void Write(object value);
        void Write(object value, ConsoleColor color);
        void WriteLine(object value, ConsoleColor color);
        void WriteLine(object value);
        void NewLine();
        void WriteBlock();
        void WriteBlock(ConsoleColor color);
        void Disable();
        void Enable();
        void WriteTime();


    }
}
