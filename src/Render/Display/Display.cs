namespace SCENeo;

/// <summary>
/// A class that renders a <see cref="IRenderable"/> to a given <see cref="IOutputSource"/>.
/// </summary>
public sealed class Display
{
    private Vec2I _size;

    public Display()
    {
    }

    public Display(Action<Vec2I> resized)
    {
        Resized = resized;
    }

    /// <summary>
    /// Gets or sets the item to render on update.
    /// </summary>
    public IRenderable Renderable { get; set; }

    /// <summary>
    /// Gets or sets the output source to render to on update.
    /// </summary>
    public IOutputSource Output { get; set; }

    /// <summary>
    /// Called when the output size channges.
    /// </summary>
    public event Action<Vec2I> Resized;

    /// <summary>
    /// Renders the <see cref="Renderable"/> to the <see cref="Output"/>.
    /// </summary>
    public void Update()
    {
        Vec2I size = new Vec2I(Output.Width, Output.Height);

        if (size != _size)
        {
            _size = size;
            Resized?.Invoke(size);
        }

        IView<Pixel> view = Renderable.Render();

        Output.Update(view);
    }
}
