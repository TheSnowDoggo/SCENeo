namespace SCENeo.Node.Boid;

internal class MoveWrapper(KinematicNode2D node) : IMove
{
    private readonly KinematicNode2D _node = node;

    public Vec2 Position { get { return _node.GlobalPosition; } }

    public Vec2 Velocity { get { return _node.Velocity; } }
}