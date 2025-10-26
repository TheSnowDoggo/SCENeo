namespace SCENeo.Node;

public interface IEngine
{
    bool Enabled { get; }
    void Update(double delta, IReadOnlyList<Node> active);
}
