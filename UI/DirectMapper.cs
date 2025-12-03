using SCENeo.Utils;

namespace SCENeo.UI;

public sealed class DirectMapper<T> : UIBaseImage
{
    public DirectMapper() : base() { }
    
    public DirectMapper(IView<T> source)
        : base(source.Width, source.Height)
    {
        Source = source;
    }
    
    public IView<T>? Source { get; set; }
    
    public Func<int, int, Pixel>? Translation { get; set; }

    public bool Bake { get; set; } = false;

    public bool IsBaked { get; set; } = false;

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
        }
        
        return base.Render();
    }

    private void Update()
    {
        if (Translation == null)
        {
            throw new NullReferenceException("Translation unit is null.");
        }
        
        _source.Fill(Translation);
        
        IsBaked = true;
    }
    
    private IView<T> GetSource()
    {
        return Source ?? throw new NullReferenceException("Source is null.");
    }
}