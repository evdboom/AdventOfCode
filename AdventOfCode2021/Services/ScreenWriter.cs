namespace AdventOfCode2021.Services
{
    public class ScreenWriter : IScreenWriter
    {
        private const string Block = " ";

        public void NewLine()
        {
            Console.WriteLine();
        }

        public void Write(object value)
        {
            Console.Write(value);
        }

        public void Write(object value, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Write(value);
            Console.ForegroundColor = old;
        }

        public void WriteBlock()
        {
            Write(Block);
        }


        public void WriteBlock(ConsoleColor color)
        {
            var old = Console.BackgroundColor;
            Console.BackgroundColor = color;
            WriteBlock();
            Console.BackgroundColor = old;
        }

        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(object value, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            WriteLine(value);
            Console.ForegroundColor = old;
        }
    }
}
