namespace SCENeo.Ui;

public abstract class UpdateBase : IUpdate
{
	public event Action Updated;

	protected void Update()
	{
		Updated?.Invoke();
	}
	
	protected void ObserveSet<T>(ref T field, T value)
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
		{
			return;
		}

		field = value;
		Updated?.Invoke();
	}
	
	protected void ObserveSet<T>(ref T field, T value, Action updateCallback)
		where T : IUpdate
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
		{
			return;
		}

		if (field != null)
		{
			field.Updated -= updateCallback;
		}

		if (value != null)
		{
			value.Updated += updateCallback;
		}

		field = value;
		Updated?.Invoke();
	}
}