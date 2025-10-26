namespace SCENeo;

public struct Rect2DI(int left, int top, int right, int bottom) : IEquatable<Rect2DI>
{
    public int Left   = left;
    public int Top    = top;
    public int Right  = right;
    public int Bottom = bottom;

    public Rect2DI(Vec2I start, Vec2I end)
        : this(start.X, start.Y, end.X, end.Y)
    {
    }

    public Rect2DI(Vec2I dimensions)
        : this(Vec2I.Zero, dimensions)
    {
    }

    public Rect2DI(int width, int height)
        : this(0, 0, width, height)
    {
    }

    #region Operators

    public static bool operator ==(Rect2DI r1, Rect2DI r2) => r1.Equals(r2);

    public static bool operator !=(Rect2DI r1, Rect2DI r2) => !r1.Equals(r2);

    #endregion

    public readonly Vec2I Start { get { return new Vec2I(Left, Top); } }

    public readonly Vec2I End { get { return new Vec2I(Right, Bottom); } }

    public readonly Vec2I Size()
    {
        return End - Start;
    }

    #region Overlaps

    public readonly bool Overlaps(int left, int top, int right, int bottom)
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

    public readonly bool Overlaps(Vec2I start, Vec2I end)
    {
        return Overlaps(start.X, start.Y, end.X, end.Y);
    }

    public readonly bool Overlaps(Rect2DI other)
    {
        return Overlaps(other.Start, other.End);
    }

    #endregion

    #region Trim

    public readonly Rect2DI Trim(int left, int top, int right, int bottom)
    {
        return new Rect2DI()
        {
            Left   = Math.Max(Left  , left  ),
            Top    = Math.Max(Top   , top   ),
            Right  = Math.Min(Right , right ),
            Bottom = Math.Min(Bottom, bottom),
        };
    }

    public readonly Rect2DI Trim(Vec2I start, Vec2I end)
    {
        return Trim(start.X, start.Y, end.X, end.Y);
    }

    public readonly Rect2DI Trim(Rect2DI other)
    {
        return Trim(other.Left, other.Top, other.Right, other.Bottom);
    }

    #endregion

    #region Contains

    public readonly bool Contains(int left, int top, int right, int bottom)
    {
        return left >= Left && right <= Right && top >= Top && bottom <= Bottom;
    }

    public readonly bool Contains(Vec2I start, Vec2I end)
    {
        return Contains(start.X, start.Y, end.X, end.Y);
    }

    public readonly bool Contains(Rect2DI rect)
    {
        return Contains(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }

    public readonly bool Contains(int x, int y)
    {
        return x >= Left && x <= Right && y >= Top && y <= Bottom;
    }

    public readonly bool Contains(Vec2I position)
    {
        return Contains(position.X, position.Y);
    }

    #endregion

    #region Equality

    public readonly bool Equals(Rect2DI other)
    {
        return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Vec2I vec && Equals(vec);
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