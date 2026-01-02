namespace SCENeo;

/// <summary>
/// An interface representing a renderable item.
/// </summary>
public interface IRenderable : IDimensioned
{
    /// <summary>
    /// Gets a value representing whether this should be visible.
    /// </summary>
    bool Visible { get; }

    /// <summary>
    /// Gets the coordinate offset.
    /// </summary>
    public Vec2I Offset { get; }

    /// <summary>
    /// Gets the layer representing the order this should be rendered in.
    /// </summary>
    /// <remarks>
    /// Lower values should represent items which will be rendered first.
    /// </remarks>
    public int Layer { get; }

    /// <summary>
    /// Gets the anchor alignment.
    /// </summary>
    public Anchor Anchor { get; }

    /// <summary>
    /// Returns the view to be rendered.
    /// </summary>
    /// <returns>The view to be rendered.</returns>
    IView<Pixel> Render();
}