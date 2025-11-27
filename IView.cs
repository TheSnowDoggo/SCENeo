namespace SCENeo;

public interface IView<T> : IEnumerable<T>, IDimensioned
{
    T this[int x, int y] { get; }

    T this[Vec2I pos] { get; }
}
