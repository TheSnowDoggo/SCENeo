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
    
    protected static void ObserveSet<T>(ref T property, T value, ref bool updateFlag)
    {
        if (EqualityComparer<T>.Default.Equals(value, property))
        {
            return;
        }
        
        property = value;
        updateFlag = true;
    }
    
    protected static void ObserveSet<T>(ref T property, T value, ref bool updateFlag, Action updateCallback)
        where T : IUpdate
    {
        if (EqualityComparer<T>.Default.Equals(value, property))
        {
            return;
        }

        if (property != null)
        {
            property.Updated -= updateCallback;
        }

        if (value != null)
        {
            value.Updated += updateCallback;
        }
        
        property = value;
        updateFlag = true;
        updateCallback?.Invoke();
    }
}