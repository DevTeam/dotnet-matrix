using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class DeserializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? SystemTextJson()
    {
        var result = JsonSerializer.Deserialize<SimpleModel>(
            Input,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Simple(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
