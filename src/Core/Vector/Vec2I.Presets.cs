namespace SCENeo;

public partial struct Vec2I
{
    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, 1).
    /// </summary>
    public static Vec2I One => new Vec2I(1, 1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 0).
    /// </summary>
    public static Vec2I Zero => new Vec2I(0, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, -1).
    /// </summary>
    public static Vec2I Up => new Vec2I(0, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(0, 1).
    /// </summary>
    public static Vec2I Down => new Vec2I(0, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 0).
    /// </summary>
    public static Vec2I Left => new Vec2I(-1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(+1, 0).
    /// </summary>
    public static Vec2I Right => new Vec2I(+1, 0);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, -1).
    /// </summary>
    public static Vec2I UpLeft => new Vec2I(-1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, -1).
    /// </summary>
    public static Vec2I UpRight => new Vec2I(+1, -1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(-1, 1).
    /// </summary>
    public static Vec2I DownLeft => new Vec2I(-1, +1);

    /// <summary>
    /// Gets the vector equivalent to Vec2I(1, 1).
    /// </summary>
    public static Vec2I DownRight => new Vec2I(+1, +1);
}