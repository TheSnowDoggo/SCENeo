namespace SCENeo;

public interface IDimensioned
{
    int Width { get; }
    int Height { get; }
    int Size { get; }
    Vec2I Dimensions { get; }
}
