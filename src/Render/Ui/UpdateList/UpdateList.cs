using System.Collections;

namespace SCENeo.Ui;

/// <summary>
/// Represents a list of update items.
/// </summary>
/// <typeparam name="T">The stored <see cref="IUpdate"/> type.</typeparam>
public class UpdateList<T> : IList<T>, IUpdate
    where T : IUpdate
{
    private readonly List<T> _list = [];

    public event Action Updated;

    public UpdateList()
    {
    }

    public UpdateList(int capacity)
    {
        _list = new List<T>(capacity);
    }

    public UpdateList(IEnumerable<T> collection)
    {
        AddRange(collection);
    }

    public T this[int index]
    {
        get => _list[index];
        set
        {
            if (EqualityComparer<T>.Default.Equals(value, _list[index]))
            {
                return;
            }

            Unhook(_list[index]);
            Hook(value);

            _list[index] = value;
            Updated?.Invoke();
        }
    }

    public int Count => _list.Count;
    public int Capacity => _list.Capacity;

    public bool IsReadOnly => false;

    public virtual void Add(T item)
    {
        _list.Add(item);
        Hook(item);

        Item_Update();
    }

    public void AddRange(IEnumerable<T> collection)
    {
        foreach (T item in collection)
        {
            _list.Add(item);
            Hook(item);
        }

        Item_Update();
    }

    public bool Remove(T item)
    {
        int index = _list.IndexOf(item);

        if (index == -1)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    public virtual void RemoveAt(int index)
    {
        var item = _list[index];
        
        Unhook(item);

        _list.RemoveAt(index);

        Item_Update();
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    } 

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);

        Hook(item);
    }

    public void RemoveRange(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Unhook(_list[index + i]);
            _list.RemoveAt(index + i);
        }

        Item_Update();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public void Clear()
    {
        foreach (T item in _list)
        {
            Unhook(item);
        }

        _list.Clear();

        Item_Update();
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
            item.Updated += Item_Update;
        }
    }

    private void Unhook(T item)
    {
        if (item != null)
        {
            item.Updated -= Item_Update;
        }
    }

    private void Item_Update()
    {
        Updated?.Invoke();
    }
}
