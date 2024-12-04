using System.Drawing;

namespace AdventOfCode.Shared.Grid
{
    public record GridCell<TValue>(Point Point, TValue? Value)
    {

    }
}
