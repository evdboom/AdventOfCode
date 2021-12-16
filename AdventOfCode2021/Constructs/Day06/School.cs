namespace AdventOfCode2021.Constructs.Day06
{
    internal class School
    {        
        private const int ResetCounter = 6;
        private const int InitialCounter = 8;

        public int Counter { get; private set; }
        public long Size { get; private set; }

        public School(long size) : this(InitialCounter, size)
        {            
        }

        public School(int initialCounter, long size)
        {
            Counter = initialCounter;
            Size = size;
        }

        public void PassDay(out bool reproduces)
        {
            if (Counter == 0)
            {
                Counter = ResetCounter;
                reproduces = true;
            }
            else
            {
                Counter--;
                reproduces = false;
            }
        }
    }
}
