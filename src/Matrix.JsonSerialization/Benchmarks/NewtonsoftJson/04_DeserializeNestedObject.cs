using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.NestedJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public OrderModel? NewtonsoftJson()
    {
        var result = JsonConvert.DeserializeObject<OrderModel>(
            Input,
            JsonConfiguration.NewtonsoftDefault);
        SerializationChecks.Nested(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
