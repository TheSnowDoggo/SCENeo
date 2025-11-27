using SCENeo.Utils;

namespace SCENeo;

public sealed class Display
{
    public IRenderable Source { get; set; } = default!;

    public IOutputSource? Output { get; set; } = null;

    public Vec2I Position = Vec2I.Zero;

    public Action<Vec2I>? OnResize = null;

    public void Update()
    {
        if (Output == null)
        {
            throw new NullReferenceException("No output source set.");
        }

        Vec2I winDimensions = SCEUtils.ConsoleWindowSize();

        if (winDimensions != Source.Dimensions())
        {
            OnResize?.Invoke(winDimensions);
        }

        IView<Pixel> view = Source.Render();

        Output.Update(view);
    }
}
