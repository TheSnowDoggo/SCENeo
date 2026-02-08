using System.Collections;

namespace SCENeo.Ui;

/// <summary>
/// Represents a list of update items.
/// </summary>
/// <typeparam name="T">The stored <see cref="IUpdate"/> type.</typeparam>
public sealed class UpdateList<T> : IList<T>, IUpdate
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

    public int Count => _list.Count;

    public int Capacity => _list.Capacity;

    public bool IsReadOnly => false;

    public void Add(T item)
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
        if (!_list.Remove(item))
        {
            return false;
        }

        Unhook(item);

        Item_Update();

        return true;
    }

    public void RemoveAt(int index)
    {
        Unhook(_list[index]);

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
