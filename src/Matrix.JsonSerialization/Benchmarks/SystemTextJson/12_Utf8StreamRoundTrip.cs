using System.Text.Json;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class Utf8StreamRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemTextJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? SystemTextJson()
    {
        using var stream = new MemoryStream(128);
        JsonSerializer.Serialize(stream, _input, JsonConfiguration.SystemTextDefault);
        SerializationChecks.Bytes(
            LibraryCatalog.SystemTextJson,
            stream.ToArray(),
            SerializationData.SimpleJson);
        stream.Position = 0;
        var result = JsonSerializer.Deserialize<SimpleModel>(
            stream,
            JsonConfiguration.SystemTextDefault);
        SerializationChecks.Simple(LibraryCatalog.SystemTextJson, result);
        return result;
    }
}
