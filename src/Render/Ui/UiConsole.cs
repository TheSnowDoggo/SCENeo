using System.Text;

namespace SCENeo.Ui;

public sealed class UiConsole : TextWriter, IRenderable
{
    public const int DefaultBufferWidth  = 200;
    public const int DefaultBufferHeight = 9000;

    private readonly Image _visual = [];

    private ShiftBuffer<Pixel> _buffer = [];

    private bool _update;

    private bool _bufferUpdate;

    public UiConsole()
    {
    }

    /// <inheritdoc cref="UiBase.Visible"/>
    public bool Visible { get; set; } = true;

    /// <inheritdoc cref="UiBase.Offset"/>
    public Vec2I Offset { get; set; }

    /// <inheritdoc cref="UiBase.Layer"/>
    public int Layer { get; set; }

    /// <inheritdoc cref="UiBase.Anchor"/>
    public Anchor Anchor { get; set; }

    private int _width;

    /// <summary>
    /// Gets or sets the console width.
    /// </summary>
    public int Width
    {
        get { return _width; }
        set { SCEUtils.ObserveSet(value, ref _width, ref _update); }
    }

    private int _height;

    /// <summary>
    /// Gets or sets the console height.
    /// </summary>
    public int Height
    {
        get { return _height; }
        set { SCEUtils.ObserveSet(value, ref _height, ref _update); }
    }

    private Vec2I _scroll;

    /// <summary>
    /// Gets or sets the console scroll.
    /// </summary>
    public Vec2I Scroll
    {
        get { return _scroll; }
        set { SCEUtils.ObserveSet(value, ref _scroll, ref _update); }
    }

    private int _bufferWidth;

    /// <summary>
    /// Gets or sets the width of the buffer.
    /// </summary>
    /// <remarks>
    /// Note: Changing the width will clear the buffer and reset cursor position.
    /// </remarks>
    public int BufferWidth
    {
        get
        {
            return _bufferWidth;
        }
        set
        {
            if (value == _bufferWidth)
            {
                return;
            }

            _bufferWidth = value;

            _bufferUpdate = true;

            _cursorPosition = Vec2I.Zero;
        }
    }

    private int _bufferHeight;

    /// <summary>
    /// Gets or sets the height of the buffer.
    /// </summary>
    /// <remarks>
    /// Note: Changing the height will clear the buffer and reset cursor position.
    /// </remarks>
    public int BufferHeight
    {
        get
        {
            return _bufferHeight;
        }
        set
        {
            if (value == _bufferHeight)
            {
                return;
            }

            _bufferHeight = value;

            _bufferUpdate = true;

            _cursorPosition = Vec2I.Zero;
        }
    }

    public SCEColor ForegroundColor { get; set; } = SCEColor.Gray;
    public SCEColor BackgroundColor { get; set; }

    public Pixel BasePixel { get; set; }

    private Vec2I _cursorPosition;

    public Vec2I CursorPosition
    {
        get
        {
            return _cursorPosition;
        }
        set
        {
            if (value.X < 0 || value.Y < 0 ||
                value.X >= BufferWidth || value.Y >= BufferHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Cursor position was invalid.");
            }

            _cursorPosition = value;
        }
    }

    public int CursorIndex
    {
        get { return ToCursorIndex(); }
        set { CursorPosition = ToCursorPosition(value); }
    }

    private int _tabWidth;

    public int TabWidth
    {
        get
        {
            return _tabWidth;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Tab width cannot be negative.");
            }

            _tabWidth = value;
        }
    }

    public bool Autoscroll { get; set; } = true;

    public override Encoding Encoding { get { return Encoding.UTF8; } }

    public override void Write(char c)
    {
        HandleBufferUpdate();
        
        switch (c)
        {
        case '\n':
        case '\f':
        case '\v':
            Newline();
            break;
        case '\r':
            _cursorPosition.X = 0;
            break;
        case '\t':
            for (int i = 0; i < TabWidth; i++)
            {
                Write(' ');
            }
            break;
        case '\b':
            if (_cursorPosition.X > 0)
            {
                _cursorPosition.X--;
                break;
            }

            if (_cursorPosition.Y > 0)
            {
                _cursorPosition.Y--;
            }

            break;
        default:
            if (c < ' ')
            {
                break;
            }

            _buffer[ToCursorIndex()] = new Pixel(c, ForegroundColor, BackgroundColor);

            _cursorPosition.X++;

            if (_cursorPosition.X < BufferWidth)
            {
                break;
            }

            Newline();

            break;
        }

        _update = true;
    }

    public IView<Pixel> Render()
    {
        if (_update)
        {
            Update();
        }

        return _visual.AsReadonly();
    }

    public void Clear()
    {
        _buffer.Fill(BasePixel);
        CursorPosition = Vec2I.Zero;
        Scroll = Vec2I.Zero;
        _update = true;
    }

    public void MoveCursor(int move)
    {
        CursorIndex = Math.Clamp(CursorIndex + move, 0, _buffer.Count - 1);
    }

    public void ShiftLeft(int index, int count, int shift)
    {
        if (shift < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shift), shift, "Shift must be positive.");
        }

        if (shift == 0 || count <= 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            int current = index + i;
            _buffer[current - shift] = _buffer[current];
        }

        _update = true;
    }

    public void ShiftRight(int index, int count, int shift)
    {
        if (shift < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shift), shift, "Shift must be positive.");
        }

        if (shift == 0 || count <= 0)
        {
            return;
        }

        for (int i = count - 1; i >= 0; i--)
        {
            int current = index + i;
            _buffer[current + shift] = _buffer[current];
        }

        _update = true;
    }

    private void HandleBufferUpdate()
    {
        if (!_bufferUpdate)
        {
            return;
        }

        _buffer = new ShiftBuffer<Pixel>(_bufferWidth * _bufferHeight);
        _buffer.Fill(BasePixel);

        _bufferUpdate = false;
    }

    private void Newline()
    {
        _cursorPosition.X = 0;

        if (_cursorPosition.Y < BufferHeight - 1)
        {
            _cursorPosition.Y++;

            if (!Autoscroll)
            {
                return;
            }

            if (_cursorPosition.Y - _scroll.Y != Height)
            {
                return;
            }

            _scroll.Y++;

            return;
        }

        _buffer.Shift(BufferWidth);

        int start = BufferWidth * (BufferHeight - 1);

        for (int i = 0; i < BufferWidth; i++)
        {
            _buffer[start + i] = BasePixel;
        }
    }

    private void Update()
    {
        if (_visual.Width != Width || _visual.Height != Height)
        {
            _visual.CleanResize(Width, Height);
        }

        HandleBufferUpdate();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int bufferIndex = Translate(x + _scroll.X, y + _scroll.Y);

                _visual[x, y] = GetBufferOrDefault(bufferIndex);
            }
        }

        _update = false;
    }

    private Pixel GetBufferOrDefault(int index)
    {
        if (index < 0 || index >= _buffer.Count)
        {
            return BasePixel;
        }

        return _buffer[index];
    }

    private int Translate(int x, int y)
    {
        return y * BufferWidth + x;
    }

    private int ToCursorIndex()
    {
        return CursorPosition.Y * BufferWidth + CursorPosition.X;
    }

    private Vec2I ToCursorPosition(int cursorIndex)
    {
        return new Vec2I(cursorIndex % BufferWidth, cursorIndex / BufferWidth);
    }
}
