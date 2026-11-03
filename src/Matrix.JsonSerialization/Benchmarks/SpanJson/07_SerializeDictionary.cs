using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public string SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.DictionaryJson);
        return json;
    }
}
