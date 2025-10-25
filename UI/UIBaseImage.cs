namespace SCENeo.UI;

public abstract class UIBaseImage : UIBase<Image>
{
    public UIBaseImage(int width, int height) : base(new Image(width, height)) { }

    public UIBaseImage(Vec2I dimensions) : base(new Image(dimensions)) { }

    protected abstract void Update();

    public override Grid2DView<Pixel> Render()
    {
        Update();
        return _data;
    }
}