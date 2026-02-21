namespace SCENeo.Ui;

public sealed class WideRotater : UiModifier<IRenderable>
{
    private static readonly Vec2I Wide = new Vec2I(2, 1);

    public override int Width => Rotation switch
    {
        Rotation.None or Rotation.Flip  => Source.Width,
        Rotation.Left or Rotation.Right => Source.Height * 2,
        _ => throw new InvalidDataException($"Rotation is invalid {Rotation}")
    };

    public override int Height => Rotation switch
    {
        Rotation.None or Rotation.Flip  => Source.Height,
        Rotation.Left or Rotation.Right => Source.Width / 2,
        _ => throw new InvalidDataException($"Rotation is invalid {Rotation}")
    };

    public Rotation Rotation { get; set; }

    public override IView<Pixel> Render()
    {
        IView<Pixel> view = Source.Render();

        return Rotation switch
        {
            Rotation.None  => view,
            Rotation.Right => Rotated90(view),
            Rotation.Flip  => Grid2D<Pixel>.Rotated180(view),
            Rotation.Left  => Rotated90(view),
            _ => throw new InvalidDataException($"Rotation is invalid {Rotation}"),
        };
    }

    private Grid2D<Pixel> Rotated90(IView<Pixel> view)
    {
        Vec2I size = SquareSize(view);

        var grid = new Grid2D<Pixel>(Width, Height);

        if (grid.Length == 0)
        {
            return grid;
        }

        Midpoint(size, out Vec2 midpoint, out Vec2 change);

        foreach (Vec2I position in SCEUtils.EnumerateArea(size))
        {
            Vec2I rotated = (Vec2I)((position - midpoint).Rotated(Rotation) + change) * Wide;
            Pixel pixel = view[position * Wide];

            grid[rotated] = pixel;
            grid[rotated + Vec2I.Right] = pixel;
        }

        return grid;
    }

    private static Vec2I SquareSize(IView<Pixel> view)
    {
        return new Vec2I(view.Width / 2, view.Height);
    }

    private static void Midpoint(Vec2I size, out Vec2 midpoint, out Vec2 change)
    {
        midpoint = (size - Vec2.One) / 2;
        change = midpoint.Inverted();
    }
}