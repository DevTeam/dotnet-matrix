using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.NestedJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public OrderModel? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<OrderModel>(Input);
        SerializationChecks.Nested(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
