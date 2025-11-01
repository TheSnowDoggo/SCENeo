namespace SCENeo.Node.Collision;

internal class RayCast2D : Node2D, IListen
{
    public ushort Masks { get; set; }

    public Action<IReceive>? OnCollisionListen { get; set; }

    public Vec2 EndPosition { get; set; }

    public Vec2 GlobalEnd()
    {
        return GlobalPosition + EndPosition;
    }

    public bool CollidesWith(IReceive other)
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