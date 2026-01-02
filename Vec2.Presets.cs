namespace SCENeo;

public partial struct Vec2
{
    /// <summary>
    /// Gets the vector equivalent to Vec2(0, 0).
    /// </summary>
    public static Vec2 Zero => new(0f, 0f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(0, -1).
    /// </summary>
    public static Vec2 Up => new(0f, -1f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(0, 1).
    /// </summary>
    public static Vec2 Down => new(0f, +1f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(-1, 0).
    /// </summary>
    public static Vec2 Left => new(-1f, 0f);

    /// <summary>
    /// Gets the vector equivalent to Vec2(1, 0).
    /// </summary>
    public static Vec2 Right => new(+1f, 0f);
}