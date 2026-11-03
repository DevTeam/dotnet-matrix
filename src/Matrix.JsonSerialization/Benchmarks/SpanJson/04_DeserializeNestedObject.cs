using System.Diagnostics.CodeAnalysis;
using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.NestedJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public OrderModel? SpanJson()
    {
        var result = JsonSerializer.Generic.Utf16.Deserialize<OrderModel>(Input);
        SerializationChecks.Nested(LibraryCatalog.SpanJson, result);
        return result;
    }
}
