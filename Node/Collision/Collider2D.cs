namespace SCENeo.Node.Collision;

public abstract class Collider2D : Node2D, IReceiver, IListener
{
    public ushort Layers { get; set; }
    public ushort Masks { get; set; }

    public Action<IReceiver>? OnCollisionListen { get; set; }
    public Action<IListener>? OnCollisionReceive { get; set; }

    public abstract bool CollidesWith(IReceiver other);
}