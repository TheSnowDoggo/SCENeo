namespace SCENeo.Node.Render;

public class Camera2D : Node2D
{
    public int Channel { get; set; }

    public Vec2I RenderPosition()
    {
        return (Vec2I)GlobalPosition.Round() * new Vec2I(2, 1);
    }
}
