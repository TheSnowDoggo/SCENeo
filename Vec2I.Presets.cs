namespace SCENeo;

public partial struct Vec2I
{
    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 0).
    /// </summary>
    public static Vec2I Zero => new(0, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, -1).
    /// </summary>
    public static Vec2I Up => new(0, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 1).
    /// </summary>
    public static Vec2I Down => new(0, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 0).
    /// </summary>
    public static Vec2I Left => new(-1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(+1, 0).
    /// </summary>
    public static Vec2I Right => new(+1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, -1).
    /// </summary>
    public static Vec2I UpLeft => new(-1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, -1).
    /// </summary>
    public static Vec2I UpRight => new(+1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 1).
    /// </summary>
    public static Vec2I DownLeft => new(-1, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, 1).
    /// </summary>
    public static Vec2I DownRight => new(+1, +1);
}