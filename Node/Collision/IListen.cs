namespace SCENeo.Node.Collision;

public interface IListen
{
    ushort Masks { get; }
    Action<IReceive>? OnCollisionListen { get; }
    bool CollidesWith(IReceive other);
}
