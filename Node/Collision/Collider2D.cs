namespace SCENeo.Node.Collision;

public abstract class Collider2D : Node2D, IReceive, IListen
{
    public ushort Layers { get; set; }
    public ushort Masks { get; set; }

    public Action<IReceive>? OnCollisionListen { get; set; }
    public Action<IListen>? OnCollisionReceive { get; set; }

    public abstract bool CollidesWith(IReceive other);
}