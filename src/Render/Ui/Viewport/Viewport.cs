namespace SCENeo.Ui;

public sealed partial class Viewport : ViewportBase
{
    private readonly Image _buffer = new Image();

    public override IView<Pixel> Render()
    {
        Update();

        return _buffer.AsReadonly();
    }

    private void Update()
    {
        if (Width != _buffer.Width || Height != _buffer.Height)
        {
            _buffer.CleanResize(Width, Height);
        }

        _buffer.Fill(BasePixel);

        Rect2DI renderArea = new Rect2DI(Width, Height);

        foreach (IRenderable renderable in GetSorted())
        {
            MappedArea(renderable, out Vec2I position, out Rect2DI area);

            if (renderArea.Overlaps(area))
            {
                _buffer.MergeMap(renderable.Render(), position);
            }
        }
    }
}