using SCENeo.Node;
using SCENeo.Node.Collision;
using SCENeo.Node.Render;
using SCENeo.UI;
using SCENeo.Utils;

namespace SCENeo.Testing;

internal sealed class Box : Node2D
{
    private static readonly Random _rand = new();

    public Box()
    {
        var dpMap = new DisplayMap(15, 5)
        {
            Anchor = Anchor.Right | Anchor.Bottom,
        };

        dpMap.Fill(() => new Pixel((char)(_rand.Next('a', 'z' + 1)), SCEColor.White, _rand.NextColor()));

        var sprite = new Sprite2D()
        {
            Name     = "Sprite2D",
            Source   = new Stretcher(dpMap)
            {
                ScaleWidth  = 2,
                TextScaling = Stretcher.Scaling.Stretch,
                Bake        = true,
            },
        };

        var collider = new BoxCollider2D()
        {
            Area  = new Rect2D(dpMap.Dimensions),
            Masks = SCEUtils.CreateFlags(0),
        };

        AddChildren(sprite, collider);
    }
}