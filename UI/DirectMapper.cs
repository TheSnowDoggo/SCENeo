using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class DirectMapper<T> : UIBaseImage
{
    private bool _update = false;

    private Func<T, Pixel>? _translation;

    public DirectMapper() : base() { }
    
    public DirectMapper(IView<T> source)
        : base(source.Width, source.Height)
    {
        Source = source;
    }

    #region Properties

    public IView<T>? Source { get; set; }
    
    public Func<T, Pixel>? Translation
    {
        get { return _translation; }
        set { SCEUtils.ObserveSet(value, ref _translation, ref _update); }
    }

    public bool Bake { get; set; } = false;

    public bool IsBaked { get; set; } = false;

    public override int Width { get { return Source?.Width ?? 0; } }

    public override int Height { get { return Source?.Height ?? 0; } }

    #endregion

    public override Grid2DView<Pixel> Render()
    {
        Vec2I expected = GetSource().Dimensions();
        
        if (_source.Dimensions() != expected)
        {
            _source.CleanResize(expected.X, expected.Y);
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
        
        return _source;
    }

    private void Update()
    {
        if (Translation == null)
        {
            throw new NullReferenceException("Translation unit is null.");
        }

        if (Source == null)
        {
            throw new NullReferenceException("Source is null.");
        }
        
        _source.Fill((x, y) => Translation.Invoke(Source[x, y]));
    }
    
    private IView<T> GetSource()
    {
        return Source ?? throw new NullReferenceException("Source is null.");
    }
}