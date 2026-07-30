using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class CustomConverterRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.IdentifierJson)]
    public IdentifierModel? SystemTextJson()
    {
        var json = JsonSerializer.Serialize(_input, JsonConfiguration.SystemTextIdentifier);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.IdentifierJson);
        var result = JsonSerializer.Deserialize<IdentifierModel>(
            json,
            JsonConfiguration.SystemTextIdentifier);
        SerializationChecks.Identifier(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
