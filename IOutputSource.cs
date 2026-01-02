namespace SCENeo;

/// <summary>
/// An interface representing a source for outputing to the console. 
/// </summary>
public interface IOutputSource : IDimensioned
{
    /// <summary>
    /// Outputs the given view to the console.
    /// </summary>
    /// <param name="view">The view to output.</param>
    void Update(IView<Pixel> view);
}