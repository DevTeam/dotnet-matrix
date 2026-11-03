using System.Diagnostics.CodeAnalysis;
using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Dictionary<string, int>? SpanJson()
    {
        var result = JsonSerializer.Generic.Utf16.Deserialize<Dictionary<string, int>>(Input);
        SerializationChecks.Dictionary(LibraryCatalog.SpanJson, result);
        return result;
    }
}
