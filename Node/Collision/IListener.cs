namespace SCENeo.Node.Collision;

public interface IListener
{
    ushort Masks { get; }
    Action<IReceiver>? OnCollisionListen { get; }
    bool CollidesWith(IReceiver other);
}
