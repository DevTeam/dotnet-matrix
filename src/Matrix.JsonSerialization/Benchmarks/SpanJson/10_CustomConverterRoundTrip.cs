using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class CustomConverterRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.IdentifierJson)]
    public IdentifierModel? SpanJson()
    {
        var json = JsonSerializer.Generic.Utf16.Serialize<IdentifierModel, SpanJsonResolver<char>>(_input);
        SerializationChecks.Json(
            LibraryCatalog.SpanJson,
            json,
            SerializationData.IdentifierJson);
        var result = JsonSerializer.Generic.Utf16.Deserialize<IdentifierModel, SpanJsonResolver<char>>(json);
        SerializationChecks.Identifier(LibraryCatalog.SpanJson, result);
        return result;
    }
}
