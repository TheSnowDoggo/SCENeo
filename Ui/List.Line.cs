namespace SCENeo.Ui;

public sealed partial class List
{
    /// <summary>
    /// A class representing a single line in a <see cref="List"/>.
    /// </summary>
    public class Line : IUpdate
    {
        /// <summary>
        /// Action invoked on property update.
        /// </summary>
        public event Action? OnUpdate;

        public Line()
        {
        }

        private Line _inherited = null!;

        public Line Inherited
        {
            get { return _inherited; }
            set
            {
                if (value != _inherited)
                {
                    return;
                }

                if (_inherited != null)
                {
                    _inherited.OnUpdate -= OnUpdate;
                }

                if (value != null)
                {
                    value.OnUpdate += OnUpdate;
                }

                _inherited = value!;

                OnUpdate?.Invoke();
            }
        }

        private string? _text;

        /// <summary>
        /// Gets or sets the text contents.
        /// </summary>
        public string? Text
        {
            get => _text;
            set => Update(value, ref _text);
        }

        private SCEColor? _fgColor;

        /// <summary>
        /// Gets or sets the text foreground color.
        /// </summary>
        public SCEColor? FgColor
        {
            get => _fgColor;
            set => Update(value, ref _fgColor);
        }

        private SCEColor? _bgColor;

        /// <summary>
        /// Gets or sets the text background color.
        /// </summary>
        public SCEColor? BgColor
        {
            get => _bgColor;
            set => Update(value, ref _bgColor);
        }

        private Anchor? _anchor;

        /// <summary>
        /// Gets or sets the text anchor alignment.
        /// </summary>
        public Anchor? Anchor
        {
            get => _anchor;
            set => Update(value, ref _anchor);
        }

        private bool? _fitToLength;

        /// <summary>
        /// Gets or sets whether the text should be fit to length.
        /// </summary>
        public bool? FitToLength
        {
            get => _fitToLength;
            set => Update(value, ref _fitToLength);
        }

        public string GetText()
        {
            return Text ?? Inherited?.GetText() ?? string.Empty;
        }

        public SCEColor GetFgColor()
        {
            return FgColor ?? Inherited?.GetFgColor() ?? SCEColor.Gray;
        }

        public SCEColor GetBgColor()
        {
            return BgColor ?? Inherited?.GetBgColor() ?? SCEColor.Black;
        }

        public Anchor GetAnchor()
        {
            return Anchor ?? Inherited?.GetAnchor() ?? SCENeo.Anchor.None;
        }

        public bool GetFitToLength()
        {
            return FitToLength ?? Inherited?.GetFitToLength() ?? false;
        }

        public IEnumerable<Line> SubLine(IEnumerable<string> lineText)
        {
            foreach (string text in lineText)
            {
                yield return new Line()
                {
                    Inherited = this,
                    Text = text,
                };
            }
        }

        private void Update<T>(T value, ref T field)
        {
            if (EqualityComparer<T>.Default.Equals(value, field))
            {
                return;
            }

            field = value;

            OnUpdate?.Invoke();
        }
    }
}
