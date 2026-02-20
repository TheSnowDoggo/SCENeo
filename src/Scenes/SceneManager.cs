using System.Collections;

namespace SCENeo.Scenes;

public class SceneManager : Scene, IReadOnlyList<Scene>
{
	private readonly List<Scene> _scenes = [];

	public int Count => _scenes.Count;
	
	public Scene InputFocus { get; set; }
	
	public Scene this[int index] => _scenes[index];

	public void Add(Scene scene)
	{
		_scenes.Add(scene);
		scene.Parent = this;
	}

	public bool Remove(Scene scene)
	{
		if (!_scenes.Remove(scene))
		{
			return false;
		}
		scene.Parent = null;
		return true;
	}

	public override void Open()
	{
		base.Open();
		foreach (var scene in _scenes)
		{
			scene.Open();
		}
	}
	
	public override void Close()
	{
		base.Close();
		foreach (var scene in _scenes)
		{
			scene.Close();
		}
	}

	public override void Start()
	{
		foreach (var scene in _scenes)
		{
			scene.Start();
		}
	}

	public override void Update(double delta)
	{
		foreach (var scene in _scenes)
		{
			if (!scene.Enabled)
			{
				continue;
			}
			
			scene.Update(delta);
		}
	}

	public override IEnumerable<IRenderable> Render()
	{
		foreach (var scene in _scenes)
		{
			if (!scene.Enabled || !scene.Visible)
			{
				continue;
			}

			foreach (var renderable in scene.Render())
			{
				yield return renderable;
			}
		}
	}

	public override void DisplayResize(Vec2I size)
	{
		foreach (var scene in _scenes)
		{
			scene.DisplayResize(size);
		}
	}

	public override void UnfocusedInput(ConsoleKeyInfo cki)
	{
		var focus = InputFocus;
		
		foreach (var scene in _scenes)
		{
			if (!scene.Enabled)
			{
				continue;
			}
			
			if (focus == scene)
			{
				scene.FocusedInput(cki);
				continue;
			}
			
			if (focus == null)
			{
				scene.UnfocusedInput(cki);
			}
			
			scene.RawInput(cki);
		}
	}

	public IEnumerator<Scene> GetEnumerator()
	{
		return _scenes.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}