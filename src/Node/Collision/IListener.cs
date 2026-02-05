namespace SCENeo.Node.Collision;

public interface IListener
{
    ushort Masks { get; }
    Action<IReceiver> Listen { get; }
    bool CollidesWith(IReceiver other);
}
