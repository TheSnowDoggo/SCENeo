using System.Text;

namespace SCENeo;

/// <summary>
/// A static class containing utility functions for serializing and deserializing images to the SIF format.
/// </summary>
public static class SIFUtils
{
    private const char Delimiter   = '|';
    private const char EmptySif    = '^';
    private const string Signature = "SIF0";

    public static IReadOnlyBiMap<char, SCEColor> SifCodes { get; } = new BiMap<char, SCEColor>()
    {
        { 'K', SCEColor.Black       },
        { 'B', SCEColor.DarkBlue    },
        { 'G', SCEColor.DarkGreen   },
        { 'C', SCEColor.DarkCyan    },
        { 'R', SCEColor.DarkRed     },
        { 'M', SCEColor.DarkMagenta },
        { 'Y', SCEColor.DarkYellow  },
        { 's', SCEColor.Gray        },
        { 'S', SCEColor.DarkGray    },
        { 'b', SCEColor.Blue        },
        { 'g', SCEColor.Green       },
        { 'c', SCEColor.Cyan        },
        { 'r', SCEColor.Red         },
        { 'm', SCEColor.Magenta     },
        { 'y', SCEColor.Yellow      },
        { 'W', SCEColor.White       },
        { 'N', SCEColor.Transparent },
    };

    /// <summary>
    /// Serializes the image to a SIF.
    /// </summary>
    /// <param name="image">The image to serialize.</param>
    /// <returns>The serialized SIF.</returns>
    public static string Serialize(Grid2DView<Pixel> image)
    {
        return Serialize((IView<Pixel>)image);
    }

    /// <inheritdoc cref="Serialize(Grid2DView{Pixel})"/>
    public static string Serialize(IView<Pixel> image)
    {
        if (image.Width == 0 && image.Height == 0)
        {
            return $"{Signature}{EmptySif}";
        }

        var fgBuilder   = new StringBuilder();
        var bgBuilder   = new StringBuilder();
        var charBuilder = new StringBuilder();

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Pixel pixel = image[x, y];

                fgBuilder.Append(SifCodes.GetKey1(pixel.FgColor));
                bgBuilder.Append(SifCodes.GetKey1(pixel.BgColor));
                charBuilder.Append(pixel.Element >= ' ' ? pixel.Element : ' ');
            }
        }

        string fgStr = fgBuilder.ToString();
        fgStr = Same(fgStr) ? fgStr[0].ToString() : RunLengthEncode(fgStr);

        string bgStr   = bgBuilder.ToString();
        bgStr = Same(bgStr) ? bgStr[0].ToString() : RunLengthEncode(bgStr);

        string charStr = charBuilder.ToString();
        charStr = Same(charStr) ? charStr[0].ToString() : RunLengthEncode(charStr);

        return $"{Signature}[{image.Width},{image.Height}]{fgStr}${bgStr}${charStr}$";
    }

    /// <summary>
    /// Deserializes the SIF to an image.
    /// </summary>
    /// <param name="sif">The SIF to deserialize.</param>
    /// <returns>The deserialized grid.</returns>
    public static Grid2D<Pixel> Deserialize(string sif)
    {
        if (!sif.StartsWith(Signature))
        {
            throw new InvalidDataException($"Expected signature {Signature}.");
        }

        if (sif.Length == Signature.Length)
        {
            throw new InvalidDataException("SIF missing data.");
        }

        if (sif[Signature.Length] == EmptySif)
        {
            return new Grid2D<Pixel>();
        }

        if (sif[Signature.Length] != '[')
        {
            throw new InvalidDataException("Expected [ after signature.");
        }

        int comma = sif.IndexOf(',', Signature.Length);

        if (comma == -1)
        {
            throw new InvalidDataException("Could not find , dimension seperator.");
        }

        int close = sif.IndexOf(']', comma + 1);

        if (close == -1)
        {
            throw new InvalidDataException("Could not find matching ].");
        }

        string widthStr = sif[(Signature.Length + 1)..comma];

        if (!int.TryParse(widthStr, out int width) || width < 0)
        {
            throw new InvalidDataException($"Width {widthStr} was invalid.");
        }

        string heightStr = sif[(comma + 1)..close];

        if (!int.TryParse(heightStr, out int height) || height < 0)
        {
            throw new InvalidDataException($"Height {heightStr} was invalid.");
        }

        int fgEnd = sif.IndexOf('$', close + 1);

        if (fgEnd == -1)
        {
            throw new InvalidDataException("Could not find foreground end delimiter $.");
        }

        int bgEnd = sif.IndexOf('$', fgEnd + 1);

        if (bgEnd == -1)
        {
            throw new InvalidDataException("Could not find background end delimiter $.");
        }

        if (sif[^1] != '$')
        {
            throw new InvalidDataException("Could not find end delimiter $.");
        }

        int size = width * height;

        SCEColor? fgColorDef = null;

        string fgStr = RunLengthDecode(sif[(close + 1)..fgEnd]);

        if (fgStr.Length == 1)
        {
            fgColorDef = GetColor(fgStr[0]);
        }
        else if (fgStr.Length != size)
        {
            throw new InvalidDataException("Foreground data missing full range.");
        }

        SCEColor? bgColorDef = null;

        string bgStr = RunLengthDecode(sif[(fgEnd + 1)..bgEnd]);

        if (bgStr.Length == 1)
        {
            bgColorDef = GetColor(bgStr[0]);
        }
        else if (bgStr.Length != size)
        {
            throw new InvalidDataException("Background data missing full range.");
        }

        char charDef = '\0';

        string charStr = RunLengthDecode(sif[(bgEnd + 1)..^1]);

        if (charStr.Length == 1)
        {
            charDef = charStr[0];
        }
        else if (charStr.Length != size)
        {
            throw new InvalidDataException("Character data missing full range.");
        }

        var grid = new Grid2D<Pixel>(width, height);

        int i = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = charDef != '\0' ? charDef : charStr[i];
                SCEColor fg = fgColorDef ?? GetColor(fgStr[i]);
                SCEColor bg = bgColorDef ?? GetColor(bgStr[i]);

                grid[x, y] = new Pixel(c, fg, bg);

                i++;
            }
        }

        return grid;
    }

    private static bool Same(string s)
    {
        if (s == string.Empty)
        {
            return false;
        }

        for (int i = 1; i < s.Length; i++)
        {
            if (s[i - 1] != s[i])
            {
                return false;
            }
        }

        return true;
    }

    private static SCEColor GetColor(char code)
    {
        if (!SifCodes.TryGetKey2(code, out SCEColor color))
        {
            throw new InvalidDataException($"Unrecognised color code {code}.");
        }
        return color;
    }

    private static string RunLengthEncode(string str)
    {
        if (str == string.Empty)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        char last = str[0];
        int count = 1;

        for (int i = 1; i < str.Length; i++)
        {
            if (str[i] == last)
            {
                count++;
                continue;
            }

            AppendEncode(sb, last, count);

            last  = str[i];
            count = 1;
        }

        AppendEncode(sb, last, count);

        return sb.ToString();
    }

    private static string RunLengthDecode(string str)
    {
        if (str == string.Empty)
        {
            return string.Empty;
        }

        bool delimited = false;

        var sb = new StringBuilder();

        for (int i = 0; i < str.Length; i++)
        {
            // ignore non-space control characters
            if (str[i] < ' ')
            {
                continue;
            }

            if (str[i] == Delimiter)
            {
                if (delimited && i < str.Length - 1 && str[i + 1] == Delimiter)
                {
                    sb.Append(Delimiter);
                }

                delimited = !delimited;
                continue;
            }

            if (delimited || !char.IsDigit(str[i]))
            {
                sb.Append(str[i]);
                continue;
            }

            int end = str.IndexOf(c => !char.IsDigit(c), i + 1);

            if (end == -1)
            {
                throw new InvalidDataException("Run count missing associated character.");
            }

            string countStr = str[i..end];

            if (!int.TryParse(countStr, out int count) || count < 0)
            {
                throw new InvalidDataException($"Count {countStr} was invalid.");
            }

            char c = str[end];

            if (c == Delimiter)
            {
                if (i == str.Length - 1)
                {
                    throw new InvalidDataException("Delimiter must be followed by a character.");
                }

                end++;

                c = str[end];
            }

            sb.Append(c, count);

            i = end;
        }

        return sb.ToString();
    }

    private static void AppendSingle(StringBuilder sb, char c)
    {
        if (c == Delimiter)
        {
            sb.Append(Delimiter, 2);
            return;
        }

        if (!char.IsDigit(c))
        {
            sb.Append(c);
            return;
        }

        sb.Append($"{Delimiter}{c}{Delimiter}");
    }

    private static void AppendEncode(StringBuilder sb, char c, int count)
    {
        if (count == 1)
        {
            AppendSingle(sb, c);
            return;
        } 

        if (!char.IsDigit(c) && c != Delimiter)
        {
            sb.Append($"{count}{c}");
            return;
        }

        sb.Append($"{count}{Delimiter}{c}");
    }
}