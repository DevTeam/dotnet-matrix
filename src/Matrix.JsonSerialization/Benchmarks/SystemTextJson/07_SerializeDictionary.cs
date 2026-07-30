using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class SerializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public string SystemTextJson()
    {
        var json = JsonSerializer.Serialize(_input, JsonConfiguration.SystemTextDefault);
        SerializationChecks.Json(
            LibraryCatalog.SystemTextJson,
            json,
            SerializationData.DictionaryJson);
        return json;
    }
}
