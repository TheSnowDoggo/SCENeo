namespace SCENeo.UI;

public sealed class Filter<T>(T source) : UIModifier<T>(source)
    where T : IRenderable, IDimensioned
{
    public Func<Pixel, Pixel>? FilterMode = null;

    public override Grid2DView<Pixel> Render()
    {
        Grid2DView<Pixel> view = _source.Render();

        if (FilterMode == null) return view;

        Image image = new Image(view.Dimensions);

        image.Fill((x, y) => FilterMode.Invoke(view[x, y]));

        return image;
    }
}