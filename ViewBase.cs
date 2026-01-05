using System.Collections;

namespace SCENeo;

public abstract class ViewBase : IView<Pixel>
{
    public ViewBase()
    {
    }

    public ViewBase(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public virtual int Width { get; }
    public virtual int Height { get; }

    public abstract Pixel this[int x, int y] { get; }
    public abstract Pixel this[Vec2I position] { get; }

    public IEnumerator<Pixel> GetEnumerator()
    {
        return SCEUtils.EnumerateData(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return SCEUtils.EnumerateData(this);
    }
}
