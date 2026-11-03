using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.EnumJson)]
    public EnumModel? SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.EnumJson);
        var result = JsonSerializer.Generic.Utf16.Deserialize<EnumModel>(json);
        SerializationChecks.Enum(LibraryCatalog.SpanJson, result);
        return result;
    }
}
