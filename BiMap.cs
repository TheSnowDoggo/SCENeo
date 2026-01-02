using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SCENeo;

internal sealed class BiMap<TKey, UKey> : IReadOnlyBiMap<TKey, UKey>
    where TKey : notnull
    where UKey : notnull
{
    private readonly Dictionary<TKey, UKey> _tKeyDict;
    private readonly Dictionary<UKey, TKey> _uKeyDict;

    public BiMap()
    {
        _tKeyDict = [];
        _uKeyDict = [];
    }

    public BiMap(int capacity)
    {
        _tKeyDict = new Dictionary<TKey, UKey>(capacity);
        _uKeyDict = new Dictionary<UKey, TKey>(capacity);
    }

    public int Count { get { return _tKeyDict.Count; } }

    public int Capacity { get { return _tKeyDict.Capacity; } }

    public void Add(TKey tKey, UKey uKey)
    {
        _tKeyDict.Add(tKey, uKey);
        _uKeyDict.Add(uKey, tKey);
    }

    public void Add(KeyValuePair<TKey, UKey> pair)
    {
        Add(pair.Key, pair.Value);
    }

    public bool Remove(TKey tKey)
    {
        if (!_tKeyDict.TryGetValue(tKey, out UKey? uKey))
        {
            return false;
        }

        _tKeyDict.Remove(tKey);

        if (!_uKeyDict.Remove(uKey))
        {
            throw new UnreachableException("Failed to remove uKey.");
        }

        return true;
    }

    public bool Remove(UKey uKey)
    {
        if (!_uKeyDict.TryGetValue(uKey, out TKey? tKey))
        {
            return false;
        }

        _uKeyDict.Remove(uKey);

        if (!_tKeyDict.Remove(tKey))
        {
            throw new UnreachableException("Failed to remove tKey.");
        }

        return true;
    }

    public UKey GetUKey(TKey tKey)
    {
        return _tKeyDict[tKey];
    }

    public TKey GetTKey(UKey uKey)
    {
        return _uKeyDict[uKey];
    }

    public UKey? GetUKeyOrDefault(TKey tKey)
    {
        return _tKeyDict.GetValueOrDefault(tKey);
    }

    public UKey GetUKeyOrDefault(TKey tKey, UKey defaultValue)
    {
        return _tKeyDict.GetValueOrDefault(tKey, defaultValue);
    }

    public TKey? GetTKeyOrDefault(UKey uKey)
    {
        return _uKeyDict.GetValueOrDefault(uKey);
    }

    public TKey GetUKeyOrDefault(UKey uKey, TKey defaultValue)
    {
        return _uKeyDict.GetValueOrDefault(uKey, defaultValue);
    }

    public bool TryGetUKey(TKey tKey, [MaybeNullWhen(false)] out UKey? uKey)
    {
        return _tKeyDict.TryGetValue(tKey, out uKey);
    }

    public bool TryGetTKey(UKey uKey, [MaybeNullWhen(false)] out TKey? tKey)
    {
        return _uKeyDict.TryGetValue(uKey, out tKey);
    }

    public bool ContainsTKey(TKey tKey)
    {
        return _tKeyDict.ContainsKey(tKey);
    }

    public bool ContainsUKey(UKey uKey)
    {
        return _uKeyDict.ContainsKey(uKey);
    }

    public void Clear()
    {
        _tKeyDict.Clear();
        _uKeyDict.Clear();
    }

    #region IEnumerable

    public IEnumerator<KeyValuePair<TKey, UKey>> GetEnumerator()
    {
        return _tKeyDict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}