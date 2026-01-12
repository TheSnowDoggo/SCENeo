namespace SCENeo.Node.Render;

public class Sprite2D : Node2D, IRenderable
{
    public virtual IRenderable Source { get; set; } = null!;

    public bool Visible => VisibleOverride ?? Source?.Visible ?? false;
    public Vec2I Offset => (GlobalPosition * new Vec2(2, 1)).Round().ToVec2I() + (OffsetOverride ?? Source.Offset);
    public int Layer => LayerOverride ?? Source.Layer;
    public Anchor Anchor => AnchorOverride ?? Source.Anchor;

    public bool? VisibleOverride { get; set; }
    public Vec2I? OffsetOverride { get; set; }
    public int? LayerOverride { get; set; }
    public Anchor? AnchorOverride { get; set; }

    public virtual int Width => Source.Width;
    public virtual int Height => Source.Height;

    public virtual IView<Pixel> Render()
    {
        return Source.Render();
    }
}