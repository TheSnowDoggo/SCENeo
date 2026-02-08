using SCENeo.Ui;

namespace SCENeo.Render;

public sealed class VirtualFilter : UiModifier<IRenderable>
{
    private sealed class View : IView<Pixel>
    {
        private readonly Func<Pixel, Pixel> _filterMode;
        private readonly IView<Pixel> _source;

        public View(Func<Pixel, Pixel> filterMode, IView<Pixel> source)
        {
            _filterMode = filterMode;
            _source = source;
        }

        public int Width => _source.Width;
        public int Height => _source.Height;

        public Pixel this[int x, int y] => _filterMode(_source[x, y]);
        public Pixel this[Vec2I coord] => _filterMode(_source[coord]);
    }

    public Func<Pixel, Pixel> FilterMode;

    public override IView<Pixel> Render()
    {
        var view = Source.Render();

        if (FilterMode == null)
        {
            return view;
        }

        return new View(FilterMode, view);
    }
}