namespace SCENeo;

/// <summary>
/// A class that renders a <see cref="IRenderable"/> to a given <see cref="IOutputSource"/>.
/// </summary>
public sealed class Display
{
    private int _width;
    private int _height;

    /// <summary>
    /// Gets or sets the item to render on update.
    /// </summary>
    public IRenderable Renderable { get; set; } = default!;

    /// <summary>
    /// Gets or sets the output source to render to on update.
    /// </summary>
    public IOutputSource Output { get; set; } = null!;

    /// <summary>
    /// Called when the output size channges.
    /// </summary>
    public event Action<int, int>? OnResize = null;

    /// <summary>
    /// Renders the <see cref="Renderable"/> to the <see cref="Output"/>.
    /// </summary>
    public void Update()
    {
        int width  = Output.Width;
        int height = Output.Height;

        if (width != _width || height != _height)
        {
            _width = width;
            _height = height;

            OnResize?.Invoke(width, height);
        }

        IView<Pixel> view = Renderable.Render();

        Output.Update(view);
    }
}
