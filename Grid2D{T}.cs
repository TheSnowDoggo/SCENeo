using System.Text;
using System.Collections;

namespace SCENeo;

/// <summary>
/// A wrapper class around a 2D array,
/// </summary>
/// <typeparam name="T">The stored type.</typeparam>
public class Grid2D<T> : IView<T>, IEnumerable<T>,
    ICloneable, IDimensioned
{
    private T[,] _data;

    public Grid2D(T[,] data)
    {
        _data = data;
    }

    public Grid2D(Grid2D<T> grid)
       : this(grid._data)
    {
    }

    public Grid2D() 
        : this(new T[0, 0]) 
    { 
    }

    public Grid2D(int width, int height)
        : this(new T[width, height]) 
    { 
    }

    public Grid2D(Vec2I size) 
        : this(size.X, size.Y)
    { 
    }

    public int Width { get { return _data.GetLength(0); } }

    public int Height { get { return _data.GetLength(1); } }

    /// <summary>
    /// Gets the number of items in the grid.
    /// </summary>
    public int Length { get { return _data.Length; } }

    public Vec2I Size { get { return new Vec2I(Width, Height); } }

    public T this[int x, int y]
    {
        get { return _data[x, y]; }
        set { _data[x, y] = value; }
    }

    public T this[Vec2I pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    /// <summary>
    /// Maps the <paramref name="view"/> onto this grid.
    /// </summary>
    /// <param name="view">The view to read from.</param>
    /// <param name="position">The offset on this grid.</param>
    /// <param name="area">The area on the <paramref name="view"/> to map.</param>
    public void Map(IView<T> view, Vec2I position, Rect2DI area)
    {
        area = area.Trim(0, 0, view.Width, view.Height);

        Vec2I start = area.Start - position;

        area = area.Trim(start, Size + start);

        foreach (Vec2I viewPosition in area)
        {
            this[viewPosition - start] = view[viewPosition];
        }
    }

    /// <inheritdoc cref="Map(IView{T}, Vec2I, Rect2DI)"/>
    public void Map(IView<T> view, Rect2DI area)
    {
        Map(view, Vec2I.Zero, area);
    }

    /// <inheritdoc cref="Map(IView{T}, Vec2I, Rect2DI)"/>
    public void Map(IView<T> view, Vec2I position)
    {
        Map(view, position, view.Area());
    }

    /// <inheritdoc cref="Map(IView{T}, Vec2I, Rect2DI)"/>
    public void Map(IView<T> view)
    {
        Map(view, Vec2I.Zero);
    }

    /// <inheritdoc cref="Fill(T, Rect2DI)"/>
    public void Fill(T item, int left, int top, int right, int bottom)
    {
        for (int y = top; y < bottom; y++)
        {
            for (int x = left; x < right; x++)
            {
                this[x, y] = item;
            }
        }
    }

    /// <summary>
    /// Fills this grid with the given item inside the given area.
    /// </summary>
    /// <param name="item">The item to fill with.</param>
    /// <param name="area">The area to fill inside.</param>
    public void Fill(T item, Rect2DI area)
    {
        Fill(item, area.Left, area.Top, area.Right, area.Bottom);
    }

    /// <summary>
    /// Fills the entire grid with the given item.
    /// </summary>
    /// <param name="item">The item to fill with.</param>
    public void Fill(T item)
    {
        Fill(item, 0, 0, Width, Height);
    }

    /// <inheritdoc cref="Fill(Func{int, int, T}, Rect2DI)"/>
    public void Fill(Func<int, int, T> callback, int left, int top, int right, int bottom)
    {
        for (int y = top; y < bottom; y++)
        {
            for (int x = left; x < right; x++)
            {
                this[x, y] = callback.Invoke(x, y);
            }
        }
    }

    /// <summary>
    /// Fills this grid from the callback function inside the given area.
    /// </summary>
    /// <param name="callback">The callback function.</param>
    /// <param name="area">The area to fill inside.</param>
    public void Fill(Func<int, int, T> callback, Rect2DI area)
    {
        Fill(callback, area.Left, area.Top, area.Right, area.Bottom);
    }

    /// <summary>
    /// Fills the entire grid from the callback function.
    /// </summary>
    /// <param name="callback">The callback function.</param>
    public void Fill(Func<int, int, T> callback)
    {
        Fill(callback, 0, 0, Width, Height);
    }

    /// <summary>
    /// Resizes the grid to the new size copying over any data that fits.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    public void Resize(int width, int height)
    {
        var newData = new T[width, height];

        int minWidth  = Math.Min(width , Width );
        int minHeight = Math.Min(height, Height);

        for (int y = 0; y < minHeight; y++)
        {
            for (int x = 0; x < minWidth; x++)
            {
                newData[x, y] = this[x, y];
            }
        }

        _data = newData;
    }

    /// <summary>
    /// Resizes the grid to the new size copying over any data that fits.
    /// </summary>
    /// <param name="size">The new size.</param>
    public void Resize(Vec2I size)
    {
        Resize(size.X, size.Y);
    }

    /// <summary>
    /// Resizes the grid clearing all previous data.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    public void CleanResize(int width, int height)
    {
        _data = new T[width, height];
    }

    /// <summary>
    /// Resizes the grid clearing all previous data.
    /// </summary>
    /// <param name="size">The new size.</param>
    public void CleanResize(Vec2I size)
    {
        CleanResize(size.X, size.Y);
    }

    /// <summary>
    /// Returns a <see cref="Grid2DView{T}"/> containing a reference to this grid.
    /// </summary>
    /// <returns>A readonly view.</returns>
    public Grid2DView<T> AsReadonly()
    {
        return new Grid2DView<T>(this);
    }

    /// <summary>
    /// Gets the zero-based area of the grid.
    /// </summary>
    /// <returns>The zero-based area of the grid.</returns>
    public Rect2DI Area()
    {
        return new Rect2DI(Width, Height);
    }

    /// <summary>
    /// Clears the contents of the grid.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_data);
    }

    /// <summary>
    /// Translates the given index to a position.
    /// </summary>
    /// <param name="index">The index to translate.</param>
    /// <returns>The resulting position.</returns>
    public Vec2I ToPosition(int index)
    {
        return new Vec2I(index % Width, index / Width);
    }

    /// <summary>
    /// Translates the given position to an index.
    /// </summary>
    /// <param name="position">The position to translate.</param>
    /// <returns>The resulting index.</returns>
    public int ToIndex(Vec2I position)
    {
        return position.Y * Width + position.X;
    }

    public object Clone()
    {
        return new Grid2D<T>((T[,])_data.Clone());
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T item in _data)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        var sb = new StringBuilder("[ { ");

        for (int y = 0; y < Height; y++)
        {
            if (y != 0)
            {
                sb.Append(" }, { ");
            }

            for (int x = 0; x < Width; x++)
            {
                if (x != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(this[x, y]);
            }
        }

        sb.Append(" } ]");

        return sb.ToString();
    }
}