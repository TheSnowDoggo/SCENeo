namespace SCENeo.Ui;

/// <summary>
/// An interface representing an class which sends updates.
/// </summary>
public interface IUpdate
{
    /// <summary>
    /// Action invoked on update.
    /// </summary>
    event Action Updated;
}