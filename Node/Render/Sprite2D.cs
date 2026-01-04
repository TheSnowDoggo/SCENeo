namespace SCENeo.Node.Render;

public sealed class Sprite2D : Node2D, IRenderable
{
    public IRenderable Source { get; set; } = null!;

    public bool Visible { get { return VisibleOverride ?? Source?.Visible ?? false; } }
    public Vec2I Offset { get { return (Vec2I)GlobalPosition.Round() * new Vec2I(2, 1) + (OffsetOverride ?? Source.Offset); } }
    public int Layer { get { return LayerOverride ?? Source.Layer; } }
    public Anchor Anchor { get { return AnchorOverride ?? Source.Anchor; } }

    public bool? VisibleOverride { get; set; }
    public Vec2I? OffsetOverride { get; set; }
    public int? LayerOverride { get; set; }
    public Anchor? AnchorOverride { get; set; }

    public int Width { get { return Source.Width; } }
    public int Height { get { return Source.Height; } }

    public IView<Pixel> Render()
    {
        return Source.Render();
    }
}