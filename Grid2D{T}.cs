using System.Text;
using System.Collections;

namespace SCENeo;

internal class Grid2D<T>(T[,] data) : IEnumerable<T>,
    ICloneable
{
    private readonly T[,] _data = data;

    public Grid2D(int width, int height)
        : this(new T[width, height])
    {
    }

    public int Width  => _data.GetLength(0);

    public int Height => _data.GetLength(1);

    public int Size   => _data.Length;

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

    public static implicit operator Grid2DView<T>(Grid2D<T> grid) => new(grid);

    public void Map(Grid2DView<T> source, Vec2I position, Rect2DI area)
    {
        var offsetArea = new Rect2DI()
        {
            Left   = area.Left   + position.X,
            Top    = area.Top    + position.Y,
            Right  = area.Right  + position.X,
            Bottom = area.Bottom + position.Y,
        };

        if (!offsetArea.Overlaps(0, 0, Width, Height))
        {
            return;
        }

        area.SetToOverlap(offsetArea);

        area.Left   -= position.X;
        area.Top    -= position.Y;
        area.Right  -= position.X;
        area.Bottom -= position.Y;

        for (int y = area.Top; y < area.Bottom; y++)
        {
            for (int x = area.Left; x < area.Right; x++)
            {
                this[position] = source[x, y];
                position.X++;
            }
            position.X = 0;
            position.Y++;
        }
    }

    public void Fill(T item)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                this[x, y] = item;
            }
        }
    }

    public void Clear()
    {
        Array.Clear(_data);
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
        return (IEnumerator<T>) _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        return new Grid2D<T>((T[,])_data.Clone());
    }

    #endregion
}