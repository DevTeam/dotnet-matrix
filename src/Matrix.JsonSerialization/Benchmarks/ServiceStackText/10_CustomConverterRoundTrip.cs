using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class CustomConverterRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.IdentifierJson)]
    public IdentifierModel? ServiceStackText()
    {
        var json = JsonSerializer.SerializeToString(_input);
        SerializationChecks.Json(
            LibraryCatalog.ServiceStackText,
            json,
            SerializationData.IdentifierJson);
        var result = JsonSerializer.DeserializeFromString<IdentifierModel>(json);
        SerializationChecks.Identifier(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
