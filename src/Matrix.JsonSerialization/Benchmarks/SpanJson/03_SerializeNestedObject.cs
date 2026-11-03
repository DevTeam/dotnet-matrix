using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.NestedJson)]
    public string SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.NestedJson);
        return json;
    }
}
