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

    public Grid2D()
       : this(new T[0, 0])
    {
    }

    public Grid2D(T[,] data)
    {
        _data = data;
    }

    public Grid2D(Grid2D<T> grid)
       : this(grid._data)
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

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <remarks>
    /// Setting the width will call <see cref="Resize(int, int)"/>.
    /// </remarks>
    public int Width
    {
        get => _data.GetLength(0);
        set => Resize(value, Height);
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <remarks>
    /// Setting the height will call <see cref="Resize(int, int)"/>.
    /// </remarks>
    public int Height
    {
        get => _data.GetLength(1);
        set => Resize(value, Width);
    }

    /// <summary>
    /// Gets the number of items in the grid.
    /// </summary>
    public int Length => _data.Length;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    /// <remarks>
    /// Setting the size will call <see cref="Resize(Vec2I)"/>.
    /// </remarks>
    public Vec2I Size
    {
        get => new Vec2I(Width, Height);
        set => Resize(value);
    }

    public T this[int x, int y]
    {
        get => _data[x, y];
        set => _data[x, y] = value;
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
        if (width == Width && height == Height)
        {
            return;
        }
        
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
    /// Attempts to clean resize if the width or height are different.
    /// </summary>
    public bool TryCleanResize(int width, int height)
    {
        if (width == Width || height == Height)
        {
            return false;
        }
        _data = new T[width, height];
        return true;
    }

    /// <summary>
    /// Attempts to clean resize if the width or height are different.
    /// </summary>
    public bool TryCleanResize(Vec2I size)
    {
        return TryCleanResize(size.X, size.Y);
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

    /// <summary>
    /// Returns a new grid rotating the view by 90 degrees clockwise.
    /// </summary>
    public static Grid2D<T> Rotated90C(IView<T> view)
    {
        var grid = new Grid2D<T>(view.Height, view.Width);

        if (grid.Length == 0)
        {
            return grid;
        }

        Vec2 midpoint = (view.Size() - Vec2.One) / 2;
        Vec2 change = midpoint.Inverted();

        foreach (Vec2I position in SCEUtils.EnumerateArea(view))
        {
            grid[(Vec2I)((position - midpoint).Rotated90C() + change)] = view[position];
        }

        return grid;
    }

    /// <summary>
    /// Returns a new grid rotating the view by 90 degrees anti-clockwise.
    /// </summary>
    public static Grid2D<T> Rotated90AC(IView<T> view)
    {
        var grid = new Grid2D<T>(view.Height, view.Width);

        if (grid.Length == 0)
        {
            return grid;
        }

        Vec2 midpoint = (view.Size() - Vec2.One) / 2;
        Vec2 change = midpoint.Inverted();

        foreach (Vec2I position in SCEUtils.EnumerateArea(view))
        {
            grid[(Vec2I)((position - midpoint).Rotated90AC() + change)] = view[position];
        }

        return grid;
    }

    /// <summary>
    /// Returns a new grid rotating the view by 180 degrees.
    /// </summary>
    public static Grid2D<T> Rotated180(IView<T> view)
    {
        var grid = new Grid2D<T>(view.Width, view.Height);

        if (grid.Length == 0)
        {
            return [];
        }

        Vec2 midpoint = (view.Size() - Vec2.One) / 2;

        foreach (Vec2I position in SCEUtils.EnumerateArea(view))
        {
            grid[(Vec2I)((position - midpoint).Rotated180() + midpoint)] = view[position];
        }

        return grid;
    }

    /// <summary>
    /// Rotates this grid 90 degrees clockwise.
    /// </summary>
    public void Rotate90C()
    {
        _data = Rotated90C(this)._data;
    }

    /// <summary>
    /// Rotates this grid 90 degrees anti-clockwise.
    /// </summary>
    public void Rotate90AC()
    {
        _data = Rotated90AC(this)._data;
    }

    /// <summary>
    /// Rotates this grid 180 degrees.
    /// </summary>
    public void Rotate180()
    {
        _data = Rotated180(this)._data;
    }

    /// <summary>
    /// Returns a new grid flipping the view horizontally.
    /// </summary>
    public static Grid2D<T> FlippedHorizontal(IView<T> view)
    {
        var grid = new Grid2D<T>(view.Width, view.Height);

        foreach (Vec2I position in SCEUtils.EnumerateArea(view))
        {
            grid[grid.Width - position.X - 1, position.Y] = view[position];
        }

        return grid;
    }

    /// <summary>
    /// Returns a new grid flipping the view vertically.
    /// </summary>
    public static Grid2D<T> FlippedVertical(IView<T> view)
    {
        var grid = new Grid2D<T>(view.Width, view.Height);

        foreach (Vec2I position in SCEUtils.EnumerateArea(view))
        {
            grid[grid.Width - position.X - 1, position.Y] = view[position];
        }

        return grid;
    }

    /// <summary>
    /// Flips the grid values horizontally.
    /// </summary>
    public void FlipHorizontal()
    {
        int endX = Width / 2;

        for (int x = 0; x < endX; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SCEUtils.Swap(ref _data[x, y], ref _data[Width - x - 1, y]);
            }
        }
    }

    /// <summary>
    /// Flips the grid values vertically.
    /// </summary>
    public void FlipVertical()
    {
        int endY = Height / 2;

        for (int y = 0; y < endY; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SCEUtils.Swap(ref _data[x, y], ref _data[x, Height - y - 1]);
            }
        }
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