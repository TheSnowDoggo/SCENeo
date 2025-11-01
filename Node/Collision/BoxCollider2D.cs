namespace SCENeo.Node.Collision;

public sealed class BoxCollider2D : Collider2D
{
    public Rect2D Area;

    public Rect2D GlobalArea()
    {
        return Area + GlobalPosition;
    }

    public override bool CollidesWith(IReceive receiver)
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