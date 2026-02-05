namespace SCENeo.Ui;

public sealed class VirtualScaler : UiModifier<IRenderable>
{
    private sealed class View : ViewBase
    {
        private readonly IView<Pixel> _view;
        private readonly Vec2I _scale;

        public View(IView<Pixel> view, Vec2I scale)
            : base(view.Width * scale.X, view.Height * scale.Y)
        {
            _view = view;
            _scale = scale;
        }

        public override Pixel this[int x, int y] => _view[x / _scale.X, y / _scale.Y];
        public override Pixel this[Vec2I position] => _view[position / _scale];
    }

    public override int Width => base.Width * Scale.X;
    public override int Height => base.Height * Scale.Y;

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    public Vec2I Scale { get; set; } = Vec2I.One;

    public override IView<Pixel> Render()
    {
        return new View(Source.Render(), Scale);
    }
}