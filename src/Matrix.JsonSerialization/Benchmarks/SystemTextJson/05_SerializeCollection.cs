using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    public string SystemTextJson()
    {
        var json = JsonSerializer.Serialize(_input, JsonConfiguration.SystemTextDefault);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.CollectionJson);
        return json;
    }
}
