namespace SCENeo;

public sealed class Display
{
    public IRenderable Source { get; set; } = default!;

    public IOutputSource Output { get; set; } = null!;

    public Vec2I Position = Vec2I.Zero;

    public Action<int, int>? OnResize = null;

    public void Update()
    {
        int width  = Output.Width;
        int height = Output.Height;

        if (width != Source.Width || height != Source.Height)
        {
            OnResize?.Invoke(width, height);
        }

        IView<Pixel> view = Source.Render();

        Output.Update(view);
    }
}
