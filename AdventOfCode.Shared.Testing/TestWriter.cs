using AdventOfCode.Shared.Services;
using System.Diagnostics;

namespace AdventOfCode.Shared.Testing
{
    public class TestWriter : IScreenWriter
    {
        private bool _disabled;

        public void Enable()
        {
            _disabled = false;
        }

        public void Disable()
        {
            _disabled = true;
        }

        public void NewLine()
        {
            if (_disabled)
            {
                return;
            }
            Debug.WriteLine("");
        }

        public void Write(object value)
        {
            if (_disabled)
            {
                return;
            }
            Debug.Write(value);
        }

        public void Write(object value, ConsoleColor color)
        {
            Write(value);
        }

        public void WriteBlock()
        {
            Write(" ");
        }

        public void WriteBlock(ConsoleColor color)
        {
            if (_disabled)
            {
                return;
            }
            Debug.Write($"{color}"[0]);
        }

        public void WriteLine(object value, ConsoleColor color)
        {
            WriteLine(value);
        }

        public void WriteLine(object value)
        {
            if (_disabled)
            {
                return;
            }
            Debug.WriteLine(value);
        }

        public void WriteTime()
        {
            Write($"{DateTime.Now:G} | ");
        }

        public void SetStart()
        {
            throw new NotImplementedException();
        }

        public ConsoleKey ReadKey()
        {
            throw new NotImplementedException();
        }

        public string? ReadLine()
        {
            throw new NotImplementedException();
        }
    }
}
