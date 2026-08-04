using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.NestedJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public OrderModel? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<OrderModel>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Nested(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
