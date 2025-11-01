using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class Stretcher : UIModifier<IRenderable>
{
    public enum Scaling
    {
        None,
        Stretch,
        Slide,
        Hide,
    }

    private readonly Image _buffer = new Image();

    private int _scaleWidth;

    public Stretcher(IRenderable source) : base(source) { }

    public int ScaleWidth
    {
        get {  return _scaleWidth; }
        set
        {
            if (value < 1)
            {
                throw new NotImplementedException("Scale width cannot be negative or zero.");
            }

            _scaleWidth = value;
        }
    }

    public Scaling TextScaling { get; set; } = Scaling.None;

    public bool Bake { get; set; } = false;

    public bool IsBaked { get; set; } = false;

    public override int Width { get { return _source.Width * ScaleWidth; } }

    private void Update()
    {
        Grid2DView<Pixel> view = _source.Render();

        if (_buffer.Dimensions != this.Dimensions())
        {
            _buffer.CleanResize(Width, view.Height);
        }

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                switch (TextScaling)
                {
                    case Scaling.None:
                        _buffer[x * ScaleWidth, y] = view[x, y];
                        for (int i = 1; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = new Pixel(view[x, y].Colors);
                        break;
                    case Scaling.Stretch:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x * ScaleWidth + i, y] = view[x, y];
                        break;
                    case Scaling.Slide:
                        for (int i = 0; i < ScaleWidth; i++)
                            _buffer[x + view.Width * i, y] = view[x, y];
                        break;
                    case Scaling.Hide:
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
