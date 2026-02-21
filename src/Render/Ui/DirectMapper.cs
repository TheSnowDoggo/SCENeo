namespace SCENeo.Ui;

public sealed class DirectMapper<T> : UiBase, IRenderable
{
    private readonly Image _buffer = new Image();

    private bool _update;

    public IView<T> Source { get; set; }

    private Func<T, Pixel> _translation;

    public Func<T, Pixel> Translation
    {
        get => _translation;
        set => ObserveSet(ref _translation, value, ref _update);
    }

    public bool Bake { get; set; }
    public bool IsBaked { get; set; }

    public int Width => Source?.Width ?? 0;
    public int Height => Source?.Height ?? 0;

    public IView<Pixel> Render()
    {
        if (_buffer.Width != Width || _buffer.Height != Height)
        {
            _buffer.CleanResize(Width, Height);
            IsBaked = false;
        }
        
        if (!Bake || !IsBaked)
        {
            Update();
            IsBaked = true;
            
        }

        if (_update)
        {
            Update();
            _update = false;
        }
        
        return _buffer.AsReadonly();
    }

    private void Update()
    {
        if (Translation == null)
        {
            throw new NullReferenceException("Translation unit is null.");
        }

        _buffer.Fill((x, y) => Translation.Invoke(Source[x, y]));
    }
}