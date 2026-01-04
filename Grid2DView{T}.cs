using System.Collections;

namespace SCENeo;

public sealed class Grid2DView<T>(Grid2D<T> grid) : IView<T>, ICloneable
{
    private readonly Grid2D<T> _grid = grid;

    public int Width { get { return _grid.Width; } }

    public int Height { get { return _grid.Height; } }

    public T this[int x, int y] { get { return _grid[x, y]; } }

    public T this[Vec2I pos] { get { return _grid[pos]; } }

    public override string ToString()
    {
        return _grid.ToString();
    }

    public Rect2DI Area() => _grid.Area();

    public IEnumerator<T> GetEnumerator()
    {
        return SCEUtils.GetEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return SCEUtils.GetEnumerator(this);
    }

    public object Clone()
    {
        return _grid.Clone();
    }
}