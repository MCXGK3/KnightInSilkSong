using System.Collections;
using Unity.Collections;

public class DefaultDict<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly Dictionary<TKey, TValue> _dict = new();
    private readonly Func<TValue> _factory;

    public DefaultDict(Func<TValue> factory)
    {
        _factory = factory;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (!_dict.TryGetValue(key, out var value))
            {
                value = _factory();
                _dict[key] = value;
            }
            return value;
        }
        set => _dict[key] = value;
    }

    public bool TryGetValue(TKey key, out TValue value)
        => _dict.TryGetValue(key, out value);


    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var key in Keys)
        {
            yield return new(key, this[key]);
        }
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<TKey> Keys => _dict.Keys;
}
public static class DictUtils
{

}