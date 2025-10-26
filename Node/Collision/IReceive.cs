namespace SCENeo.Node.Collision;

public interface IReceive
{
    ushort Layers { get; }
    Action<IListen>? OnCollisionReceive { get; }
}
