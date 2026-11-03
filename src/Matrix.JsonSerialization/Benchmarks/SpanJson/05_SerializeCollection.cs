using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    public string SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.CollectionJson);
        return json;
    }
}
