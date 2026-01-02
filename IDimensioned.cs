namespace SCENeo;

/// <summary>
/// An interface representing a width and height.
/// </summary>
public interface IDimensioned
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    int Height { get; }
}