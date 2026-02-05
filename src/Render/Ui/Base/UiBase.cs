namespace SCENeo.Ui;

public abstract class UiBase
{
    /// <summary>
    /// Gets or sets a value representing whether this should be visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the coordinate offset.
    /// </summary>
    public Vec2I Offset { get; set; }

    /// <summary>
    /// Gets or sets the layer.
    /// </summary>
    public int Layer { get; set; }

    /// <summary>
    /// Gets or sets the anchor alignment.
    /// </summary>
    public Anchor Anchor { get; set; }
}