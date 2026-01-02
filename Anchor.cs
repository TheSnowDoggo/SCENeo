namespace SCENeo;

/// <summary>
/// Represents an anchor.
/// </summary>
[Flags]
public enum Anchor
{
    /// <summary>
    /// No alignment (defaults to left, top alignment)
    /// </summary>
    None   = 0,

    /// <summary>
    /// Right alignment.
    /// </summary>
    Right  = 1,

    /// <summary>
    /// Horizontal center alignment.
    /// </summary>
    Center = 2,

    /// <summary>
    /// Bottom alignment.
    /// </summary>
    Bottom = 4,

    /// <summary>
    /// Vertical middle alignment.
    /// </summary>
    Middle = 8,
}