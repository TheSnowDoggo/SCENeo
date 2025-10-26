namespace SCENeo.UI;

public sealed class HorizontalScaler<T> : UIModifier<T>, IRenderable, IDimensioned
    where T : IRenderable, IDimensioned
{
    private readonly Image _buffer;

    public HorizontalScaler(T source, int scaleWidth) : base(source)
    {
        if (scaleWidth < 1)
        {
            throw new NotImplementedException("Scale width cannot be negative or zero.");
        }

        _buffer = new Image(source.Width * scaleWidth, source.Height);

        ScaleWidth = scaleWidth;
    }

    public readonly int ScaleWidth;

    public TextScaleMode TextScaling = TextScaleMode.None;

    public bool Bake = false;

    public bool IsBaked = false;

    public override int Width { get { return _buffer.Width * ScaleWidth; } }

    private void Update()
    {
        Grid2DView<Pixel> view = _source.Render();

        if (_buffer.Dimensions != view.Dimensions)
        {
            _buffer.CleanResize(view.Width * ScaleWidth, view.Height);
        }

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                switch (TextScaling)
                {
                    case TextScaleMode.None:
                        _buffer[x * ScaleWidth, y] = view[x, y];
                        for (int i = 1; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].Colors);
                        break;
                    case TextScaleMode.Stretch:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = view[x, y];
                        break;
                    case TextScaleMode.Slide:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x + view.Width * i, y] = view[x, y];
                        break;
                    case TextScaleMode.Hide:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].Colors);
                        break;
                }
            }
        }
    }

    public override Grid2DView<Pixel> Render()
    {
        if (!Bake || !IsBaked)
        {
            Update();
            IsBaked = true;
        }

        return _buffer;
    }
}
