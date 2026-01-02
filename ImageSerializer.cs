using System.Text.Json;

namespace SCENeo;

public static class ImageSerializer
{
    public static void Serialize(Stream stream, IView<Pixel> view)
    {
        stream.Write(BitConverter.GetBytes(view.Width));
        stream.Write(BitConverter.GetBytes(view.Height));

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                Pixel pixel = view[x, y];

                stream.Write(BitConverter.GetBytes(pixel.Element));
                stream.WriteByte((byte)pixel.FgColor);
                stream.WriteByte((byte)pixel.BgColor);
            }
        }
    }

    public static void Serialize(string path, IView<Pixel> view)
    {
        using Stream stream = File.Open(path, FileMode.Create);

        Serialize(stream, view);
    }

    public static Grid2D<Pixel> Deserialize(Stream stream)
    {
        int width = stream.ReadInt32();
        int height = stream.ReadInt32();

        var grid = new Grid2D<Pixel>(width, height);

        Span<byte> span = stackalloc byte[2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                stream.ReadExactly(span);
                char element = BitConverter.ToChar(span);

                SCEColor fgColor = (SCEColor)stream.ReadByte();
                SCEColor bgColor = (SCEColor)stream.ReadByte();

                grid[x, y] = new Pixel(element, fgColor, bgColor);
            }
        }

        return grid;
    }

    public static Grid2D<Pixel> Deserialize(string path)
    {
        using Stream stream = File.OpenRead(path);

        return Deserialize(stream);
    }

    private static int ReadInt32(this Stream stream)
    {
        Span<byte> span = stackalloc byte[4];

        stream.ReadExactly(span);

        return BitConverter.ToInt32(span);
    }
}