using System.ComponentModel;

namespace SCENeo.Ui;

/// <summary>
/// A UI control representing a vertical set of options.
/// </summary>
public sealed class ListBox : UiBaseDimensioned, IRenderable
{
    private readonly Image _buffer = new Image();

    private UpdateList<ListBoxItem> _items = [];

    /// <summary>
    /// Gets or sets the list of options.
    /// </summary>
    public UpdateList<ListBoxItem> Items
    {
        get => _items;
        set => ObserveSet(ref _items, value, () => _update = true);
    }

    private Pixel _basePixel = new Pixel(SCEColor.Gray, SCEColor.Black);

    public Pixel BasePixel
    {
        get => _basePixel;
        set => ObserveSet(ref _basePixel, value);
    }

    private StackMode _stackMode;

    public StackMode StackMode
    {
        get => _stackMode;
        set => ObserveSet(ref _stackMode, value);
    }

    private int _selected;

    public int Selected
    {
        get => _selected;
        set => ObserveSet(ref _selected, value);
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

    public void WrapMove(int move)
    {
        Selected = SCEMath.Mod(Selected + move, Height);
    }

    public void LimitMove(int move)
    {
        Selected = Math.Clamp(Selected + move, 0, Height - 1);
    }

    public void ScrollMove(int move)
    {
        if (Items.Count == 0) return;

        Selected = Math.Clamp(Selected + move, 0, Items.Count - 1);

        Scroll = Math.Max(1 + Selected - Height, 0);
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

            ListBoxItem option = Items[index];

            if (option == null)
            {
                ClearLine(y);
                continue;
            }
            
            bool selected = index == _selected;
            
            string text = option.GetText();

            var textMapper = new TextMapper()
            {
                FitToLength =  option.GetFitToLength(),
                Anchor = option.GetAnchor(),
                FgColor = option.GetFgColor(selected),
                BgColor = option.GetBgColor(selected),
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
    
    public int TranslateIndex(int i) => StackMode switch
    {
        StackMode.TopDown  => i,
        StackMode.BottomUp => Height - i - 1,
        _ => throw new InvalidEnumArgumentException(nameof(StackMode), (int)StackMode, typeof(StackMode)),
    };
}