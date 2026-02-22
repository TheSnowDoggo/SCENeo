using System.ComponentModel;
using System.Text;
using SCENeo.Ui;

namespace SCENeo;

public sealed class TextMapper
{
	public const int AutoWidth = -1;
	
	public bool FitToLength { get; set; }
    public bool FitToHeight { get; set; }
	public TextWrapping Wrapping { get; set; }
    public Anchor Anchor { get; set; }

    public SCEColor FgColor { get; set; } = SCEColor.Gray;
    public SCEColor BgColor { get; set; } = SCEColor.Black;

	public int MaxWidth { get; set; } = AutoWidth;
	public int MaxHeight { get; set; } = AutoWidth;
    
    public Vec2I StartOffset { get; set; }
	
	public static string[] CharacterSplitLines(string text, int width)
    {
        var list = new List<string>();

        var sb = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            bool newline = text[i] == '\n';

            if (!newline)
            {
                sb.Append(text[i]);
            }

            if (!newline && (sb.Length != width || i != text.Length - 1 && text[i + 1] == '\n'))
            {
                continue;
            }
            
            list.Add(sb.ToString());
            sb.Clear();
        }

        if (sb.Length != 0)
        {
            list.Add(sb.ToString());
        }

        return [..list];
    }

    public static string[] WordSplitLines(string text, int width)
    {
        var list = new List<string>();

        var wordBuilder = new StringBuilder();
        var lineBuilder = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            bool newLine = text[i] == '\n';

            if (text[i] != ' ' && !newLine)
            {
                wordBuilder.Append(text[i]);
                
                if (i != text.Length - 1)
                {
                    continue;
                }
            }

            if (newLine || lineBuilder.Length + wordBuilder.Length >= width)
            {
                list.Add(lineBuilder.ToString());
                lineBuilder.Clear();
            }

            lineBuilder.Append(wordBuilder);
            wordBuilder.Clear();

            if (lineBuilder.Length < width)
            {
                lineBuilder.Append(' ');
            }
        }

        if (lineBuilder.Length != 0)
        {
            list.Add(lineBuilder.ToString());
        }

        return [..list];
    }
    
    public static string[] SplitLines(string text, int width, TextWrapping wrapping) => wrapping switch
    {
        TextWrapping.None      => text.Split('\n'),
        TextWrapping.Character => CharacterSplitLines(text, width),
        TextWrapping.Word      => WordSplitLines(text, width),
        _ => throw new InvalidEnumArgumentException(nameof(wrapping), (int)wrapping, typeof(TextWrapping)),
    };
	
	public void MapText(Image image, string text)
    {
        if (StartOffset.X >= image.Width || StartOffset.Y >= image.Height)
        {
            return;
        }

        int startX = Math.Max(StartOffset.X, 0);
        int startY = Math.Max(StartOffset.Y, 0);
        
        int width = MaxWidth < 0 ? image.Width : MaxWidth;
        width = Math.Min(width, image.Width - startX);
        
        int height = MaxHeight < 0 ? image.Height : MaxHeight;
        height = Math.Min(height, image.Height - startY);
        
        string[] lines = SplitLines(text, width, Wrapping);

        int yAnchor = startY + Anchor.AnchorVertical(height - lines.Length);
        
        int top = Math.Max(yAnchor, startY);
        int bottom = Math.Min(yAnchor + lines.Length, startY + height);

        for (int y = top; y < bottom; y++)
        {
            string line = lines[y - yAnchor];
            
            if (FitToLength)
            {
                image.MapLine(line.FitToLength(width, Anchor), startX, y, FgColor, BgColor);
                continue;
            }
            
            int x = startX + Anchor.AnchorHorizontal(width - line.Length);
            image.MapLine(line, x, y, FgColor, BgColor);
        }

        if (!FitToHeight)
        {
            return;
        }

        var fill = new Pixel(' ', FgColor, BgColor);

        int right = startX + width;
        
        // top fill
        image.Fill(fill, startX, startY, right, top);
        // bottom fill
        image.Fill(fill, startX, bottom, right, startY + height);
    }
}