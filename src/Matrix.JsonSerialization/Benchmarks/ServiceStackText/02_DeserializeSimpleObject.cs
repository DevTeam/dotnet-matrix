using System.Diagnostics.CodeAnalysis;
using ServiceStack.Text;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.SimpleJson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public SimpleModel? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<SimpleModel>(Input);
        SerializationChecks.Simple(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
