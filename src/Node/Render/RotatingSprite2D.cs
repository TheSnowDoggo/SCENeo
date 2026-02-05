using SCENeo.Ui;

namespace SCENeo.Node.Render;

public class RotatingSprite2D : Sprite2D
{
    private readonly WideRotater _rotater = new();

    public override IRenderable Source
    {
        get => _rotater.Source;
        set => _rotater.Source = value;
    }

    public override int Width => _rotater.Width;
    public override int Height => _rotater.Height;

    public Rotation Rotation
    {
        get => _rotater.Rotation;
        set => _rotater.Rotation = value;
    }

    public override IView<Pixel> Render()
    {
        return _rotater.Render();
    }
}