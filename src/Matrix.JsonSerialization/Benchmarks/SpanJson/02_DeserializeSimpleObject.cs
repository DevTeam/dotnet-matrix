using System.Diagnostics.CodeAnalysis;
using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel? SpanJson()
    {
        var result = JsonSerializer.Generic.Utf16.Deserialize<SimpleModel>(Input);
        SerializationChecks.Simple(LibraryCatalog.SpanJson, result);
        return result;
    }
}
