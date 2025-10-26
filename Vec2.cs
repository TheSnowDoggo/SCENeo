namespace SCENeo;

public struct Vec2(float x, float y) : IEquatable<Vec2>
{
    public float X = x;
    public float Y = y;

    #region Presets

    public static Vec2 Zero  => new(+0, +0);
    public static Vec2 Up    => new(+0, -1);
    public static Vec2 Down  => new(+0, +1);
    public static Vec2 Left  => new(-1, +0);
    public static Vec2 Right => new(+1, +0);

    #endregion

    #region Operators

    public static explicit operator Vec2I(Vec2 v) => new((int)v.X, (int)v.Y);

    public static bool operator ==(Vec2 v1, Vec2 v2) => v1.Equals(v2);

    public static bool operator !=(Vec2 v1, Vec2 v2) => !(v1 == v2);

    public static Vec2 operator +(Vec2 v1, Vec2 v2)    => new(v1.X + v2.X, v1.Y + v2.Y);
    public static Vec2 operator +(Vec2 v1, float num ) => new(v1.X + num , v1.Y + num );

    public static Vec2 operator -(Vec2 v1, Vec2 v2)    => new(v1.X - v2.X, v1.Y - v2.Y);
    public static Vec2 operator -(Vec2 v1, float num ) => new(v1.X - num , v1.Y - num );
    public static Vec2 operator -(Vec2 v           )   => new(-v.X       , -v.Y       );

    public static Vec2 operator *(Vec2 v1, Vec2 v2)    => new(v1.X * v2.X, v1.Y * v2.Y);
    public static Vec2 operator *(Vec2 v1, float num ) => new(v1.X * num , v1.Y * num );

    public static Vec2 operator /(Vec2 v1, Vec2 v2)    => new(v1.X / v2.X, v1.Y / v2.Y);
    public static Vec2 operator /(Vec2 v1, float num ) => new(v1.X / num , v1.Y / num );

    #endregion

    #region Maths

    public readonly Vec2 Abs()
    {
        return new Vec2(Math.Abs(X), Math.Abs(Y));
    }

    public readonly Vec2 Invert()
    {
        return new Vec2(Y, X);
    }

    public readonly float Magnitude()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    public readonly Vec2 Normalized()
    {
        float magnitude = Magnitude();
        return new Vec2(X / magnitude, Y / magnitude);
    }

    public readonly Vec2 Round()
    {
        return new Vec2(MathF.Round(X), MathF.Round(Y));
    }

    #endregion

    #region Equality

    public readonly bool Equals(Vec2 other)
    {
        return X == other.X && Y == other.Y;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vec2 vec && Equals(vec);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    #endregion

    public readonly override string ToString()
    {
        return $"{{ {X}, {Y} }}";
    }
}