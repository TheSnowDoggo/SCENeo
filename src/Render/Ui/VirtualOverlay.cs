namespace SCENeo.Ui;

public sealed class VirtualOverlay : UiModifier<IRenderable>, IRenderable
{
    private sealed class View : ViewBase
    {
        private readonly IView<Pixel> _source;
        private readonly IView<Pixel> _top;
        private readonly Vec2I _topOffset;
        private readonly Rect2DI _overlap;

        public View(IView<Pixel> source, IView<Pixel> top, Vec2I topOffset)
            : base(source.Width, source.Height)
        {
            _source = source;
            _top = top;
            _topOffset = topOffset;

            _overlap = Rect2DI.Area(topOffset, top.Width, top.Height)
                .Trim(0, 0, source.Width, source.Height);
        }

        public override Pixel this[Vec2I pos]
            => _overlap.HasPoint(pos) ? _source[pos].Merge(_top[pos - _topOffset]) : _source[pos];

        public override Pixel this[int x, int y]
            => this[new Vec2I(x, y)];
    }

    public IRenderable Overlay { get; set; }

    public override IView<Pixel> Render()
    {
        IView<Pixel> view = Source.Render();

        if (!Overlay.Visible)
        {
            return view;
        }

        Vec2I position = Overlay.Offset + Overlay.Anchor.AnchorDimension(this.Size() - Overlay.Size());

        if (!Rect2DI.Area(position, Overlay.Width, Overlay.Height).Overlaps(0, 0, Width, Height))
        {
            return view;
        }

        return new View(view, Overlay.Render(), position);
    }
}