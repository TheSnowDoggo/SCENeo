namespace SCENeo.Node.Collision;

public sealed class CircleCollider2D : Collider2D
{
    public float Radius { get; set; }
    public Anchor Anchor { get; set; }

    public Vec2 Size => Vec2.One * Radius;

    public Circle2D GlobalCircle()
    {
        Vec2 position = GetPosition() + Anchor.AnchorDimension(Size) - Size;

        return new Circle2D(Radius, position);
    }

    public override bool CollidesWith(IReceiver receiver)
    {
        if (receiver is CircleCollider2D sphere)
        {
            return GlobalCollision.Collides(this, sphere);
        }

        if (receiver is BoxCollider2D box)
        {
            return GlobalCollision.Collides(this, box);
        }

        throw new IncompatibleReceiverException(receiver);
    }
}