namespace SCENeo.Node.Collision;

public sealed class BoxCollider2D : Collider2D
{
    public Vec2 Size { get; set; }
    public Anchor Anchor { get; set; }

    public Rect2D GlobalArea()
    {
        Vec2 position = GetPosition() + Anchor.AnchorDimension(Size) - Size;

        return new Rect2D(position, position + Size);
    }

    public override bool CollidesWith(IReceiver receiver)
    {
        if (receiver is BoxCollider2D box)
        {
            return GlobalCollision.Collides(this, box);
        }

        if (receiver is CircleCollider2D sphere)
        {
            return GlobalCollision.Collides(sphere, this);
        }

        throw new IncompatibleReceiverException(receiver);
    }
}