using AdventOfCode2021.Services;
using System;
using System.Diagnostics;

namespace AdventOfCode2021.Tests.Base
{
    public class TestWriter : IScreenWriter
    {
        public void NewLine()
        {
            Debug.WriteLine("");
        }

        public void Write(object value)
        {
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
            Debug.Write($"{color}"[0]);
        }

        public void WriteLine(object value, ConsoleColor color)
        {
            WriteLine(value);
        }

        public void WriteLine(object value)
        {
            Debug.WriteLine(value);
        }
    }
}
