using SCENeo.Utils;

namespace SCENeo.Node.Render;

public sealed class RenderEngine : IEngine
{
    private sealed class RenderInput(IRenderable renderable, Rect2DI renderArea)
    {
        public readonly IRenderable Renderable = renderable;
        public readonly Rect2DI     RenderArea = renderArea;
    }

    private sealed class RenderOutput(RenderChannel channel, Rect2DI renderArea)
    {
        public readonly RenderChannel Channel    = channel;
        public readonly Rect2DI       RenderArea = renderArea;
    }

    private sealed class RenderState
    {
        public readonly List<RenderInput>  Inputs  = [];
        public readonly List<RenderOutput> Outputs = [];
    }

    public bool Enabled { get; set; } = true;

    public Dictionary<int, RenderChannel> Channels { get; init; } = [];

    public void Update(double _, IReadOnlyList<Node> nodes)
    {
        Render(LoadRenderState(nodes));
    }

    private static void Render(RenderState renderState)
    {
        foreach (RenderOutput output in renderState.Outputs)
        {
            output.Channel.Clear();
        }

        foreach (RenderInput input in renderState.Inputs)
        {
            Grid2DView<Pixel>? view = null;

            foreach (RenderOutput output in renderState.Outputs)
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

                output.Channel.Load(view, screenPosition);
            }
        }
    }

    private static RenderInput CreateInput(IRenderable renderable)
    {
        Vec2I dimensions = renderable.Dimensions();

        Vec2I anchorOffset = renderable.Anchor.AnchorDimension(dimensions) - dimensions;

        Vec2I renderPosition = renderable.Offset + anchorOffset;

        Rect2DI renderArea = Rect2DI.Area(renderPosition, dimensions);

        return new RenderInput(renderable, renderArea);
    }

    private RenderOutput CreateOutput(Camera2D camera)
    {
        RenderChannel renderChannel = Channels[camera.Channel];

        Rect2DI renderArea = Rect2DI.Area(camera.RenderPosition(), renderChannel.Dimensions());

        return new RenderOutput(renderChannel, renderArea);
    }

    private RenderState LoadRenderState(IReadOnlyList<Node> nodes)
    {
        var state = new RenderState();

        var channels = new HashSet<int>();

        foreach (Node node in nodes)
        {
            if (node is IRenderable renderable && renderable.Enabled)
            {
                state.Inputs.Add(CreateInput(renderable));
            }

            if (node is Camera2D camera && Channels.ContainsKey(camera.Channel) && channels.Add(camera.Channel))
            {
                state.Outputs.Add(CreateOutput(camera));
            }
        }

        return state;
    }
}