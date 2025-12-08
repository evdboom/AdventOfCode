using System.Diagnostics;

namespace AdventOfCode.Shared;

[DebuggerDisplay("({X}, {Y}, {Z})")]
public readonly struct Point3D(int x, int y, int z)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public bool Equals(Point3D other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point3D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Point3D left, Point3D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point3D left, Point3D right)
    {
        return !left.Equals(right);
    }
}
