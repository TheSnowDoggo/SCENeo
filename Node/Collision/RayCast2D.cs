namespace SCENeo.Node.Collision;

internal class RayCast2D : Node2D, IListen
{
    public ushort Masks { get; set; }

    public Action<IReceive>? OnCollisionListen { get; set; }

    public Vec2 EndPosition;

    public Vec2 GlobalEnd()
    {
        return GlobalPosition + EndPosition;
    }

    public bool CollidesWith(IReceive other)
    {
        throw new IncompatibleReceiverException(other);
    }
}