namespace SCENeo.Ui;

/// <summary>
/// DO NOT USE SLOW
/// </summary>
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

    private sealed class View : ViewBase
    {
        private IReadOnlyList<RenderItem> _items;
        private Pixel _basePixel;
        private bool _merging;

        public View(VirtualViewport source, IReadOnlyList<RenderItem> renderItems)
            : base(source.Width, source.Height)
        {
            _basePixel = source.BasePixel;
            _merging = source.Merging;

            _items = renderItems;
        }

        public override Pixel this[int x, int y]
        {
            get => this[new Vec2I(x, y)];
        }

        public override Pixel this[Vec2I position]
        {
            get => _merging ? GetMerged(position) : GetFirst(position);
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

            return _basePixel;
        }

        private Pixel GetMerged(Vec2I position)
        {
            Pixel pixel = _basePixel;

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

        return new View(this, renderItems);
    }
}