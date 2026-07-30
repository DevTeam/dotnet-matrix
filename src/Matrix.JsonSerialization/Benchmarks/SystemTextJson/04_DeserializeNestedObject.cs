using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.NestedJson)]
    public OrderModel? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<OrderModel>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Nested(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
