using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SCENeo;

/// <summary>
/// Represents a two-way dictionary.
/// </summary>
/// <typeparam name="Key1">The first key type.</typeparam>
/// <typeparam name="Key2">The second key type.</typeparam>
public sealed class BiMap<Key1, Key2> : IReadOnlyBiMap<Key1, Key2>
    where Key1 : notnull
    where Key2 : notnull
{
    private readonly Dictionary<Key1, Key2> _tKeyDict;
    private readonly Dictionary<Key2, Key1> _uKeyDict;

    public BiMap()
    {
        _tKeyDict = [];
        _uKeyDict = [];
    }

    public BiMap(int capacity)
    {
        _tKeyDict = new Dictionary<Key1, Key2>(capacity);
        _uKeyDict = new Dictionary<Key2, Key1>(capacity);
    }

    public int Count { get { return _tKeyDict.Count; } }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Capacity"/>
    public int Capacity { get { return _tKeyDict.Capacity; } }

    /// <summary>
    /// Adds the specified pair of keys to the dictionary.
    /// </summary>
    /// <param name="key1">The first key.</param>
    /// <param name="key2">The second key.</param>
    public void Add(Key1 key1, Key2 key2)
    {
        _tKeyDict.Add(key1, key2);
        _uKeyDict.Add(key2, key1);
    }

    /// <summary>
    /// Adds the specified key pair to the dictionary.
    /// </summary>
    /// <param name="pair">The pair of keys.</param>
    public void Add(KeyValuePair<Key1, Key2> pair)
    {
        Add(pair.Key, pair.Value);
    }

    /// <summary>
    /// Removes the key pair associated with <paramref name="key1"/>.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <returns><see langword="true"/> if the key pair was successfully removed; otherwise, <see langword="false"/>.</returns>
    public bool Remove(Key1 key1)
    {
        if (!_tKeyDict.TryGetValue(key1, out Key2 uKey))
        {
            return false;
        }

        _tKeyDict.Remove(key1);

        if (!_uKeyDict.Remove(uKey))
        {
            throw new UnreachableException("Failed to remove uKey.");
        }

        return true;
    }

    /// <summary>
    /// Removes the key pair associated with <paramref name="key2"/>.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <returns><see langword="true"/> if the key pair was successfully removed; otherwise, <see langword="false"/>.</returns>
    public bool Remove(Key2 key2)
    {
        if (!_uKeyDict.TryGetValue(key2, out Key1 tKey))
        {
            return false;
        }

        _uKeyDict.Remove(key2);

        if (!_tKeyDict.Remove(tKey))
        {
            throw new UnreachableException("Failed to remove tKey.");
        }

        return true;
    }

    public Key2 GetKey2(Key1 key2)
    {
        return _tKeyDict[key2];
    }

    public Key1 GetKey1(Key2 key1)
    {
        return _uKeyDict[key1];
    }

    public Key2 GetKey2OrDefault(Key1 tKey)
    {
        return _tKeyDict.GetValueOrDefault(tKey);
    }

    public Key2 GetKey2OrDefault(Key1 tKey, Key2 defaultValue)
    {
        return _tKeyDict.GetValueOrDefault(tKey, defaultValue);
    }

    public Key1 GetKey1OrDefault(Key2 uKey)
    {
        return _uKeyDict.GetValueOrDefault(uKey);
    }

    public Key1 GetKey1OrDefault(Key2 uKey, Key1 defaultValue)
    {
        return _uKeyDict.GetValueOrDefault(uKey, defaultValue);
    }

    public bool TryGetKey2(Key1 tKey, [MaybeNullWhen(false)] out Key2 uKey)
    {
        return _tKeyDict.TryGetValue(tKey, out uKey);
    }

    public bool TryGetKey1(Key2 uKey, [MaybeNullWhen(false)] out Key1 tKey)
    {
        return _uKeyDict.TryGetValue(uKey, out tKey);
    }

    public bool ContainsKey1(Key1 tKey)
    {
        return _tKeyDict.ContainsKey(tKey);
    }

    public bool ContainsKey2(Key2 uKey)
    {
        return _uKeyDict.ContainsKey(uKey);
    }

    /// <summary>
    /// Removes all key pairs in this dictionary.
    /// </summary>
    public void Clear()
    {
        _tKeyDict.Clear();
        _uKeyDict.Clear();
    }

    public IEnumerator<KeyValuePair<Key1, Key2>> GetEnumerator()
    {
        return _tKeyDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}