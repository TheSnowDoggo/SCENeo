namespace SCENeo;

internal class Display(int width, int height)
{
    private readonly Grid2D<Pixel> _grid = new(width, height);
}