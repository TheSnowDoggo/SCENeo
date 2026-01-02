using static SCENeo.Ui.VerticalSelector;

namespace SCENeo.Ui;

public sealed partial class LineRenderer
{
    /// <summary>
    /// A class representing a single line in a <see cref="LineRenderer"/>.
    /// </summary>
    public sealed class Line : IUpdate
    {
        /// <summary>
        /// Action invoked on property update.
        /// </summary>
        public event Action? OnUpdate;

        public Line()
        {
        }

        private string _text = string.Empty;

        /// <summary>
        /// Gets or sets the text contents.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { Update(value, ref _text); }
        }

        private SCEColor _fgColor = SCEColor.Gray;

        /// <summary>
        /// Gets or sets the text foreground color.
        /// </summary>
        public SCEColor FgColor
        {
            get { return _fgColor; }
            set { Update(value, ref _fgColor); }
        }

        private SCEColor _bgColor = SCEColor.Black;

        /// <summary>
        /// Gets or sets the text background color.
        /// </summary>
        public SCEColor BgColor
        {
            get { return _bgColor; }
            set { Update(value, ref _bgColor); }
        }

        private Anchor _anchor;

        /// <summary>
        /// Gets or sets the text anchor alignment.
        /// </summary>
        public Anchor Anchor
        {
            get { return _anchor; }
            set { Update(value, ref _anchor); }
        }

        private bool _fitToLength;

        /// <summary>
        /// Gets or sets whether the text should be fit to length.
        /// </summary>
        public bool FitToLength
        {
            get { return _fitToLength; }
            set { Update(value, ref _fitToLength); }
        }

        public Line FromText(string text)
        {
            return new Line()
            {
                Text = text,
                FgColor = FgColor,
                BgColor = BgColor,
                Anchor = Anchor,
                FitToLength = FitToLength,
            };
        }

        public UpdateCollection<Line> FromArray(params string[] text)
        {
            var collection = new UpdateCollection<Line>(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                collection.Add(FromText(text[i]));
            }

            return collection;
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
