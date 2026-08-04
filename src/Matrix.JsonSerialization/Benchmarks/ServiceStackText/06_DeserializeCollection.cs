using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.CollectionJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel[]? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<SimpleModel[]>(Input);
        SerializationChecks.Collection(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
