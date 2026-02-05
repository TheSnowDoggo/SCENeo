namespace SCENeo.Ui;

public sealed class ViewForwarder : UiBase, IRenderable
{
    public ViewForwarder()
    {
    }

    public IView<Pixel> View { get; set; } = null!;

    public int Width => View.Width;
    public int Height => View.Height;

    public IView<Pixel> Render()
    {
        return View;
    }
}
