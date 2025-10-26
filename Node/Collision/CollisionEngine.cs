using SCENeo.Utils;

namespace SCENeo.Node.Collision;

public sealed class CollisionEngine : IEngine
{
    public bool Enabled { get; set; } = true;

    public void Update(double _, IReadOnlyList<Node> nodes)
    {
        var listeners  = LoadListeners(nodes);
        var layers     = LoadReceivers(nodes);

        while (listeners.TryDequeue(out IListen? listener))
        {
            FindCollisions(listener, layers);
        }
    }

    private static void FindCollisions(IListen listener, Dictionary<int, List<IReceive>> layers)
    {
        foreach (int mask in listener.Masks.EnumerateFlags()) 
        { 
            if (!layers.TryGetValue(mask, out List<IReceive>? receivers))
            {
                continue;
            }

            FindCollisionsInLayer(listener, receivers);
        }
    }

    private static void FindCollisionsInLayer(IListen listener, List<IReceive> receivers)
    {
        foreach (IReceive receiver in receivers)
        {
            if (listener == receiver)
            {
                continue;
            }

            if (listener.CollidesWith(receiver))
            {
                receiver.OnCollisionReceive?.Invoke(listener);
                listener.OnCollisionListen?.Invoke(receiver);
            }
        }
    }

    private static Queue<IListen> LoadListeners(IReadOnlyList<Node> nodes)
    {
        var queue = new Queue<IListen>();

        foreach (Node node in nodes)
        {
            if (node is not IListen listener || listener.Masks == 0)
            {
                continue;
            }

            queue.Enqueue(listener);
        }

        return queue;
    }

    private static Dictionary<int, List<IReceive>> LoadReceivers(IReadOnlyList<Node> nodes)
    {
        var layers = new Dictionary<int, List<IReceive>>();

        foreach (Node node in nodes)
        {
            if (node is not IReceive receiver || receiver.Layers == 0)
            {
                continue;
            }

            foreach (int layer in receiver.Layers.EnumerateFlags())
            {
                if (!layers.TryGetValue(layer, out var list))
                {
                    list          = [];
                    layers[layer] = list;
                }

                list.Add(receiver);
            }
        }

        return layers;
    }
}