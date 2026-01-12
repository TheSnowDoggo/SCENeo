namespace SCENeo.Node.Collision;

public interface IListener
{
    ushort Masks { get; }
    Action<IReceiver>? CollisionListen { get; }
    bool CollidesWith(IReceiver other);
}
