using SCENeo.Ui;

namespace SCENeo;

/// <summary>
/// A struct representing a 2D integer vector.
/// </summary>
public partial struct Vec2I : IEquatable<Vec2I>
{
    /// <summary>
    /// The x-component of this vector.
    /// </summary>
    public int X;

    /// <summary>
    /// The x-component of this vector.
    /// </summary>
    public int Y;

    public Vec2I(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator Vec2(Vec2I v) => new(v.X, v.Y);

    public static bool operator ==(Vec2I v1, Vec2I v2) => v1.Equals(v2);
    public static bool operator !=(Vec2I v1, Vec2I v2) => !(v1 == v2);

    public static Vec2I operator +(Vec2I v1, Vec2I v2) => new(v1.X + v2.X, v1.Y + v2.Y);
    public static Vec2I operator +(Vec2I v1, int num) => new(v1.X + num, v1.Y + num);

    public static Vec2I operator -(Vec2I v1, Vec2I v2) => new(v1.X - v2.X, v1.Y - v2.Y);
    public static Vec2I operator -(Vec2I v1, int num) => new(v1.X - num, v1.Y - num);
    public static Vec2I operator -(Vec2I v) => new(-v.X, -v.Y);

    public static Vec2I operator *(Vec2I v1, Vec2I v2) => new(v1.X * v2.X, v1.Y * v2.Y);
    public static Vec2I operator *(Vec2I v1, int num ) => new(v1.X * num, v1.Y * num);

    public static Vec2I operator /(Vec2I v1, Vec2I v2) => new(v1.X / v2.X, v1.Y / v2.Y);
    public static Vec2I operator /(Vec2I v1, int num) => new(v1.X / num, v1.Y / num);

    public static bool operator <(Vec2I v1, Vec2I v2) => v1.X < v2.X && v1.Y < v2.Y;
    public static bool operator >(Vec2I v1, Vec2I v2) => v1.X > v2.X && v1.Y > v2.Y;

    public static bool operator <=(Vec2I v1, Vec2I v2) => v1.X <= v2.X && v1.Y <= v2.Y;
    public static bool operator >=(Vec2I v1, Vec2I v2) => v1.X >= v2.X && v1.Y >= v2.Y;

    public static Vec2I Parse(string str)
    {
        int comma = str.IndexOf(',');
        if (comma == -1)
        {
            throw new InvalidDataException("Missing comma seperating components.");
        }

        string xStr = str[..comma];
        if (!int.TryParse(xStr, out int x))
        {
            throw new InvalidDataException($"X component \'{xStr}\' was in an invalid format.");
        }

        string yStr = str[(comma + 1)..];
        if (!int.TryParse(yStr, out int y))
        {
            throw new InvalidDataException($"Y component \'{yStr}\' was in an invalid format.");
        }

        return new Vec2I(x, y);
    }

    public static bool TryParse(string str, out Vec2I value)
    {
        int comma = str.IndexOf(',');
        if (comma == -1 || comma == str.Length -1)
        {
            value = default;
            return false;
        }

        if (!int.TryParse(str[..comma], out int x))
        {
            value = default;
            return false;
        }

        if (!int.TryParse(str[(comma + 1)..], out int y))
        {
            value = default;
            return false;
        }

        value = new Vec2I(x, y);
        return true;
    }

    /// <summary>
    /// Returns this vector with the absolute value of both components.
    /// </summary>
    /// <returns>The absolute vector.</returns>
    public readonly Vec2I Abs()
    {
        return new Vec2I(Math.Abs(X), Math.Abs(Y));
    }

    /// <summary>
    /// Returns this vector with the x and y components swapped.
    /// </summary>
    /// <returns>The inverted vector.</returns>
    public readonly Vec2I Inverted()
    {
        return new Vec2I(Y, X);
    }

    /// <summary>
    /// Returns this vector rotated 90 degrees clockwise.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2I Rotated90C()
    {
        return new Vec2I(-Y, X);
    }

    /// <summary>
    /// Returns this vector rotated 90 degrees anti-clockwise.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2I Rotated90AC()
    {
        return new Vec2I(Y, -X);
    }

    /// <summary>
    /// Returns the vector rotated by the given rotation enum.
    /// </summary>
    /// <param name="rotation">The rotation.</param>
    /// <returns>The rotated vector.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public readonly Vec2I Rotated(Rotation rotation)
    {
        return rotation switch
        {
            Rotation.None  => this,
            Rotation.Right => Rotated90C(),
            Rotation.Flip  => Rotated180(),
            Rotation.Left  => Rotated90AC(),
            _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, "Rotation is invalid."),
        };
    }

    /// <summary>
    /// Returns this vector rotated 180 degrees.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2I Rotated180()
    {
        return new Vec2I(-X, -Y);
    }

    /// <summary>
    /// Deconstructs this vector.
    /// </summary>
    /// <param name="x">The x-component.</param>
    /// <param name="y">The y-component.</param>
    public readonly void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public readonly Vec2 ToVec2()
    {
        return new Vec2(X, Y);
    }

    public readonly bool Equals(Vec2I other)
    {
        return X == other.X && Y == other.Y;
    }

    public readonly override bool Equals(object obj)
    {
        return obj is Vec2I vec && Equals(vec);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public readonly override string ToString()
    {
        return $"{X}, {Y}";
    }
}