using System.ComponentModel;

namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a vertical set of lines.
/// </summary>
public sealed class List : UiBaseDimensioned, IRenderable
{
    private readonly Image _buffer = new Image();

    private UpdateList<ListItem> _items = [];

    public UpdateList<ListItem> Items
    {
        get => _items;
        set => ObserveSet(ref _items, value, () => _update = true);
    }

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    /// <summary>
    /// Gets or sets the base pixel.
    /// </summary>
    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value);
    }

    private StackMode _stackMode = StackMode.TopDown;

    /// <summary>
    /// Gets or sets the stack mode.
    /// </summary>
    public StackMode StackMode
    {
        get => _stackMode;
        set => ObserveSet(ref _stackMode, value);
    }

    private int _scroll;

    /// <summary>
    /// Gets or sets the scroll.
    /// </summary>
    public int Scroll
    {
        get => _scroll;
        set => ObserveSet(ref _scroll, value);
    }

    public IView<Pixel> Render()
    {
        if (!_update)
        {
            return _buffer;
        }
        
        _update = false;
        
        _buffer.TryCleanResize(Width, Height);

        if (Items == null || Items.Count == 0)
        {
            _buffer.Fill(BasePixel);
            return _buffer;
        }

        for (int y = 0; y < _buffer.Height; y++)
        {
            int index = TranslateIndex(y) + Scroll;

            if (index < 0 || index >= Items.Count)
            {
                ClearLine(y);
                continue;
            }

            ListItem line = Items[index];

            if (line == null)
            {
                ClearLine(y);
                continue;
            }
            
            string text = line.GetText();

            var textMapper = new TextMapper()
            {
                FitToLength = line.GetFitToLength(),
                Anchor = line.GetAnchor(),
                FgColor = line.GetFgColor(),
                BgColor = line.GetBgColor(),
                StartOffset = new Vec2I(0, y),
            };
            
            textMapper.MapText(_buffer, text);
        }

        return _buffer;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < _buffer.Width; x++)
        {
            _buffer[x, y] = BasePixel;
        }
    }

    private int TranslateIndex(int i) => StackMode switch
    {
        StackMode.TopDown  => i,
        StackMode.BottomUp => Height - i - 1,
        _ => throw new InvalidEnumArgumentException(nameof(StackMode), (int)StackMode, typeof(StackMode)),
    };
}