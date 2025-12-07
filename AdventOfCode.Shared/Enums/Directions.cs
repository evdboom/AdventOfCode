namespace AdventOfCode.Shared.Enums
{
    [Flags]
    public enum Directions
    {
        Unknown = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        UpLeft = 16,
        UpRight = 32,
        DownLeft = 64,
        DownRight = 128,
        Vertical = Up | Down,
        Horizontal = Left | Right,
        Diagonal = UpLeft | UpRight | DownLeft | DownRight,
        Cardinal = Vertical | Horizontal,
        Slash = UpRight | DownLeft,
        Backslash = UpLeft | DownRight,
        CaretUp = UpLeft | UpRight,
        CaretDown = DownLeft | DownRight,
        CaretLeft = UpLeft | DownLeft,
        CaretRight = DownRight | UpRight,
        All = Diagonal | Cardinal,
    }
}
