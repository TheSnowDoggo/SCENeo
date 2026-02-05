namespace SCENeo.Node.Collision;

public sealed class CollisionEngine : IEngine
{
    private sealed class CollisionState
    {
        public readonly List<IListener> Listeners = [];
        public readonly Dictionary<int, List<IReceiver>> Layers = [];

        public void AddReceiver(int layer, IReceiver receiver)
        {
            if (!Layers.TryGetValue(layer, out List<IReceiver> receivers))
            {
                receivers = [];
                Layers.Add(layer, receivers);
            }

            receivers.Add(receiver);
        }
    }

    private CollisionState _state = null!;

    public bool Enabled { get; set; } = true;

    public void Update(double _, IReadOnlyList<Node> nodes)
    {
        LoadCollisionState(nodes);

        ResolveCollisions();
    }

    private void ResolveCollisions()
    {
        foreach (IListener listener in _state.Listeners)
        {
            foreach (int mask in listener.Masks.EnumerateFlags())
            {
                if (!_state.Layers.TryGetValue(mask, out List<IReceiver> receivers))
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

            if (!listener.CollidesWith(receiver))
            {
                continue;
            }

            receiver.Receive?.Invoke(listener);
            listener.Listen?.Invoke(receiver);
        }
    }

    private void LoadCollisionState(IReadOnlyList<Node> nodes)
    {
        _state = new CollisionState();

        foreach (Node node in nodes)
        {
            if (node is IListener listener && listener.Masks != 0)
            {
                _state.Listeners.Add(listener);
            }

            if (node is IReceiver receiver)
            {
                foreach (int layer in receiver.Layers.EnumerateFlags())
                {
                    _state.AddReceiver(layer, receiver);
                }
            }
        }
    }
}