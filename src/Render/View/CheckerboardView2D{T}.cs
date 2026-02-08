namespace SCENeo;

public sealed class CheckerboardView2D<T> : IView<T>
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Vec2I CheckSize { get; set; } = Vec2I.One;

    public T Value { get; set; }
    public T AltValue { get; set; }

    public T this[int x, int y]
    {
        get
        {
            int checkX = Math.Abs(x / CheckSize.X % 2);
            int checkY = Math.Abs(y / CheckSize.Y % 2);

            return checkX == checkY ? Value : AltValue;
        }
    }

    public T this[Vec2I coord] => this[coord.X, coord.Y];
}
