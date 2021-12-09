namespace AdventOfCode2021.Constructs
{
    internal class Day06FishSchool
    {        
        private const int ResetCounter = 6;
        private const int InitialCounter = 8;

        public int Counter { get; private set; }
        public long Size { get; private set; }

        public Day06FishSchool(long size) : this(InitialCounter, size)
        {            
        }

        public Day06FishSchool(int initialCounter, long size)
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
