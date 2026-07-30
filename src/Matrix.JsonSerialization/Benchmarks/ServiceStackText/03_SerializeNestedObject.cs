using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.NestedJson)]
    public string ServiceStackText()
    {
        var json = JsonSerializer.SerializeToString(_input);
        SerializationChecks.Json(
            LibraryCatalog.ServiceStackText,
            json,
            SerializationData.NestedJson);
        return json;
    }
}
