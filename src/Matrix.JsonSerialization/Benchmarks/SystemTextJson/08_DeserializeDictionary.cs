using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Dictionary<string, int>? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<Dictionary<string, int>>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Dictionary(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
