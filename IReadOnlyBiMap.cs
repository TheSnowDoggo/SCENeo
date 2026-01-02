using System.Diagnostics.CodeAnalysis;

namespace SCENeo;

public interface IReadOnlyBiMap<TKey, UKey> : IReadOnlyCollection<KeyValuePair<TKey, UKey>>
{
    UKey GetUKey(TKey tKey);
    TKey GetTKey(UKey uKey);

    UKey? GetUKeyOrDefault(TKey tKey);
    UKey GetUKeyOrDefault(TKey tKey, UKey defaultValue);

    TKey? GetTKeyOrDefault(UKey uKey);
    TKey GetUKeyOrDefault(UKey uKey, TKey defaultValue);

    bool TryGetUKey(TKey tKey, [MaybeNullWhen(false)] out UKey? uKey);
    bool TryGetTKey(UKey uKey, [MaybeNullWhen(false)] out TKey? tKey);

    bool ContainsTKey(TKey tKey);
    bool ContainsUKey(UKey uKey);
}
