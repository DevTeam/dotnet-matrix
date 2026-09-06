using System.Text.Json;

namespace Matrix.Tests;

internal sealed class MemoryReportStore : IMatrixReportStore
{
    private readonly Dictionary<string, object> _values = new(StringComparer.OrdinalIgnoreCase);
    public int Writes { get; private set; }
    public int Reads { get; private set; }
    public bool RequireWebOptions { get; set; }

    public bool Exists(string fileName) => _values.ContainsKey(Path.GetFullPath(fileName));

    public T? Read<T>(string fileName, JsonSerializerOptions? options = null)
    {
        if (RequireWebOptions && options?.PropertyNameCaseInsensitive != true)
        {
            throw new InvalidOperationException("Presentation readers must preserve Web JSON settings.");
        }

        Reads++;
        return _values.TryGetValue(Path.GetFullPath(fileName), out var value) ? (T)value : default;
    }

    public void Write<T>(string fileName, T value)
    {
        _values[Path.GetFullPath(fileName)] = value!;
        Writes++;
    }

    public void WarnEnvironmentMismatch(
        IReadOnlyCollection<BenchmarkEnvironment> existing,
        BenchmarkEnvironment current)
    {
    }
}
