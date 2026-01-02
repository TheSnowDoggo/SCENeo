namespace SCENeo.Ui;

public interface IRenderSource
{
    /// <summary>
    /// Gets the render order priority.
    /// </summary>
    /// <remarks>
    /// Lower values will be rendered first.
    /// </remarks>
    int Priority { get { return 0; } }
    
    /// <summary>
    /// Renders the render source.
    /// </summary>
    /// <returns>A collection containing the renderables to be rendered.</returns>
    IEnumerable<IRenderable> Render();
}