using System.Collections;

namespace SCENeo;

/// <summary>
/// A class representing a fixed length array that can be virtually shifted.
/// </summary>
/// <typeparam name="T">The stored type.</typeparam>
public sealed class ShiftBuffer<T> : IReadOnlyList<T>
{
    private readonly T[] _buffer;
    private int _head;

    public ShiftBuffer()
    {
        _buffer = [];
    }

    public ShiftBuffer(int length)
    {
        _buffer = new T[length];
    }

    public int Count { get { return _buffer.Length; } }

    /// <summary>
    /// Gets or sets the item at the given virtual index.
    /// </summary>
    /// <param name="index">The virtual index.</param>
    /// <returns>The item at the given virtual index.</returns>
    public T this[int index]
    {
        get { return _buffer[Translate(index)]; }
        set { _buffer[Translate(index)] = value; }
    }

    /// <summary>
    /// Virtually shifts all the contents of the buffer by the given amount.
    /// </summary>
    /// <param name="shift">The amount to shift.</param>
    public void Shift(int shift)
    {
        _head = SCEMath.Mod(_head + shift, _buffer.Length);
    }

    /// <summary>
    /// Sets all the values in the given range to <see langword="default"/>.
    /// </summary>
    /// <param name="start">The start virtual index.</param>
    /// <param name="count">The number of items to remove.</param>
    public void Remove(int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            this[start + i] = default!;
        }
    }

    /// <summary>
    /// Clears the contents of the buffer.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_buffer);
    }

    public void Fill(T value)
    {
        Array.Fill(_buffer, value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _buffer.Length; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private int Translate(int index)
    {
        return (_head + index) % _buffer.Length;
    }
}