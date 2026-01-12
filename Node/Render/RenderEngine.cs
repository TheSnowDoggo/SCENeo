namespace SCENeo.Node.Render;

public sealed class RenderEngine : IEngine
{
    private sealed class RenderInput(IRenderable renderable, Rect2DI renderArea)
    {
        public readonly IRenderable Renderable = renderable;
        public readonly Rect2DI     RenderArea = renderArea;

        public static RenderInput Create(IRenderable renderable)
        {
            Vec2I size = renderable.Size();

            Vec2I anchorOffset = renderable.Anchor.AnchorDimension(size) - size;

            Vec2I renderPosition = renderable.Offset + anchorOffset;

            Rect2DI renderArea = Rect2DI.Area(renderPosition, size);

            return new RenderInput(renderable, renderArea);
        }
    }

    private sealed class RenderOutput(RenderChannel channel, Rect2DI renderArea)
    {
        public readonly RenderChannel Channel    = channel;
        public readonly Rect2DI       RenderArea = renderArea;

        public static RenderOutput Create(RenderChannel renderChannel, Vec2I renderPosition)
        {
            Rect2DI renderArea = Rect2DI.Area(renderPosition, renderChannel.Size());

            return new RenderOutput(renderChannel, renderArea);
        }
    }

    private sealed class RenderState
    {
        public readonly List<RenderInput>  Inputs  = [];
        public readonly List<RenderOutput> Outputs = [];
    }

    private RenderState _state = null!;

    public bool Enabled { get; set; } = true;

    public Dictionary<int, RenderChannel> Channels { get; init; } = [];

    public void Update(double _, IReadOnlyList<Node> nodes)
    {
        LoadRenderState(nodes);

        InitializeOutputs();

        Render();
    }

    private void InitializeOutputs()
    {
        foreach (RenderOutput output in _state.Outputs)
        {
            output.Channel.Initialize();
        }
    }

    private void Render()
    {
        foreach (RenderInput input in _state.Inputs)
        {
            IView<Pixel>? view = null;

            foreach (RenderOutput output in _state.Outputs)
            {
                if (!input.RenderArea.Overlaps(output.RenderArea))
                {
                    continue;
                }

                Vec2I screenPosition = input.RenderArea.Start - output.RenderArea.Start;

                if (view == null)
                {
                    view = input.Renderable.Render();
                }

                output.Channel.Load(view, screenPosition, input.Renderable.Layer);
            }
        }
    }

    private void LoadRenderState(IReadOnlyList<Node> nodes)
    {
        _state = new RenderState();

        var channels = new HashSet<int>();

        foreach (Node node in nodes)
        {
            if (node is IRenderable renderable && renderable.Visible)
            {
                _state.Inputs.Add(RenderInput.Create(renderable));
                continue;
            }

            if (node is Camera2D camera && Channels.TryGetValue(camera.Channel, out RenderChannel? renderChannel) 
                && channels.Add(camera.Channel))
            {
                Vec2I position = camera.RenderPosition() - camera.Anchor.AnchorDimension(renderChannel.Size());

                _state.Outputs.Add(RenderOutput.Create(renderChannel, position));
            }
        }
    }
}