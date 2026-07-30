using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeCollection
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.CollectionJson)]
    public SimpleModel[]? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<SimpleModel[]>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Collection(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
