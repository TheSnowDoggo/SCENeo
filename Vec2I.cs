namespace SCENeo;

/// <summary>
/// A struct representing a 2D integer vector.
/// </summary>
/// <param name="x">The x-component.</param>
/// <param name="y">The y-component.</param>
public struct Vec2I(int x, int y) : IEquatable<Vec2I>
{
    /// <summary>
    /// The x-component of this vector.
    /// </summary>
    public int X = x;

    /// <summary>
    /// The x-component of this vector.
    /// </summary>
    public int Y = y;

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 0).
    /// </summary>
    public static Vec2I Zero => new(0, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, -1).
    /// </summary>
    public static Vec2I Up => new(0, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 1).
    /// </summary>
    public static Vec2I Down => new(0, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 0).
    /// </summary>
    public static Vec2I Left => new(-1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(+1, 0).
    /// </summary>
    public static Vec2I Right => new(+1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, -1).
    /// </summary>
    public static Vec2I UpLeft => new(-1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, -1).
    /// </summary>
    public static Vec2I UpRight => new(+1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 1).
    /// </summary>
    public static Vec2I DownLeft => new(-1, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, 1).
    /// </summary>
    public static Vec2I DownRight => new(+1, +1);

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
        return new Vec2I(Y, -X);
    }

    /// <summary>
    /// Returns this vector rotated 90 degrees anti-clockwise.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2I Rotated90AC()
    {
        return new Vec2I(-Y, X);
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

    public readonly bool Equals(Vec2I other)
    {
        return X == other.X && Y == other.Y;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vec2I vec && Equals(vec);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public readonly override string ToString()
    {
        return $"{{ {X}, {Y} }}";
    }
}