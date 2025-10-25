using System.Collections;

namespace SCENeo;

public sealed class Grid2DView<T>(Grid2D<T> grid) : IEnumerable<T>,
    ICloneable, IDimensioned
{
    private readonly Grid2D<T> _grid = grid;

    public int Width { get { return _grid.Width; } }

    public int Height { get { return _grid.Height; } }

    public int Size { get { return _grid.Size; } }

    public Vec2I Dimensions {  get { return _grid.Dimensions; } }

    public T this[int x, int y] { get { return _grid[x, y]; } }

    public T this[Vec2I pos] { get { return _grid[pos]; } }

    public override string ToString()
    {
        return _grid.ToString();
    }

    public Rect2DI Area() => _grid.Area();

    #region IEnumerable

    public IEnumerator<T> GetEnumerator()
    {
        return _grid.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _grid.GetEnumerator();
    }

    #endregion

    #region ICloneable

    public object Clone()
    {
        return _grid.Clone();
    }

    #endregion
}