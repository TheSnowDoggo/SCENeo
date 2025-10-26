using SCENeo.Utils;

namespace SCENeo.Node.Render;

public sealed class RenderEngine : IEngine
{
    private sealed class RenderItem(Grid2DView<Pixel> view, Rect2DI area)
    {
        public Grid2DView<Pixel> View = view;
        public Rect2DI Area = area;
    }

    public bool Enabled { get; set; } = true;

    public Dictionary<int, RenderChannel> Channels { get; init; } = [];

    public void Update(double delta, IReadOnlyList<Node> nodes)
    {
        var channels    = new Dictionary<int, Vec2I>(Channels.Count);
        var renderItems = new Queue<RenderItem>();

        foreach (Node node in nodes)
        {
            if (node is Camera2D camera && Channels.ContainsKey(camera.Channel) && !channels.ContainsKey(camera.Channel))
            {
                channels[camera.Channel] = (Vec2I)camera.GlobalPosition.Round();
            }

            if (node is IRenderable renderable && renderable.Enabled)
            {
                Grid2DView<Pixel> view = renderable.Render();

                Vec2I anchorOffset = renderable.Anchor.AnchorDimension(view.Dimensions) - view.Dimensions;

                Vec2I glob = renderable.Offset + anchorOffset;

                Rect2DI area = new Rect2DI(glob, glob + view.Dimensions);

                renderItems.Enqueue(new RenderItem(view, area));
            }
        }

        foreach ((int channel, Vec2I position) in channels)
        {
            RenderChannel renderChannel = Channels[channel];

            renderChannel.Clear();

            Rect2DI gcArea = new Rect2DI(position, position + renderChannel.Dimensions());

            foreach (RenderItem? renderItem in renderItems)
            {
                if (!gcArea.Overlaps(renderItem.Area))
                {
                    continue;
                }

                Vec2I cameraPos = renderItem.Area.Start - position;

                renderChannel.Load(renderItem.View, cameraPos);
            }
        }
    }
}