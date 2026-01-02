namespace SCENeo;

/// <summary>
/// An interface representing a readonly 2D grid.
/// </summary>
/// <typeparam name="T">The stored type.</typeparam>
public interface IView<T> : IEnumerable<T>, IDimensioned
{
    /// <summary>
    /// Gets the value at the given position.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The value at the given position.</returns>
    T this[int x, int y] { get; }

    /// <summary>
    /// Gets the value at the given position.
    /// </summary>
    /// <param name="position">The 2D coordinate position.</param>
    /// <returns>The value at the given position.</returns>
    T this[Vec2I position] { get; }
}
