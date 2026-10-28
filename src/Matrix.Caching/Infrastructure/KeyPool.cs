namespace Matrix.Caching.Infrastructure;

/// <summary>
/// Key strings are built once, here, so a measured write or removal never pays
/// for formatting or for a fresh allocation. Each pool hands out keys in order
/// and wraps around, so an operation that needs a key nobody has touched yet
/// stays repeatable over a long benchmark run.
/// </summary>
internal sealed class KeyPool
{
    private readonly string[] _keys;
    private int _next = -1;

    public KeyPool(string prefix, int count)
    {
        _keys = new string[count];
        for (var index = 0; index < count; index++)
        {
            _keys[index] = prefix + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public IReadOnlyList<string> Keys => _keys;

    public string Next()
    {
        var index = Interlocked.Increment(ref _next) & int.MaxValue;
        return _keys[index % _keys.Length];
    }
}