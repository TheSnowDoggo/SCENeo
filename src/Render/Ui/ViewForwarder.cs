namespace SCENeo.Ui;

public sealed class ViewForwarder : UiBase, IRenderable
{
    public IView<Pixel> View { get; set; }

    public int Width => View.Width;
    public int Height => View.Height;

    public IView<Pixel> Render()
    {
        return View;
    }
}
