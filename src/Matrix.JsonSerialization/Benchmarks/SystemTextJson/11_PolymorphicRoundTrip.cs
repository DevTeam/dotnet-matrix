using System.Text.Json;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class PolymorphicRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.PolymorphicJson)]
    public ZooModel? SystemTextJson()
    {
        var json = JsonSerializer.Serialize(_input, JsonConfiguration.SystemTextDefault);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.PolymorphicJson);
        var result = JsonSerializer.Deserialize<ZooModel>(
            json,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Zoo(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
