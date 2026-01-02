using System.Collections;

namespace SCENeo.Ui;

public sealed class UpdateCollection<T> : IReadOnlyList<T>, IUpdate
    where T : IUpdate
{
    private readonly List<T> _list = [];

    public event Action? OnUpdate;

    public UpdateCollection()
    {
    }

    public UpdateCollection(int capacity)
    {
        _list = new List<T>(capacity);
    }

    public UpdateCollection(IEnumerable<T> collection)
    {
        AddRange(collection);
    }

    public T this[int index]
    {
        get { return _list[index]; }
        set
        {
            if (Comparer<T>.Default.Compare(value, _list[index]) == 0)
            {
                return;
            }

            Unhook(_list[index]);
            Hook(value);

            _list[index] = value;
        }
    }

    public int Count { get { return _list.Count; } }

    public int Capacity { get { return _list.Capacity; } }

    public void Add(T item)
    {
        _list.Add(item);

        Hook(item);
    }

    public void AddRange(IEnumerable<T> collection)
    {
        foreach (T item in collection)
        {
            Add(item);
        }
    }

    public bool Remove(T item)
    {
        if (!_list.Remove(item))
        {
            return false;
        }

        Unhook(item);

        return true;
    }

    public void RemoveAt(int index)
    {
        Unhook(_list[index]);

        _list.RemoveAt(index);
    }

    public void RemoveRange(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            RemoveAt(index + i);
        }
    }

    public void Clear()
    {
        foreach (T item in _list)
        {
            Unhook(item);
        }

        _list.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Hook(T item)
    {
        if (item != null)
        {
            item.OnUpdate += Item_Update;
        }
    }

    private void Unhook(T item)
    {
        if (item != null)
        {
            item.OnUpdate -= Item_Update;
        }
    }

    private void Item_Update()
    {
        OnUpdate?.Invoke();
    }
}
