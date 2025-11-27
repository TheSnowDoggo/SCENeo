using System.Text;
using System.Collections;
using SCENeo.Utils;

namespace SCENeo;

public class Grid2D<T>(T[,] data) : IEnumerable<T>,
    ICloneable, IDimensioned, IResizeable
{
    public Grid2D() : this(new T[0, 0]) { }
    public Grid2D(int width, int height) : this(new T[width, height]) { }
    public Grid2D(Vec2I dimensions) : this(dimensions.X, dimensions.Y) { }

    public T[,] Data = data;

    public int Width { get { return Data.GetLength(0); } }

    public int Height { get { return Data.GetLength(1); } }

    public int Size { get { return Data.Length; } }

    public Vec2I Dimensions { get { return new Vec2I(Width, Height); } }

    public T this[int x, int y]
    {
        get { return Data[x, y]; }
        set { Data[x, y] = value; }
    }

    public T this[Vec2I pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    public static implicit operator Grid2DView<T>(Grid2D<T> grid) => new Grid2DView<T>(grid);

    #region Map

    public void Map(IView<T> view, Vec2I position, Rect2DI area)
    {
        Vec2I difference = area.Start - position;

        Rect2DI trim = area.Trim(difference, Dimensions + difference);

        position += trim.Start;

        for (int viewY = trim.Top, thisY = position.Y; viewY < trim.Bottom; viewY++, thisY++)
        {
            for (int viewX = trim.Left, thisX = position.X; viewX < trim.Right; viewX++, thisX++)
            {
                this[thisX, thisY] = view[viewX, viewY];
            }
        }
    }

    public void Map(Grid2DView<T> view, Vec2I position, Rect2DI area)
    {
        Map((IView<T>)view, position, area);
    }

    public void Map(IView<T> view, Rect2DI area)
    {
        Map(view, Vec2I.Zero, area);
    }

    public void Map(Grid2DView<T> view, Rect2DI area)
    {
        Map((IView<T>)view, area);
    }

    public void Map(IView<T> view, Vec2I position)
    {
        Map(view, position, view.Area());
    }

    public void Map(Grid2DView<T> view, Vec2I position)
    {
        Map((IView<T>)view, position);
    }

    public void Map(IView<T> view)
    {
        Map(view, Vec2I.Zero);
    }

    public void Map(Grid2DView<T> view)
    {
        Map((IView<T>)view);
    }

    #endregion

    #region Fill

    public void Fill(T item, Rect2DI area)
    {
        for (int y = area.Start.Y; y < area.End.Y; y++)
            for (int x = area.Start.X; x < area.End.X; x++)
                this[x, y] = item;
    }

    public void Fill(T item)
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                this[x, y] = item;
    }

    public void Fill(Func<int, int, T> item, Rect2DI area)
    {
        for (int y = area.Start.Y; y < area.End.Y; y++)
            for (int x = area.Start.X; x < area.End.X; x++)
                this[x, y] = item.Invoke(x, y);
    }

    public void Fill(Func<int, int, T> item)
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                this[x, y] = item.Invoke(x, y);
    }

    public void Fill(Func<T> item, Rect2DI area)
    {
        for (int y = area.Start.Y; y < area.End.Y; y++)
            for (int x = area.Start.X; x < area.End.X; x++)
                this[x, y] = item.Invoke();
    }

    public void Fill(Func<T> item)
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                this[x, y] = item.Invoke();
    }

    #endregion

    #region Resize

    public void Resize(int width, int height)
    {
        var newData = new T[width, height];

        int minWidth  = Math.Min(width, Width);
        int minHeight = Math.Min(height, Height);

        for (int y = 0; y < minHeight; y++)
        {
            for (int x = 0; x < minWidth; x++)
            {
                newData[x, y] = this[x, y];
            }
        }

        Data = newData;
    }

    public void CleanResize(int width, int height)
    {
        Data = new T[width, height];
    }

    #endregion

    public Grid2DView<T> AsView()
    {
        return new Grid2DView<T>(this);
    }

    public Rect2DI Area()
    {
        return new Rect2DI(Width, Height);
    }

    public void Clear()
    {
        Array.Clear(Data);
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

    #region IEnumerable

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T item in Data)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        return new Grid2D<T>((T[,])Data.Clone());
    }

    #endregion
}