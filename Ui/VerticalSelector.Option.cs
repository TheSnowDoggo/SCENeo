using static SCENeo.Ui.LineRenderer;

namespace SCENeo.Ui;

public sealed partial class VerticalSelector
{
    /// <summary>
    /// A class representing an option for <see cref="VerticalSelector"/>.
    /// </summary>
    public sealed class Option : IUpdate
    {
        public event Action? OnUpdate;

        public Option()
        {
        }

        private string _text = string.Empty;

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { Update(value, ref _text); }
        }

        private SCEColor _selectedFgColor = SCEColor.Black;

        /// <summary>
        /// Gets or sets the selected foreground color.
        /// </summary>
        public SCEColor SelectedFgColor
        {
            get { return _selectedFgColor; }
            set { Update(value, ref _selectedFgColor); }
        }

        private SCEColor _selectedBgColor = SCEColor.White;

        /// <summary>
        /// Gets or sets the selected background color.
        /// </summary>
        public SCEColor SelectedBgColor
        {
            get { return _selectedBgColor; }
            set { Update(value, ref _selectedBgColor); }
        }

        private SCEColor _unselectedFgColor = SCEColor.White;

        /// <summary>
        /// Gets or sets the unselected foreground color.
        /// </summary>
        public SCEColor UnselectedFgColor
        {
            get { return _unselectedFgColor; }
            set { Update(value, ref _unselectedFgColor); }
        }

        private SCEColor _unselectedBgColor = SCEColor.Black;

        /// <summary>
        /// Gets or sets the unselected background color.
        /// </summary>
        public SCEColor UnselectedBgColor
        {
            get { return _unselectedBgColor; }
            set { Update(value, ref _unselectedBgColor); }
        }

        private Anchor _anchor = Anchor.None;

        /// <summary>
        /// Gets or sets the option text anchoring.
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

        /// <summary>
        /// Action invoked when the option is chosen.
        /// </summary>
        public Action? OnChoose { get; set; }

        public Option FromText(string text)
        {
            return new Option()
            {
                Text = text,
                SelectedFgColor = SelectedFgColor,
                SelectedBgColor = SelectedBgColor,
                UnselectedFgColor = UnselectedFgColor,
                UnselectedBgColor = UnselectedBgColor,
                Anchor = Anchor,
                FitToLength = FitToLength,
                OnChoose = OnChoose,
            };
        }

        public UpdateCollection<Option> FromArray(params string[] text)
        {
            var collection = new UpdateCollection<Option>(text.Length);

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