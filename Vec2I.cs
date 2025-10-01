namespace SCENeo;

public struct Vec2I(int x, int y) : IEquatable<Vec2I>
{
    public int X = x;
    public int Y = y;

    #region Operators

    public static bool operator ==(Vec2I v1, Vec2I v2) => v1.Equals(v2);

    public static bool operator !=(Vec2I v1, Vec2I v2) => !(v1 == v2);

    public static Vec2I operator +(Vec2I v1, Vec2I v2) => new(v1.X + v2.X, v1.Y + v2.Y);
    public static Vec2I operator +(Vec2I v1, int num ) => new(v1.X + num , v1.Y + num );

    public static Vec2I operator -(Vec2I v1, Vec2I v2) => new(v1.X - v2.X, v1.Y - v2.Y);
    public static Vec2I operator -(Vec2I v1, int num ) => new(v1.X - num , v1.Y - num );
    public static Vec2I operator -(Vec2I v           ) => new(-v.X       , -v.Y       );

    public static Vec2I operator *(Vec2I v1, Vec2I v2) => new(v1.X * v2.X, v1.Y * v2.Y);
    public static Vec2I operator *(Vec2I v1, int num ) => new(v1.X * num , v1.Y * num );

    public static Vec2I operator /(Vec2I v1, Vec2I v2) => new(v1.X / v2.X, v1.Y / v2.Y);
    public static Vec2I operator /(Vec2I v1, int num ) => new(v1.X / num , v1.Y / num );

    #endregion

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
