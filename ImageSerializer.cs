using System.Text;

namespace SCENeo;

public static class ImageSerializer
{
    private const byte TransparentSignature = 84;
    private const byte OpaqueSignature = 79;

    public static void Serialize(Stream stream, IView<Pixel> view, bool opaque = false)
    {
        stream.WriteByte(opaque ? OpaqueSignature : TransparentSignature);

        stream.Write(BitConverter.GetBytes(view.Width));
        stream.Write(BitConverter.GetBytes(view.Height));

        char[] buffer = new char[1];

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                Pixel pixel = view[x, y];

                buffer[0] = pixel.Element;
                stream.WriteByte(Encoding.UTF8.GetBytes(buffer)[0]);

                if (opaque)
                {
                    stream.WriteByte(PackColors(pixel.FgColor, pixel.BgColor));
                    continue;
                }

                stream.WriteByte((byte)pixel.FgColor);
                stream.WriteByte((byte)pixel.BgColor);
            }
        }
    }

    public static void Serialize(string path, IView<Pixel> view, bool opaque = false)
    {
        using (var stream = File.Open(path, FileMode.Create))
        {
            Serialize(stream, view, opaque);
        }
    }

    public static Grid2D<Pixel> Deserialize(Stream stream)
    {
        byte encoding = (byte)stream.ReadByte();

        bool opaque = encoding switch
        {
        OpaqueSignature      => true,
        TransparentSignature => false,
        _ => throw new InvalidDataException($"Unknown encoding type {encoding}.")
        };

        int width  = stream.ReadInt32();
        int height = stream.ReadInt32();

        var grid = new Grid2D<Pixel>(width, height);

        byte[] buffer = new byte[1];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                buffer[0] = (byte)stream.ReadByte();
                char element = Encoding.UTF8.GetChars(buffer)[0];

                SCEColor fgColor;
                SCEColor bgColor;

                if (opaque)
                {
                    UnpackColors((byte)stream.ReadByte(), out fgColor, out bgColor);
                }
                else
                {
                    fgColor = (SCEColor)stream.ReadByte();
                    bgColor = (SCEColor)stream.ReadByte();
                }

                grid[x, y] = new Pixel(element, fgColor, bgColor);
            }
        }

        return grid;
    }

    public static Grid2D<Pixel> Deserialize(string path)
    {
        using (var stream = File.OpenRead(path))
        {
            return Deserialize(stream);
        }
    }

    private static byte PackColors(SCEColor fgColor, SCEColor bgColor)
    {
        if (fgColor == SCEColor.Transparent)
        {
            fgColor = SCEColor.Black;
        }

        if (bgColor == SCEColor.Transparent)
        {
            bgColor = SCEColor.Black;
        }

        return (byte)((int)fgColor + (((int)bgColor) << 4));
    }

    private static void UnpackColors(byte packedColors, out SCEColor fgColor, out SCEColor bgColor)
    {
        fgColor = (SCEColor)(packedColors & 0xF);
        bgColor = (SCEColor)(packedColors >> 4);
    }

    private static int ReadInt32(this Stream stream)
    {
        Span<byte> span = stackalloc byte[4];

        stream.ReadExactly(span);

        return BitConverter.ToInt32(span);
    }
}