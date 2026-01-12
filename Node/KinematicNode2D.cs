namespace SCENeo.Node;

public class KinematicNode2D : Node2D
{
    public Vec2 Velocity;

    protected virtual void Move(double delta)
    {
        Position += Velocity * (float)delta;
    }
}
