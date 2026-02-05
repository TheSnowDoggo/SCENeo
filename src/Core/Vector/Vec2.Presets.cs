namespace SCENeo;

public partial struct Vec2
{
    /// <summary>
    /// Gets the vector equivalent to Vec2(1, 1).
    /// </summary>
    public static Vec2 One => new Vec2(1f, 1f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(0, 0).
    /// </summary>
    public static Vec2 Zero => new Vec2(0f, 0f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(0, -1).
    /// </summary>
    public static Vec2 Up => new Vec2(0f, -1f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(0, 1).
    /// </summary>
    public static Vec2 Down => new Vec2(0f, +1f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(-1, 0).
    /// </summary>
    public static Vec2 Left => new Vec2(-1f, 0f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(1, 0).
    /// </summary>
    public static Vec2 Right => new Vec2(+1f, 0f);
}