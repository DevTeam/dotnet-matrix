using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.CollectionJson)]
    public string ServiceStackText()
    {
        var json = JsonSerializer.SerializeToString(_input);
        SerializationChecks.Json(
            LibraryCatalog.ServiceStackText,
            json,
            SerializationData.CollectionJson);
        return json;
    }
}
