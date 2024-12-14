using System.Drawing;

namespace AdventOfCode2024.Days.Day14Group
{
    public class Robot(int x, int y, int dx, int dy)
    {
        public int X { get; private set; } = x;
        public int Y { get; private set; } = y;
        public int DX { get; } = dx;
        public int DY { get; } = dy;

        public Point Location => new(X, Y);

        public void Move(int steps, int gridWidth, int gridHeight)
        {
            X += DX * steps;
            if (X < 0)
            {     
                X = gridWidth + X % gridWidth;                
            }
            X = X % gridWidth;
            
            Y += DY * steps;
            if (Y < 0)
            {
                Y = gridHeight + Y % gridHeight;
            }
            Y = Y % gridHeight;
        }

        public override string ToString()
        {
            return $"Robot at ({X}, {Y}) moving ({DX}, {DY})";
        }
    }
}
