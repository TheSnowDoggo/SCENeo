namespace SCENeo.Scenes;

public abstract class Scene
{
	public bool Enabled { get; set; }
	public bool Visible { get; set; } = true;
	
	public SceneManager Parent { get; set; }

	public bool Focused => Parent != null && Parent.InputFocus == this;

	public void ChangeTo(Scene scene)
	{
		Close();
		scene.Open();
	}
	
	public virtual void Open()
	{
		Enabled = true;
	}

	public virtual void Close()
	{
		Enabled = false;
	}
	
	public virtual void Start()
	{
	}
	
	public virtual void Update(double delta)
	{
	}

	public virtual void DisplayResize(Vec2I size)
	{
	}

	public virtual IEnumerable<IRenderable> Render()
	{
		yield break;
	}

	public virtual void RawInput(ConsoleKeyInfo cki)
	{
	}

	public virtual void FocusedInput(ConsoleKeyInfo cki)
	{
	}
	
	public virtual void UnfocusedInput(ConsoleKeyInfo cki)
	{
	}
}