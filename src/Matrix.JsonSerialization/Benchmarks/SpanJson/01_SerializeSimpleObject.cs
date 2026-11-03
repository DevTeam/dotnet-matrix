using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public string SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.SimpleJson);
        return json;
    }
}
