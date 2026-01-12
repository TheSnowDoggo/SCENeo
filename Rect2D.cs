namespace SCENeo;

/// <summary>
/// A struct representing a 2D single-precision floating-point rectangular area.
/// </summary>
public struct Rect2D : IEquatable<Rect2D>
{
    /// <summary>
    /// The left boundary.
    /// </summary>
    public float Left;

    /// <summary>
    /// The top boundary.
    /// </summary>
    public float Top;

    /// <summary>
    /// The right boundary.
    /// </summary>
    public float Right;

    /// <summary>
    /// The bottom boundary.
    /// </summary>
    public float Bottom;

    public Rect2D(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public Rect2D(Vec2 start, Vec2 end) 
        : this(start.X, start.Y, end.X, end.Y) 
    {
    }

    public Rect2D(Vec2 dimensions) 
        : this(Vec2.Zero, dimensions) 
    { 
    }

    public Rect2D(float width, float height) 
        : this(0, 0, width, height) 
    { 
    }

    public static bool operator ==(Rect2D r1, Rect2D r2) => r1.Equals(r2);
    public static bool operator !=(Rect2D r1, Rect2D r2) => !r1.Equals(r2);

    public static Rect2D operator +(Rect2D r, Vec2 v) => new Rect2D(r.Left + v.X, r.Top + v.Y, r.Right + v.X, r.Bottom + v.Y);
    public static Rect2D operator -(Rect2D r, Vec2 v) => new Rect2D(r.Left - v.X, r.Top - v.Y, r.Right - v.X, r.Bottom - v.Y);

    /// <summary>
    /// Gets the start position of the area.
    /// </summary>
    public readonly Vec2 Start { get { return new Vec2(Left, Top); } }

    /// <summary>
    /// Gets the end position of the area.
    /// </summary>
    public readonly Vec2 End { get { return new Vec2(Right, Bottom); } }

    public static Rect2D FromPoints(Vec2 a, Vec2 b)
    {
        return new Rect2D()
        {
            Left   = MathF.Min(a.X, b.X),
            Top    = MathF.Min(a.Y, b.Y),
            Right  = MathF.Max(a.X, b.X),
            Bottom = MathF.Max(a.Y, b.Y),
        };
    }

    /// <summary>
    /// Returns the size of the area (equivalent to End - Start).
    /// </summary>
    /// <returns>The resulting size.</returns>
    public readonly Vec2 Size()
    {
        return End - Start;
    }

    /// <inheritdoc cref="Overlaps(Rect2D)"/>
    public readonly bool Overlaps(float left, float top, float right, float bottom)
    {
        if (Right <= left || Left >= right)
        {
            return false;
        }
        if (Bottom <= top || Top >= bottom)
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc cref="Overlaps(Rect2D)"/>
    public readonly bool Overlaps(Vec2 start, Vec2 end)
    {
        return Overlaps(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Determines whether this area overlaps the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns><see langword="true"/> if the areas overlap; otherwise, <see langword="false"/>.</returns>
    public readonly bool Overlaps(Rect2D other)
    {
        return Overlaps(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <inheritdoc cref="Trim(Rect2D)"/>
    public readonly Rect2D Trim(float left, float top, float right, float bottom)
    {
        return new Rect2D()
        {
            Left   = Math.Max(Left  , left  ),
            Top    = Math.Max(Top   , top   ),
            Right  = Math.Min(Right , right ),
            Bottom = Math.Min(Bottom, bottom),
        };
    }

    /// <inheritdoc cref="Trim(Rect2D)"/>
    public readonly Rect2D Trim(Vec2 start, Vec2 end)
    {
        return Trim(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Returns the overlap between this area and the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns>The overlaping area.</returns>
    public readonly Rect2D Trim(Rect2D other)
    {
        return Trim(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <inheritdoc cref="Encloses(Rect2D)"/>
    public readonly bool Encloses(float left, float top, float right, float bottom)
    {
        return left >= Left && right <= Right && top >= Top && bottom <= Bottom;
    }

    /// <inheritdoc cref="Encloses(Rect2D)"/>
    public readonly bool Encloses(Vec2 start, Vec2 end)
    {
        return Encloses(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Determines whether this area fully encloses the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns><see langword="true"/> if this area fully encloses the other area; otherwise, <see langword="false"/>.</returns>
    public readonly bool Encloses(Rect2D other)
    {
        return Encloses(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <summary>
    /// Determines whether this area encloses the given position.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <returns><see langword="true"/> if this area encloses the given position; otherwise, <see langword="false"/>.</returns>
    public readonly bool HasPoint(float x, float y)
    {
        return x >= Left && x <= Right && y >= Top && y <= Bottom;
    }

    /// <summary>
    /// Determines whether this area encloses the given position.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns><see langword="true"/> if this area encloses the given position; otherwise, <see langword="false"/>.</returns>
    public readonly bool HasPoint(Vec2 position)
    {
        return position.X >= Left && position.Y >= Top && position.X <= Right && position.Y <= Bottom;
    }

    public readonly bool Equals(Rect2D other)
    {
        return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vec2 vec && Equals(vec);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Left, Top, Right, Bottom);
    }

    public readonly override string ToString()
    {
        return $"[{Start}, {End}]";
    }
}