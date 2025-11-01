using SCENeo.Utils;

namespace SCENeo;

internal sealed class Display<T>(T source)
    where T : IRenderable, IResizeable
{
    public readonly T Source = source;

    public Vec2I Position = Vec2I.Zero;

    public bool AutoResizing = false;

    public Action<Vec2I>? OnResize = null;

    public void Update()
    {
        AutoResize();

        BufferUtils.WriteGrid(Source.Render(), Position);
    }

    private void AutoResize()
    {
        if (!AutoResizing) return;

        Vec2I winDimensions = SCEUtils.WindowDimensions();

        if (winDimensions == Source.Dimensions()) return;

        Source.Resize(winDimensions);

        OnResize?.Invoke(winDimensions);
    }
}
