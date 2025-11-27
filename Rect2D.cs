namespace SCENeo;

public struct Rect2D(float left, float top, float right, float bottom) : IEquatable<Rect2D>
{
    public float Left   = left;
    public float Top    = top;
    public float Right  = right;
    public float Bottom = bottom;

    public Rect2D(Vec2 start, Vec2 end) : this(start.X, start.Y, end.X, end.Y) { }
    public Rect2D(Vec2 dimensions) : this(Vec2.Zero, dimensions) { }
    public Rect2D(float width, float height) : this(0, 0, width, height) { }

    #region Operators

    public static bool operator ==(Rect2D r1, Rect2D r2) => r1.Equals(r2);

    public static bool operator !=(Rect2D r1, Rect2D r2) => !r1.Equals(r2);

    public static Rect2D operator +(Rect2D r, Vec2 v) => new Rect2D(r.Left + v.X, r.Top + v.Y, r.Right + v.X, r.Bottom + v.Y);

    public static Rect2D operator -(Rect2D r, Vec2 v) => new Rect2D(r.Left - v.X, r.Top - v.Y, r.Right - v.X, r.Bottom - v.Y);

    #endregion

    public readonly Vec2 Start { get { return new Vec2(Left, Top); } }

    public readonly Vec2 End { get { return new Vec2(Right, Bottom); } }

    public readonly Vec2 Size()
    {
        return End - Start;
    }

    #region Overlaps

    public readonly bool Overlaps(float left, float top, float right, float bottom)
    {
        if (Right < left || Left > right)
        {
            return false;
        }
        if (Bottom < top || Top > bottom)
        {
            return false;
        }
        return true;
    }

    public readonly bool Overlaps(Vec2 start, Vec2 end)
    {
        return Overlaps(start.X, start.Y, end.X, end.Y);
    }

    public readonly bool Overlaps(Rect2D other)
    {
        return Overlaps(other.Left, other.Top, other.Right, other.Bottom);
    }

    #endregion

    #region Trim

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

    public readonly Rect2D Trim(Vec2 start, Vec2 end)
    {
        return Trim(start.X, start.Y, end.X, end.Y);
    }

    public readonly Rect2D Trim(Rect2D other)
    {
        return Trim(other.Start, other.End);
    }

    #endregion

    #region Contains

    public readonly bool Contains(float left, float top, float right, float bottom)
    {
        return left >= Left && right <= Right && top >= Top && bottom <= Bottom;
    }

    public readonly bool Contains(Vec2 start, Vec2 end)
    {
        return Contains(start.X, start.Y, end.X, end.Y);
    }

    public readonly bool Contains(Rect2D rect)
    {
        return Contains(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }

    public readonly bool Contains(float x, float y)
    {
        return x >= Left && x <= Right && y >= Top && y <= Bottom;
    }

    public readonly bool Contains(Vec2 position)
    {
        return Contains(position.X, position.Y);
    }

    #endregion

    #region Equality

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

    #endregion

    public readonly override string ToString()
    {
        return $"[{Start}, {End}]";
    }
}