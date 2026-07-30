using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.NestedJson)]
    public OrderModel? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<OrderModel>(Input);
        SerializationChecks.Nested(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
