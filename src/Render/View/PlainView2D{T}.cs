using System.Collections;

namespace SCENeo;

/// <summary>
/// A class representing a resizeable view of a single value.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PlainView2D<T> : IView<T>
{
    public T Value { get; set; } = default!;

    public int Width { get; set; }
    public int Height { get; set; }

    public T this[int x, int y] => Value;
    public T this[Vec2I pos] => Value;
}
