using SCENeo.Ui;

namespace SCENeo;

/// <summary>
/// A struct representing a 2D single-precision floating-point vector.
/// </summary>
public partial struct Vec2 : IEquatable<Vec2>
{
    /// <summary>
    /// The x-component of this vector.
    /// </summary>
    public float X;

    /// <summary>
    /// The y-component of this vector.
    /// </summary>
    public float Y;

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static explicit operator Vec2I(Vec2 v) => new((int)v.X, (int)v.Y);

    public static bool operator ==(Vec2 v1, Vec2 v2) => v1.Equals(v2);
    public static bool operator !=(Vec2 v1, Vec2 v2) => !(v1 == v2);

    public static Vec2 operator +(Vec2 v1, Vec2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);
    public static Vec2 operator +(Vec2 v1, float num ) => new(v1.X + num, v1.Y + num);

    public static Vec2 operator -(Vec2 v1, Vec2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);
    public static Vec2 operator -(Vec2 v1, float num) => new(v1.X - num, v1.Y - num);
    public static Vec2 operator -(Vec2 v) => new(-v.X, -v.Y);

    public static Vec2 operator *(Vec2 v1, Vec2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);
    public static Vec2 operator *(Vec2 v1, float num) => new(v1.X * num, v1.Y * num);

    public static Vec2 operator /(Vec2 v1, Vec2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);
    public static Vec2 operator /(Vec2 v1, float num) => new(v1.X / num, v1.Y / num);

    public static bool operator <(Vec2 v1, Vec2 v2) => v1.X < v2.X && v1.Y < v2.Y;
    public static bool operator >(Vec2 v1, Vec2 v2) => v1.X > v2.X && v1.Y > v2.Y;

    public static bool operator <=(Vec2 v1, Vec2 v2) => v1.X <= v2.X && v1.Y <= v2.Y;
    public static bool operator >=(Vec2 v1, Vec2 v2) => v1.X >= v2.X && v1.Y >= v2.Y;

    /// <summary>
    /// Returns the length of this vector.
    /// </summary>
    /// <returns>The length of this vector.</returns>
    public readonly float Length()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    /// Returns the length squared of this vector.
    /// </summary>
    /// <returns>The length squared.</returns>
    public readonly float LengthSquared()
    {
        return X * X + Y * Y;
    }

    /// <summary>
    /// Returns this vector with the absolute value of both components.
    /// </summary>
    /// <returns>The absolute vector.</returns>
    public readonly Vec2 Abs()
    {
        return new Vec2(Math.Abs(X), Math.Abs(Y));
    }

    /// <summary>
    /// Returns this vector with the x and y components swapped.
    /// </summary>
    /// <returns>The inverted vector.</returns>
    public readonly Vec2 Inverted()
    {
        return new Vec2(Y, X);
    }

    /// <summary>
    /// Returns the distance between this vector and <paramref name="other"/>.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The distance between.</returns>
    public readonly float DistanceTo(Vec2 other)
    {
        return (other - this).Length();
    }

    /// <summary>
    /// Returns the normalized vector or zero if the length is zero.
    /// </summary>
    /// <returns>The normalized vector</returns>
    public readonly Vec2 Normalized()
    {
        float l = LengthSquared();

        if (l == 0f)
        {
            return Zero;
        }

        l = MathF.Sqrt(l);

        return new Vec2(X / l, Y / l);
    }

    /// <summary>
    /// Returns the angle in radians between this vector and the x-axis.
    /// </summary>
    /// <returns>The resulting angle in radians.</returns>
    public readonly float Angle()
    {
        return MathF.Atan(Y / X);
    }

    /// <summary>
    /// Returns the normalized vector direction between this vector and <paramref name="other"/> 
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The normalized vector direction.</returns>
    public readonly Vec2 DirectionTo(Vec2 other)
    {
        return (other - this).Normalized();
    }

    /// <summary>
    /// Returns the dot product between this vector and <paramref name="other"/>.
    /// </summary>
    /// <param name="other">The other vector.</param>
    /// <returns>The dot product.</returns>
    public readonly float Dot(Vec2 other)
    {
        return X * other.X + Y * other.Y;
    }

    /// <summary>
    /// Returns this vector truncated to the given length or unchanged if the length is already less.
    /// </summary>
    /// <param name="maxLength">The length to shorten to.</param>
    /// <returns>The truncated vector.</returns>
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

    /// <summary>
    /// Returns this vector rotated by the given rotation in radians.
    /// </summary>
    /// <param name="rotation">The rotation in radians.</param>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2 Rotated(float rotation)
    {
        return new Vec2()
        {
            X = X * MathF.Cos(rotation) - Y * MathF.Sin(rotation),
            Y = X * MathF.Sin(rotation) + Y * MathF.Cos(rotation),
        };
    }

    /// <summary>
    /// Returns this vector rotated 90 degrees clockwise.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2 Rotated90C()
    {
        return new Vec2(-Y, X);
    }

    /// <summary>
    /// Returns this vector rotated 90 degrees anti-clockwise.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2 Rotated90AC()
    {
        return new Vec2(Y, -X);
    }

    /// <summary>
    /// Returns this vector rotated 180 degrees.
    /// </summary>
    /// <returns>The rotated vector.</returns>
    public readonly Vec2 Rotated180()
    {
        return new Vec2(-X, -Y);
    }

    /// <summary>
    /// Returns the vector rotated by the given rotation enum.
    /// </summary>
    /// <param name="rotation">The rotation.</param>
    /// <returns>The rotated vector.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public readonly Vec2 Rotated(Rotation rotation)
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
    /// Returns this vector with each component rounded by <see cref="MathF.Round(float)"/>.
    /// </summary>
    /// <returns>The rounded vector.</returns>
    public readonly Vec2 Round()
    {
        return new Vec2(MathF.Round(X), MathF.Round(Y));
    }

    /// <summary>
    /// Returns this vector with each component rounded by <see cref="MathF.Round(float, int)"/>.
    /// </summary>
    /// <param name="digits">How many fractional digits to keep.</param>
    /// <returns>The rounded vector.</returns>
    public readonly Vec2 Round(int digits)
    {
        return new Vec2(MathF.Round(X, digits), MathF.Round(Y, digits));
    }

    /// <summary>
    /// Returns this vector with each component rounded by <see cref="MathF.Round(float, MidpointRounding)"/>.
    /// </summary>
    /// <param name="mode">The rounding convention to use.</param>
    /// <returns>The rounded vector.</returns>
    public readonly Vec2 Round(MidpointRounding mode)
    {
        return new Vec2(MathF.Round(X, mode), MathF.Round(Y, mode));
    }

    /// <summary>
    /// Returns this vector with each component rounded by <see cref="MathF.Round(float, int, MidpointRounding)"/>.
    /// </summary>
    /// <param name="digits">How many fractional digits to keep.</param>
    /// <param name="mode">The rounding convention to use.</param>
    /// <returns>The rounded vector.</returns>
    public readonly Vec2 Round(int digits, MidpointRounding mode)
    {
        return new Vec2(MathF.Round(X, digits, mode), MathF.Round(Y, digits, mode));
    }

    public readonly Vec2 RoundedToPixel()
    {
        return (this * new Vec2(2, 1)).Round().ToVec2I() / new Vec2(2, 1);
    }

    /// <summary>
    /// Returns this vector with each component floored by <see cref="MathF.Floor(float)"/>.
    /// </summary>
    /// <returns>The floored vector.</returns>
    public readonly Vec2 Floor()
    {
        return new Vec2(MathF.Floor(X), MathF.Floor(Y));
    }

    /// <summary>
    /// Returns this vector with each component ceiled by <see cref="MathF.Ceiling(float)"/>.
    /// </summary>
    /// <returns>The ceiled vector.</returns>
    public readonly Vec2 Ceiling()
    {
        return new Vec2(MathF.Ceiling(X), MathF.Ceiling(Y));
    }

    /// <summary>
    /// Returns the gradient between this vector and <paramref name="other"/> or <see cref="float.NaN"/> if there is no gradient.
    /// </summary>
    /// <param name="other">The other point.</param>
    /// <returns>The gradient or <see cref="float.NaN"/> if there is no gradient.</returns>
    public readonly float GradientBetween(Vec2 other)
    {
        float deltaX = other.X - X;

        if (deltaX == 0)
        {
            return float.NaN;
        }

        return (other.Y - Y) / deltaX;
    }

    public readonly void LineBetween(Vec2 other, out float gradient, out float yIntercept)
    {
        gradient = GradientBetween(other);
        yIntercept = float.IsNaN(gradient) ? float.NaN : Y - gradient * X;
    }

    /// <summary>
    /// Deconstructs this vector.
    /// </summary>
    /// <param name="x">The x-component.</param>
    /// <param name="y">The y-component.</param>
    public readonly void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }

    public readonly Vec2I ToVec2I()
    {
        return new Vec2I((int)X, (int)Y);
    }

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

    public readonly override string ToString()
    {
        return $"{{ {X}, {Y} }}";
    }
}