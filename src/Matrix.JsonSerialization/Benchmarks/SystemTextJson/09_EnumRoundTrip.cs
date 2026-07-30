using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.EnumJson)]
    public EnumModel? SystemTextJson()
    {
        var json = JsonSerializer.Serialize(_input, JsonConfiguration.SystemTextEnum);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.EnumJson);
        var result = JsonSerializer.Deserialize<EnumModel>(
            json,
            JsonConfiguration.SystemTextEnum);
        SerializationChecks.Enum(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
