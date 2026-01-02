using System.Diagnostics.CodeAnalysis;

namespace SCENeo;

/// <summary>
/// An interface representing a readonly two-way dictionary.
/// </summary>
/// <typeparam name="Key1">The first key type.</typeparam>
/// <typeparam name="Key2">The second key type.</typeparam>
public interface IReadOnlyBiMap<Key1, Key2> : IReadOnlyCollection<KeyValuePair<Key1, Key2>>
{
    /// <summary>
    /// Gets the <typeparamref name="Key2"/> key associated with the <typeparamref name="Key1"/> key.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <returns>The associated <typeparamref name="Key2"/> key.</returns>
    Key2 GetKey2(Key1 key1);

    /// <summary>
    /// Gets the <typeparamref name="Key1"/> key associated with the <typeparamref name="Key2"/> key.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <returns>The associated <typeparamref name="Key1"/> key.</returns>
    Key1 GetKey1(Key2 key2);

    /// <summary>
    /// Gets the <typeparamref name="Key2"/> key or <see langword="default"/> associated with the <typeparamref name="Key1"/> key.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <returns>The associated <typeparamref name="Key2"/> key.</returns>
    Key2? GetKey2OrDefault(Key1 key1);

    /// <summary>
    /// Gets the <typeparamref name="Key1"/> key or <see langword="default"/> associated with the <typeparamref name="Key2"/> key.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <returns>The associated <typeparamref name="Key1"/> key.</returns>
    Key1? GetKey1OrDefault(Key2 key2);

    /// <summary>
    /// Gets the <typeparamref name="Key2"/> key or <paramref name="defaultValue"/> associated with the <typeparamref name="Key1"/> key.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The associated <typeparamref name="Key2"/> key.</returns>
    Key2 GetKey2OrDefault(Key1 key1, Key2 defaultValue);

    /// <summary>
    /// Gets the <typeparamref name="Key1"/> key or <paramref name="defaultValue"/> associated with the <typeparamref name="Key2"/> key.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The associated <typeparamref name="Key1"/> key.</returns>
    Key1 GetKey1OrDefault(Key2 key2, Key1 defaultValue);

    /// <summary>
    /// Gets the <typeparamref name="Key2"/> key associated with the <typeparamref name="Key1"/> key.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <param name="key2">The associated key.</param>
    /// <returns><see langword="true"/> if the <paramref name="key1"/> exists in the dictionary; otherwise, <see langword="false"/>.</returns>
    bool TryGetKey2(Key1 key1, [MaybeNullWhen(false)] out Key2? key2);

    /// <summary>
    /// Gets the <typeparamref name="Key1"/> key associated with the <typeparamref name="Key2"/> key.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <param name="key1">The associated key.</param>
    /// <returns><see langword="true"/> if the <paramref name="key2"/> exists in the dictionary; otherwise, <see langword="false"/>.</returns>
    bool TryGetKey1(Key2 key2, [MaybeNullWhen(false)] out Key1? key1);

    /// <summary>
    /// Determines whether this dictionary contains the given <paramref name="key1"/>.
    /// </summary>
    /// <param name="key1">The key.</param>
    /// <returns><see langword="true"/> if the <paramref name="key1"/> exists in the dictionary; otherwise, <see langword="false"/>.</returns>
    bool ContainsKey1(Key1 key1);

    /// <summary>
    /// Determines whether this dictionary contains the given <paramref name="key2"/>.
    /// </summary>
    /// <param name="key2">The key.</param>
    /// <returns><see langword="true"/> if the <paramref name="key2"/> exists in the dictionary; otherwise, <see langword="false"/>.</returns>
    bool ContainsKey2(Key2 key2);
}
