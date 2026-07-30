using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.CollectionJson)]
    public SimpleModel[]? ServiceStackText()
    {
        var result = JsonSerializer.DeserializeFromString<SimpleModel[]>(Input);
        SerializationChecks.Collection(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
