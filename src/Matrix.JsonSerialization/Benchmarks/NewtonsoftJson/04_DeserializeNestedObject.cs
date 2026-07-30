using Newtonsoft.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.NestedJson)]
    public OrderModel? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<OrderModel>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Nested(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
