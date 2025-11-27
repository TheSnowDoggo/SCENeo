using SCENeo.Utils;

namespace SCENeo;

public sealed class Display
{
    public IRenderable Source { get; set; } = default!;

    public Vec2I Position = Vec2I.Zero;

    public Action<Vec2I>? OnResize = null;

    public void Update()
    {
        AutoResize();

        BufferUtils.WriteGrid(Source.Render(), Position);
    }

    private void AutoResize()
    {
        Vec2I winDimensions = SCEUtils.WindowDimensions();

        if (winDimensions == Source.Dimensions())
        {
            return;
        }

        OnResize?.Invoke(winDimensions);
    }
}
