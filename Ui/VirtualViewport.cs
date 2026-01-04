using System.Collections;

namespace SCENeo.Ui;

public sealed class VirtualViewport : ViewportBase
{
    private sealed class RenderItem
    {
        public IView<Pixel> View { get; init; } = null!;
        public Rect2DI Area { get; init; }
        public Vec2I Start { get; init; }

        public Pixel Get(Vec2I position)
        {
            return View[position - Start];
        }
    }

    private sealed class ViewportView : IView<Pixel>
    {
        private VirtualViewport _source;
        private IReadOnlyList<RenderItem> _items;

        public ViewportView(VirtualViewport source, IReadOnlyList<RenderItem> renderItems)
        {
            _source = source;
            _items = renderItems;
        }

        public int Width => _source.Width;
        public int Height => _source.Height;

        public Pixel this[int x, int y]
        {
            get => this[new Vec2I(x, y)];
        }

        public Pixel this[Vec2I position]
        {
            get => _source.Merging ? GetMerged(position) : GetFirst(position);
        }

        public IEnumerator<Pixel> GetEnumerator()
        {
            return SCEUtils.GetEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SCEUtils.GetEnumerator(this);
        }

        private Pixel GetFirst(Vec2I position)
        {
            foreach (RenderItem item in _items)
            {
                if (item.Area.HasPoint(position))
                {
                    return item.Get(position);
                }
            }

            return _source.BasePixel;
        }

        private Pixel GetMerged(Vec2I position)
        {
            Pixel pixel = _source.BasePixel;

            foreach (RenderItem item in _items)
            {
                if (item.Area.HasPoint(position))
                {
                    pixel = pixel.Merge(item.Get(position));
                }
            }

            return pixel;
        }
    }

    /// <summary>
    /// Gets or sets a value indicated whether merging should be enabled.
    /// </summary>
    public bool Merging { get; set; } = true;

    public override IView<Pixel> Render()
    {
        var renderItems = new List<RenderItem>();

        Rect2DI renderArea = new Rect2DI(Width, Height);

        _comparer.Reverse = !Merging;

        foreach (IRenderable renderable in GetSorted())
        {
            MappedArea(renderable, out Vec2I position, out Rect2DI area);

            if (!renderArea.Overlaps(area))
            {
                continue;
            }

            IView<Pixel> view = renderable.Render();

            area = Rect2DI.Area(position, view.Size());

            if (!renderArea.Overlaps(area))
            {
                continue;
            }

            area = area.Trim(renderArea);

            var renderItem = new RenderItem()
            {
                View = view,
                Area = area,
                Start = position,
            };

            renderItems.Add(renderItem);
        }

        return new ViewportView(this, renderItems);
    }
}