using System.Text;

namespace SCENeo.Utils;

public static class SIFUtils
{
    private const char Delimiter   = '|';
    private const char EmptySif    = '^';
    private const string Signature = "SIF0";

    private static readonly BiMap<char, SCEColor> SifCodes = new()
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

    public static string Serialize(Grid2DView<Pixel> view)
    {
        return Serialize((IView<Pixel>)view);
    }

    public static string Serialize(IView<Pixel> view)
    {
        if (view.Width == 0 && view.Height == 0)
        {
            return $"{Signature}{EmptySif}";
        }

        var fgBuilder   = new StringBuilder();
        var bgBuilder   = new StringBuilder();
        var charBuilder = new StringBuilder();

        for (int y = 0; y < view.Height; y++)
        {
            for (int x = 0; x < view.Width; x++)
            {
                Pixel pixel = view[x, y];

                fgBuilder.Append(SifCodes.GetTKey(pixel.Colors.ForegroundColor));
                bgBuilder.Append(SifCodes.GetTKey(pixel.Colors.BackgroundColor));
                charBuilder.Append(pixel.Element >= ' ' ? pixel.Element : ' ');
            }
        }

        string fgStr   = RunLengthEncode(fgBuilder.ToString());
        string bgStr   = RunLengthEncode(bgBuilder.ToString());
        string charStr = RunLengthEncode(charBuilder.ToString());

        return $"{Signature}[{view.Width},{view.Height}]{fgStr}${bgStr}${charStr}";
    }

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

        int size = width * height;

        string fgStr = RunLengthDecode(sif[(close + 1)..fgEnd]);

        if (fgStr.Length != size)
        {
            throw new InvalidDataException("Foreground data missing full range.");
        }

        string bgStr = RunLengthDecode(sif[(fgEnd + 1)..bgEnd]);

        if (bgStr.Length != size)
        {
            throw new InvalidDataException("Background data missing full range.");
        }

        string charStr = RunLengthDecode(sif[(bgEnd + 1)..]);

        if (charStr.Length != size)
        {
            throw new InvalidDataException("Character data missing full range.");
        }

        var grid = new Grid2D<Pixel>(width, height);

        int i = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!SifCodes.TryGetUKey(fgStr[i], out SCEColor fg))
                {
                    throw new InvalidDataException($"Unrecognised foreground color code {fgStr[i]}.");
                }

                if (!SifCodes.TryGetUKey(bgStr[i], out SCEColor bg))
                {
                    throw new InvalidDataException($"Unrecognised background color code {fgStr[i]}.");
                }

                grid[x, y] = new Pixel(charStr[i], fg, bg);

                i++;
            }
        }

        return grid;
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
            if (str[i] == last && count < 98)
            {
                count++;
                continue;
            }

            if (count < 4)
            {
                sb.Append(last, count);
            }
            else
            {
                sb.Append($"{Delimiter}{(char)(' ' + (count - 4))}{last}");
            }

            last  = str[i];
            count = 1;
        }

        if (count < 4)
        {
            sb.Append(last, count);
        }
        else
        {
            sb.Append($"{Delimiter}{(char)(' ' + (count - 4))}{last}");
        }

        return sb.ToString();
    }

    private static string RunLengthDecode(string str)
    {
        if (str == string.Empty)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != Delimiter || i >= str.Length - 2)
            {
                sb.Append(str[i]);
                continue;
            }

            int count = str[i + 1] - ' ' + 4;

            sb.Append(str[i + 2], count);

            i += 2;
        }

        return sb.ToString();
    }
}