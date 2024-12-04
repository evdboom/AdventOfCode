using System.Collections;
using System.Drawing;

namespace AdventOfCode.Shared.Grid
{
    public class GridEnumerator<TValue>(Grid<TValue> grid) : IEnumerator<GridCell<TValue>>, IEnumerator
    {
        private readonly Grid<TValue> _grid = grid;

        private Point _currentPoint = new Point(-1, 0);


        public GridCell<TValue> Current => _grid.GetCell(_currentPoint);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (_currentPoint.X < _grid.MaxX)
            {
                _currentPoint.X++;
                return true;
            }
            else if (_currentPoint.Y < _grid.MaxY)
            {
                _currentPoint.X = 0;
                _currentPoint.Y++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _currentPoint = new Point(-1, 0);
        }
    }
}
