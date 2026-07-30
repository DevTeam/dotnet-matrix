using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SourceGenerationRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? SystemTextJson()
    {
        var typeInfo = MatrixJsonContext.Default.SimpleModel;
        var json = JsonSerializer.Serialize(_input, typeInfo);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.SimpleJson);
        var result = JsonSerializer.Deserialize(json, typeInfo);
        SerializationChecks.Simple(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
