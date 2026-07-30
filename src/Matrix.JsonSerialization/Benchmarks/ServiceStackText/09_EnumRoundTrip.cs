using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.EnumJson)]
    public EnumModel? ServiceStackText()
    {
        var json = JsonSerializer.SerializeToString(_input);
        SerializationChecks.Json(
            LibraryCatalog.ServiceStackText,
            json,
            SerializationData.EnumJson);
        var result = JsonSerializer.DeserializeFromString<EnumModel>(json);
        SerializationChecks.Enum(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
