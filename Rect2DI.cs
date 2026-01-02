using System.Collections;

namespace SCENeo;

/// <summary>
/// A struct representing a 2D integer rectangular area.
/// </summary>
public struct Rect2DI : IEquatable<Rect2DI>,
    IEnumerable<Vec2I>
{
    /// <summary>
    /// The left boundary.
    /// </summary>
    public int Left;

    /// <summary>
    /// The top boundary.
    /// </summary>
    public int Top;

    /// <summary>
    /// The right boundary.
    /// </summary>
    public int Right;

    /// <summary>
    /// The bottom boundary.
    /// </summary>
    public int Bottom;

    public Rect2DI(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public Rect2DI(Vec2I start, Vec2I end) 
        : this(start.X, start.Y, end.X, end.Y)
    { 
    }

    public Rect2DI(Vec2I size) 
        : this(Vec2I.Zero, size)
    { 
    }

    public Rect2DI(int width, int height) 
        : this(0, 0, width, height) 
    { 
    }

    /// <summary>
    /// Creates an area from a position and a size.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Area(int x, int y, int width, int height)
    {
        return new Rect2DI(x, y, x + width, y + height);
    }

    /// <summary>
    /// Creates an area from a position and a size.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Area(Vec2I position, int width, int height)
    {
        return new Rect2DI(position.X, position.Y, position.X + width, position.Y + height);
    }

    /// <summary>
    /// Creates an area from a position and a size.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <param name="size">The size.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Area(int x, int y, Vec2I size)
    {
        return new Rect2DI(x, y, x + size.X, y + size.Y);
    }

    /// <summary>
    /// Creates an area from a position and a size.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="size">The size.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Area(Vec2I position, Vec2I size)
    {
        return new Rect2DI(position, position + size);
    }

    /// <summary>
    /// Creates a single wide area from a position and a height.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <param name="height">The height of the area.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Vertical(int x, int y, int height)
    {
        return new Rect2DI(x, y, x + 1, y + height);
    }

    /// <summary>
    /// Creates a single high area from a position and a width.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <param name="width">The width of the area.</param>
    /// <returns>The resulting area.</returns>
    public static Rect2DI Horizontal(int x, int y, int width)
    {
        return new Rect2DI(x, y, x + width, y + 1);
    }

    public static bool operator ==(Rect2DI r1, Rect2DI r2) => r1.Equals(r2);
    public static bool operator !=(Rect2DI r1, Rect2DI r2) => !r1.Equals(r2);

    /// <summary>
    /// Gets the start position of the area.
    /// </summary>
    public readonly Vec2I Start { get { return new Vec2I(Left, Top); } }

    /// <summary>
    /// Gets the end position of the area.
    /// </summary>
    public readonly Vec2I End { get { return new Vec2I(Right, Bottom); } }

    /// <summary>
    /// Returns the size of the area (equivalent to End - Start).
    /// </summary>
    /// <returns>The resulting size.</returns>
    public readonly Vec2I Size()
    {
        return End - Start;
    }

    /// <inheritdoc cref="Overlaps(Rect2DI)"/>
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

    /// <inheritdoc cref="Overlaps(Rect2DI)"/>
    public readonly bool Overlaps(Vec2I start, Vec2I end)
    {
        return Overlaps(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Determines whether this area overlaps the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns><see langword="true"/> if the areas overlap; otherwise, <see langword="false"/>.</returns>
    public readonly bool Overlaps(Rect2DI other)
    {
        return Overlaps(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <inheritdoc cref="Trim(Rect2DI)"/>
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

    /// <inheritdoc cref="Trim(Rect2DI)"/>
    public readonly Rect2DI Trim(Vec2I start, Vec2I end)
    {
        return Trim(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Returns the overlap between this area and the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns>The overlaping area.</returns>
    public readonly Rect2DI Trim(Rect2DI other)
    {
        return Trim(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <inheritdoc cref="Encloses(Rect2DI)"/>
    public readonly bool Encloses(int left, int top, int right, int bottom)
    {
        return left >= Left && right <= Right && top >= Top && bottom <= Bottom;
    }

    /// <inheritdoc cref="Encloses(Rect2DI)"/>
    public readonly bool Encloses(Vec2I start, Vec2I end)
    {
        return Encloses(start.X, start.Y, end.X, end.Y);
    }

    /// <summary>
    /// Determines whether this area fully encloses the other area.
    /// </summary>
    /// <param name="other">The other area.</param>
    /// <returns><see langword="true"/> if this area fully encloses the other area; otherwise, <see langword="false"/>.</returns>
    public readonly bool Encloses(Rect2DI other)
    {
        return Encloses(other.Left, other.Top, other.Right, other.Bottom);
    }

    /// <summary>
    /// Determines whether this area encloses the given position.
    /// </summary>
    /// <param name="x">The position x-component.</param>
    /// <param name="y">The position y-component.</param>
    /// <returns><see langword="true"/> if this area encloses the given position; otherwise, <see langword="false"/>.</returns>
    public readonly bool HasPoint(int x, int y)
    {
        return x >= Left && y >= Top && x < Right  && y < Bottom;
    }

    /// <summary>
    /// Determines whether this area encloses the given position.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns><see langword="true"/> if this area encloses the given position; otherwise, <see langword="false"/>.</returns>
    public readonly bool HasPoint(Vec2I position)
    {
        return position.X >= Left && position.Y >= Top && position.X < Right && position.Y < Bottom;
    }

    public IEnumerator<Vec2I> GetEnumerator()
    {
        for (int y = Top; y < Bottom; y++)
        {
            for (int x = Left; x < Right; x++)
            {
                yield return new Vec2I(x, y);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

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

    public readonly override string ToString()
    {
        return $"[{Start}, {End}]";
    }
}