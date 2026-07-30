using ServiceStack.Text;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class Utf8StreamRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ServiceStackText)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? ServiceStackText()
    {
        using var stream = new MemoryStream(128);
        JsonSerializer.SerializeToStream(_input, stream);
        SerializationChecks.Bytes(
            LibraryCatalog.ServiceStackText,
            stream.ToArray(),
            SerializationData.SimpleJson);
        stream.Position = 0;
        var result = JsonSerializer.DeserializeFromStream<SimpleModel>(stream);
        SerializationChecks.Simple(LibraryCatalog.ServiceStackText, result);
        return result;
    }
}
