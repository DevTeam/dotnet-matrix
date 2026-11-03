using System.Diagnostics.CodeAnalysis;
using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel[]? SpanJson()
    {
        var result = JsonSerializer.Generic.Utf16.Deserialize<SimpleModel[]>(Input);
        SerializationChecks.Collection(LibraryCatalog.SpanJson, result);
        return result;
    }
}
