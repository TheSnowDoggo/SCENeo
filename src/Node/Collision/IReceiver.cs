namespace SCENeo.Node.Collision;

public interface IReceiver
{
    ushort Layers { get; }
    Action<IListener> Receive { get; }
}
