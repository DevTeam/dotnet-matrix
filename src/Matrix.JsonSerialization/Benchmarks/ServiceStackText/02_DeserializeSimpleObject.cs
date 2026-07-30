using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<SimpleModel>(Input);
        SerializationChecks.Simple(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
