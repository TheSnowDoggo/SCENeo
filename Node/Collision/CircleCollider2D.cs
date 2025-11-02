using SCENeo.Utils;

namespace SCENeo.Node.Collision;

public sealed class CircleCollider2D : Collider2D
{
    public float Radius;

    public Rect2D GlobalArea()
    {
        return new Rect2D(GlobalPosition - Radius, GlobalPosition + Radius);
    }

    public bool OverlapsVertical(float x, float top, float bottom)
    {
        float d = Descriminant(x - GlobalPosition.X);

        if (d < 0) return false;

        float y = GlobalPosition.Y + MathF.Sqrt(d);

        return y >= bottom && y <= top;
    }

    public bool OverlapsHorizontal(float y, float left, float right)
    {
        float d = Descriminant(y - GlobalPosition.Y);

        if (d < 0) return false;

        float x = GlobalPosition.X + MathF.Sqrt(d);

        return x >= left && x <= right;
    }

    private float Descriminant(float m)
    {
        return Radius.Squared() - m.Squared();
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