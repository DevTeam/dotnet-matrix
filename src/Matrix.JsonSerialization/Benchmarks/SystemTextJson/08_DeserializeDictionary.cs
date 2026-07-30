using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeDictionary
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.DictionaryJson)]
    public Dictionary<string, int>? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<Dictionary<string, int>>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Dictionary(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
