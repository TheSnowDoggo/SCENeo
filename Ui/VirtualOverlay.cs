using System.Collections;

namespace SCENeo.Ui;

public sealed class VirtualOverlay : UiModifier<IRenderable>, IRenderable
{
    private sealed class OverlayView : IView<Pixel>
    {
        private readonly IView<Pixel> _source;
        private readonly IView<Pixel> _top;
        private readonly Vec2I _topOffset;
        private readonly Rect2DI _overlap;

        public OverlayView(IView<Pixel> source, IView<Pixel> top, Vec2I topOffset)
        {
            _source = source;
            _top = top;
            _topOffset = topOffset;

            _overlap = Rect2DI.Area(topOffset, top.Width, top.Height)
                .Trim(0, 0, source.Width, source.Height);
        }

        public int Width { get { return _source.Width; } }

        public int Height { get { return _source.Height; } }

        public Pixel this[Vec2I pos]
        {
            get { return _overlap.HasPoint(pos) ? _source[pos].Merge(_top[pos - _topOffset]) : _source[pos]; }
        }

        public Pixel this[int x, int y]
        {
            get { return this[new Vec2I(x, y)]; }
        }

        public IEnumerator<Pixel> GetEnumerator()
        {
            return SCEUtils.GetEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SCEUtils.GetEnumerator(this);
        }
    }

    public IRenderable Overlay { get; set; } = default!;

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

        return new OverlayView(view, Overlay.Render(), position);
    }
}