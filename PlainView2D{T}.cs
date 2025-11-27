using System.Collections;

namespace SCENeo;

internal sealed class PlainView2D<T> : IView<T>
{
    public T Value { get; set; } = default!;

    public int Width { get; set; }

    public int Height { get; set; }

    public T this[int x, int y] { get { return Value; } }

    public T this[Vec2I pos] { get { return Value; } }

    #region IEnumerable

    public IEnumerator<T> GetEnumerator()
    {
        int size = Width * Height;

        for (int i = 0; i < size; i++)
        {
            yield return Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
