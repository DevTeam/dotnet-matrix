using System.Text;
using Newtonsoft.Json;
using NewtonsoftSerializer = Newtonsoft.Json.JsonSerializer;

namespace Matrix.JsonSerialization.Benchmarks;

public partial class Utf8StreamRoundTrip
{
    private readonly NewtonsoftSerializer _newtonsoftStreamSerializer =
        NewtonsoftSerializer.Create(JsonConfiguration.NewtonsoftDefault);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NewtonsoftJson)]
    [PayloadSize(SerializationData.SimpleJson)]
    public SimpleModel? NewtonsoftJson()
    {
        using var stream = new MemoryStream(128);
        using (var textWriter = new StreamWriter(
                   stream,
                   new UTF8Encoding(false),
                   128,
                   true))
        using (var jsonWriter = new JsonTextWriter(textWriter))
        {
            _newtonsoftStreamSerializer.Serialize(jsonWriter, _input);
            jsonWriter.Flush();
        }

        SerializationChecks.Bytes(
            LibraryCatalog.NewtonsoftJson,
            stream.ToArray(),
            SerializationData.SimpleJson);
        stream.Position = 0;
        using var textReader = new StreamReader(
            stream,
            Encoding.UTF8,
            false,
            128,
            true);
        using var jsonReader = new JsonTextReader(textReader);
        var result = _newtonsoftStreamSerializer.Deserialize<SimpleModel>(jsonReader);
        SerializationChecks.Simple(LibraryCatalog.NewtonsoftJson, result);
        return result;
    }
}
