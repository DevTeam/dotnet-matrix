using SpanJson;
// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

public partial class Utf8StreamRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpanJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? SpanJson()
    {
        using var stream = new MemoryStream(128);
        JsonSerializer.Generic.Utf8.SerializeAsync(_input, stream, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        SerializationChecks.Bytes(
            LibraryCatalog.SpanJson,
            stream.ToArray(),
            SerializationData.SimpleJson);
        stream.Position = 0;
        var result = JsonSerializer.Generic.Utf8
            .DeserializeAsync<SimpleModel>(stream, CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        SerializationChecks.Simple(LibraryCatalog.SpanJson, result);
        return result;
    }
}
