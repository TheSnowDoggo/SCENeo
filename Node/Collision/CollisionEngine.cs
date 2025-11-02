using SCENeo.Utils;

namespace SCENeo.Node.Collision;

public sealed class CollisionEngine : IEngine
{
    private sealed class CollisionState
    {
        public readonly List<IListener>                  Listeners = [];
        public readonly Dictionary<int, List<IReceiver>> Layers    = [];

        public void AddReceiver(int layer, IReceiver receiver)
        {
            if (!Layers.TryGetValue(layer, out List<IReceiver>? receivers))
            {
                receivers = [];
                Layers.Add(layer, receivers);
            }

            receivers.Add(receiver);
        }
    }

    public bool Enabled { get; set; } = true;

    public void Update(double _, IReadOnlyList<Node> nodes)
    {
        FindCollisions(LoadCollisionState(nodes));
    }

    private static void FindCollisions(CollisionState collisionState)
    {
        foreach (IListener listener in collisionState.Listeners)
        {
            foreach (int mask in listener.Masks.EnumerateFlags())
            {
                if (!collisionState.Layers.TryGetValue(mask, out List<IReceiver>? receivers))
                {
                    continue;
                }

                FindCollisionsInLayer(listener, receivers);
            }
        }
    }

    private static void FindCollisionsInLayer(IListener listener, List<IReceiver> receivers)
    {
        foreach (IReceiver receiver in receivers)
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

    private static CollisionState LoadCollisionState(IReadOnlyList<Node> nodes)
    {
        var state = new CollisionState();

        foreach (Node node in nodes)
        {
            if (node is IListener listener && listener.Masks != 0)
            {
                state.Listeners.Add(listener);
            }

            if (node is IReceiver receiver)
            {
                foreach (int layer in receiver.Layers.EnumerateFlags())
                {
                    state.AddReceiver(layer, receiver);
                }
            }
        }

        return state;
    }
}