namespace SCENeo;

public struct Vec2(float x, float y) : IEquatable<Vec2>
{
    public float X = x;
    public float Y = y;

    #region Presets

    public static Vec2 Zero  => new( 0f,  0f);
    public static Vec2 Up    => new( 0f, -1f);
    public static Vec2 Down  => new( 0f, +1f);
    public static Vec2 Left  => new(-1f,  0f);
    public static Vec2 Right => new(+1f,  0f);

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

    public readonly Rect2D AreaBetween(Vec2 other)
    {
        return new Rect2D()
        {
            Left   = MathF.Min(X, other.X),
            Top    = MathF.Min(Y, other.Y),
            Right  = MathF.Max(X, other.X),
            Bottom = MathF.Max(Y, other.Y),
        };
    }

    public readonly float Length()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    public readonly float LengthSquared()
    {
        return X * X + Y * Y;
    }

    public readonly Vec2 Abs()
    {
        return new Vec2(Math.Abs(X), Math.Abs(Y));
    }

    public readonly Vec2 Invert()
    {
        return new Vec2(Y, X);
    }

    public readonly float DistanceTo(Vec2 target)
    {
        return (target - this).Length();
    }

    public readonly Vec2 Normalized()
    {
        float l = LengthSquared();

        if (l == 0f) return Zero;

        l = MathF.Sqrt(l);

        return new Vec2(X / l, Y / l);
    }

    public readonly float Angle()
    {
        return MathF.Atan(Y / X);
    }

    public readonly Vec2 DirectionTo(Vec2 target)
    {
        return (target - this).Normalized();
    }

    public readonly float Dot(Vec2 other)
    {
        return X * other.X + Y * other.Y;
    }

    public readonly Vec2 Truncate(float maxLength)
    {
        Vec2 result = this;
        float length = result.Length();
        if (length > 0f && length > maxLength)
        {
            result /= length;
            result *= maxLength;
        }
        return result;
    }

    #endregion

    #region Rotate

    public readonly Vec2 Rotated(float rotation)
    {
        return new Vec2()
        {
            X = X * MathF.Cos(rotation) - Y * MathF.Sin(rotation),
            Y = X * MathF.Sin(rotation) + Y * MathF.Cos(rotation),
        };
    }

    public readonly Vec2 Rotate90C()
    {
        return new Vec2(Y, -X);
    }

    public readonly Vec2 Rotate90AC()
    {
        return new Vec2(-Y, X);
    }

    public readonly Vec2 Rotate180()
    {
        return new Vec2(-X, -Y);
    }

    #endregion

    #region Round

    public readonly Vec2 Round()
    {
        return new Vec2(MathF.Round(X), MathF.Round(Y));
    }

    public readonly Vec2 Round(int digits)
    {
        return new Vec2(MathF.Round(X, digits), MathF.Round(Y, digits));
    }

    public readonly Vec2 Round(MidpointRounding mode)
    {
        return new Vec2(MathF.Round(X, mode), MathF.Round(Y, mode));
    }

    public readonly Vec2 Round(int digits, MidpointRounding mode)
    {
        return new Vec2(MathF.Round(X, digits, mode), MathF.Round(Y, digits, mode));
    }

    public readonly Vec2 Floor()
    {
        return new Vec2(MathF.Floor(X), MathF.Floor(Y));
    }

    public readonly Vec2 Ceiling()
    {
        return new Vec2(MathF.Ceiling(X), MathF.Ceiling(Y));
    }

    #endregion

    #region Line

    public readonly bool HasGradientBetwen(Vec2 other)
    {
        return X != other.X;
    }

    public readonly float GradientBetween(Vec2 other)
    {
        float d = other.X - X;
        if (d == 0) return float.NaN;
        return (other.Y - Y) / d;
    }

    public readonly void LineBetween(Vec2 other, out float gradient, out float yIntercept)
    {
        gradient = GradientBetween(other);
        yIntercept = float.IsNaN(gradient) ? float.NaN : Y - gradient * X;
    }

    #endregion

    #region Utility

    public readonly void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
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