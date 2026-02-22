namespace SCENeo.Ui;

public class HierarchyNode : UpdateBase
{
	private ChildList _children;

	public ChildList Children
	{
		get => _children;
		set
		{
			if (ObserveSet(ref _children, value, Update))
			{
				_children.Parent = this;
			}
		}
	}

	public HierarchyNode Parent { get; set; }

	private string _text;

	public string Text
	{
		get => _text;
		set => ObserveSet(ref _text, value);
	}

	private bool _visible = true;

	public bool Visible
	{
		get => _visible;
		set => ObserveSet(ref _visible, value);
	}

	private bool _open = true;

	public bool Open
	{
		get => _open;
		set => ObserveSet(ref _open, value);
	}
}