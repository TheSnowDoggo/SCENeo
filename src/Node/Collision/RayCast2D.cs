namespace SCENeo.Node.Collision;

public class RayCast2D : Node2D, IListener
{
    public ushort Masks { get; set; }
    public Vec2 EndPosition { get; set; }

    public Action<IReceiver> Listen { get; set; }

    public float Left => Math.Min(GlobalPosition.X, EndPosition.X);
    public float Right => Math.Max(GlobalPosition.X, EndPosition.X);

    public Line2D GlobalLine()
    {
        return Line2D.FromPoints(GlobalPosition, GlobalPosition + EndPosition);
    }

    public bool CollidesWith(IReceiver other)
    {
        if (other is BoxCollider2D box)
        {
            return GlobalCollision.Collides(this, box);
        }

        if (other is CircleCollider2D circle)
        {
            return GlobalCollision.Collides(this, circle);
        } 

        throw new IncompatibleReceiverException(other);
    }
}