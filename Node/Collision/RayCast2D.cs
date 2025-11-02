namespace SCENeo.Node.Collision;

internal class RayCast2D : Node2D, IListener
{
    public ushort Masks { get; set; }

    public Action<IReceiver>? OnCollisionListen { get; set; }

    public Vec2 EndPosition { get; set; }

    public Vec2 GlobalEnd()
    {
        return GlobalPosition + EndPosition;
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