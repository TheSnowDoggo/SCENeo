namespace SCENeo.Node.Render;

public class Camera2D : Node2D
{
    public int Channel { get; set; }

    public Anchor Anchor { get; set; }

    public Vec2I RenderPosition()
    {
        return (GlobalPosition * new Vec2(2, 1)).Round().ToVec2I();
    }
}
